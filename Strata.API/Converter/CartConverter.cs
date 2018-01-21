using Strata.API.Model.Shopping;
using Strata.Data.Shopping;
using System;
using System.Linq;

namespace Strata.API.Converter {
    public static class CartConverter {

        public static CartDTO ToDTO(this Cart cart) {
            return new CartDTO {
                Items = cart.Items?
                    .Select(x => x.Item)
                    .GroupBy(x => x.Id)
                    .Select(x => new ItemDTO {
                        Name = x.First().Name,
                        Quantity = x.Count(),
                        Cost = Math.Round(x.First().Cost * x.Count(), 2)
                    }),
                DiscountPercent = cart.Discount
            };
        }
    }
}
