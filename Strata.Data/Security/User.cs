using Strata.Data.Shopping;
using System.Collections.Generic;

namespace Strata.Data.Security {
    public class User {

        public int Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public double Balance { get; set; }

        public Cart ShoppingCart { get; set; }

        public ICollection<Order> Orders { get; set; }

    }
}
