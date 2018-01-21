using Strata.Data.Security;
using System;
using System.Collections.Generic;

namespace Strata.Data.Shopping {
    public class Order {

        public int Id { get; set; }

        public int UserId { get; set; }

        public double Cost { get; set; }

        public DateTime ShippingDate { get; set; }

        public DateTime OrderDate { get; set; }

        public int DiscountPercent { get; set; }

        public UserLevel DiscountLevel { get; set; }

        public ICollection<OrderItem> Items { get; set; }
    }
}
