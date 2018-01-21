using Microsoft.EntityFrameworkCore;
using Strata.Data.Security;
using Strata.Data.Shopping;

namespace Strata.Data {
    public class ShoppingCartContext : DbContext {

        public ShoppingCartContext(DbContextOptions<ShoppingCartContext> options) : base(options) {
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
