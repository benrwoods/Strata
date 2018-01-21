using Strata.Data.Security;
using Strata.Data.Shopping;
using Strata.Utils;
using System;
using System.Linq;

namespace Strata.Data {
    public class DbInitializer {

        public static void Initialize(ShoppingCartContext dbContext) {
            dbContext.Database.EnsureCreated();

            // Look for any users
            if (dbContext.Users.Any()) {
                return;   // DB has been seeded
            }

            var users = new User[] {
               new User { Login = "standard", Password = "standard", Balance = 1000 },
               new User { Login = "silver", Password = "silver", Balance = 2000 },
               new User { Login = "gold", Password = "gold", Balance = 500 }
            };
            foreach (var user in users) {
                dbContext.Users.Add(user);
            }
            dbContext.SaveChanges();

            var items = new Item[] {
                new Item { Name = "Phone", Cost = 699.99 },
                new Item { Name = "Case", Cost = 9.99 },
                new Item { Name = "Laptop", Cost = 999.99 }
            };
            foreach (var item in items) {
                dbContext.Items.Add(item);
            }
            dbContext.SaveChanges();

            var carts = new Cart[] {
                new Cart { UserId = 1 },
                new Cart { UserId = 3 }
            };
            foreach (var cart in carts) {
                dbContext.Carts.Add(cart);
            }
            dbContext.SaveChanges();

            var cartItems = new CartItem[] {
                new CartItem { CartId = 1, ItemId = 1 },
                new CartItem { CartId = 1, ItemId = 1 },
                new CartItem { CartId = 1, ItemId = 2 },
                new CartItem { CartId = 2, ItemId = 2 },

            };
            foreach (var cartItem in cartItems) {
                dbContext.CartItems.Add(cartItem);
            }
            dbContext.SaveChanges();

            var orders = new Order[] {
                new Order { Cost = 9.99, DiscountPercent = 0, OrderDate = DateTime.Now, UserId = 1,
                    ShippingDate = DateTime.Now.GetDateInXWorkingDays(3), DiscountLevel = UserLevel.Standard },

                new Order { Cost = 1999.98, DiscountPercent = 0, OrderDate = DateTime.Now, UserId = 3,
                    ShippingDate = DateTime.Now.GetDateInXWorkingDays(3), DiscountLevel = UserLevel.Standard }
            };
            foreach (var order in orders) {
                dbContext.Orders.Add(order);
            }
            dbContext.SaveChanges();

            var orderItems = new OrderItem[] {
                new OrderItem { ItemId = 2, OrderId = 1},
                new OrderItem { ItemId = 3, OrderId = 2},
                new OrderItem { ItemId = 3, OrderId = 2}
            };
            foreach (var orderItem in orderItems) {
                dbContext.OrderItems.Add(orderItem);
            }
            dbContext.SaveChanges();
        }
    }
}