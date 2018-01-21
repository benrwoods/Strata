using Strata.API.Model.Shopping;
using Strata.Data.Shopping;
using System.Linq;

namespace Strata.API.Converter {
    public static class OrderConverter {

        public static OrderDTO ToDTO(this Order order) {
            return new OrderDTO {
                Id = order.Id,
                Items = order.Items?
                    .Select(x => x.Item)
                    .GroupBy(x => x.Id)
                    .Select(x => new ItemDTO {
                        Name = x.First().Name,
                        Quantity = x.Count(),
                        Cost = x.First().Cost * x.Count()
                    }),
                Cost = order.Cost,
                OrderDate = order.OrderDate,
                ShippingDate = order.ShippingDate
            };
        }

        public static OrderListDTO ToDTOList(this Order order) {
            return new OrderListDTO {
                Id = order.Id,
                Cost = order.Cost
            };
        }
    }
}
