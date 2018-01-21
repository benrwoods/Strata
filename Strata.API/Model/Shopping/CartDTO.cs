using Strata.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Strata.API.Model.Shopping {
    public class CartDTO {
        public IEnumerable<ItemDTO> Items { get; set; }

        public int DiscountPercent { get; set; }

        public double TotalCost {
            get {
                return Math.Round(CostUtil.ApplyDiscount(this.Items.Sum(x => x.Cost), this.DiscountPercent), 2);
            }
        }
    }
}
