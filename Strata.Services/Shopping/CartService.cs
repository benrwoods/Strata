﻿using Microsoft.EntityFrameworkCore;
using Strata.Data;
using Strata.Data.Shopping;
using Strata.Interfaces.Messaging;
using Strata.Interfaces.Security;
using Strata.Interfaces.Shopping;
using Strata.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Strata.Services.Shopping {
    public class CartService : ICartService {

        private ShoppingCartContext DbContext { get; }
        private IUserService UserService { get; }
        private IMessagingService MessagingService { get; }

        public CartService(ShoppingCartContext dbContext, IUserService userService, IMessagingService messagingService) {
            this.DbContext = dbContext;
            this.UserService = userService;
            this.MessagingService = messagingService;
        }

        /// <summary>
        /// Gets users cart, if user doesn't have a cart creates a new one
        /// </summary>
        /// <returns></returns>
        public async Task<Cart> GetOrCreateCart() {
            var cart = await this.DbContext.Carts
                .Join(this.UserService.CurrentUserQuery, ok => ok.UserId, ik => ik.Id, (o, i) => o)
                .Include(x => x.Items)
                .ThenInclude(x => x.Item)
                .FirstOrDefaultAsync();

            if (cart == null) {
                var userId = await this.UserService.CurrentUserQuery.Select(x => x.Id).FirstOrDefaultAsync();

                cart = new Cart {
                    UserId = userId
                };
                this.DbContext.Carts.Add(cart);
                await this.DbContext.SaveChangesAsync();
            }

            cart.Discount = await this.UserService.GetUserDiscount();
            cart.DiscountLevel = await this.UserService.GetUserLevel();

            return cart;
        }

        public async Task<bool> BuyCart() {
            var cart = await GetOrCreateCart();

            if (cart.Items == null || !cart.Items.Any()) {
                //noting to buy
                return true;
            }

            var itemsToBuy = cart.Items.Select(x => x.Item);
            var totalCost = CostUtil.ApplyDiscount(itemsToBuy.Sum(x => x.Cost), cart.Discount);

            var user = await this.UserService.CurrentUserQuery.FirstOrDefaultAsync();
            var avaliableBalance = await this.UserService.GetAvaliableBalance();

            if (avaliableBalance >= totalCost) {
                var order = new Order {
                    Cost = totalCost,
                    DiscountPercent = cart.Discount,
                    OrderDate = DateTime.Now,
                    ShippingDate = DateTime.Now.GetDateInXWorkingDays(3),
                    UserId = user.Id,
                    DiscountLevel = cart.DiscountLevel
                };

                this.DbContext.Add(order);

                var orderItems = itemsToBuy.Select(x => new OrderItem { Item = x, Order = order });

                foreach (var orderItem in orderItems) {
                    this.DbContext.Add(orderItem);
                }

                user.Balance -= totalCost;

                ClearCart(cart);
                await this.DbContext.SaveChangesAsync();

                //Now that the order has been created, send the confirmation
                await this.MessagingService.SendOrderConfirmation(order.Id);
                return true;
            }

            return false;

        }

        private void ClearCart(Cart cart) {

            foreach (var item in cart.Items) {
                this.DbContext.CartItems.Remove(item);
            }

            this.DbContext.Carts.Remove(cart);
        }
    }
}
