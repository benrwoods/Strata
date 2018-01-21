using Microsoft.EntityFrameworkCore;
using Strata.Data;
using Strata.Data.Security;
using Strata.Interfaces.Messaging;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strata.Services.Messaging {
    public class MessagingService : IMessagingService {

        private ShoppingCartContext DbContext { get; }

        public MessagingService(ShoppingCartContext dbContext) {
            this.DbContext = dbContext;
        }

        public async Task SendOrderConfirmation(int orderId) {
            var order = await this.DbContext.Orders
                .Where(x => x.Id == orderId)
                .Include(x => x.Items)
                .FirstOrDefaultAsync();

            var messageBuilder = new StringBuilder();
            messageBuilder.AppendFormat("Order #{0} – Estimated dispatch {1}",
                order.Id.ToString("D5"), order.ShippingDate.ToShortDateString());
            messageBuilder.AppendLine();

            var itemsById = order.Items
                .Select(x => x.Item)
                .GroupBy(x => x.Id);

            foreach (var itemGroup in itemsById) {
                messageBuilder.AppendFormat("{0} x{1} = £{2}",
                    itemGroup.First().Name, itemGroup.Count(), itemGroup.First().Cost * itemGroup.Count());
                messageBuilder.AppendLine();
            }

            if (order.DiscountLevel != UserLevel.Standard) {
                messageBuilder.AppendFormat("Total = £{0}", order.Items.Select(x => x.Item).Sum(x => x.Cost));
            }
            else {
                messageBuilder.AppendFormat("Total(includes {0}% {1} discount) = £{2}",
                   order.DiscountPercent, order.DiscountLevel.ToString(), order.Items.Select(x => x.Item).Sum(x => x.Cost));
            }

            SendMessage(messageBuilder.ToString());
        }

        private void SendMessage(string message) {
            Console.Write(message); //Send message somewhere
        }
    }
}