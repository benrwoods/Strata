using Strata.Data.Security;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Strata.Data.Shopping {
    public class Cart {

        public int Id { get; set; }

        public int UserId { get; set; }

        public ICollection<CartItem> Items { get; set; }

        [NotMapped]
        public int Discount { get; set; }

        [NotMapped]
        public UserLevel DiscountLevel { get; set; }
    }
}
