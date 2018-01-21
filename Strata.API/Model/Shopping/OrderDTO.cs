using System;
using System.Collections.Generic;

namespace Strata.API.Model.Shopping {
    public class OrderDTO {

        public int Id { get; set; }

        public double Cost { get; set; }

        public IEnumerable<ItemDTO> Items { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime ShippingDate { get; set; }

    }
}
