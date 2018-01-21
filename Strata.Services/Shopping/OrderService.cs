using Strata.Data;
using Strata.Data.Shopping;
using Strata.Interfaces.Security;
using Strata.Interfaces.Shopping;
using System.Linq;

namespace Strata.Services.Shopping {
    public class OrderService : IOrderService {

        private ShoppingCartContext DbContext { get; }
        private IUserService UserService { get; }

        public OrderService(ShoppingCartContext dbContext, IUserService userService) {
            this.DbContext = dbContext;
            this.UserService = userService;
        }

        public IQueryable<Order> GetOrders() {
            return this.DbContext.Orders
                .Join(this.UserService.CurrentUserQuery, ok => ok.UserId, ik => ik.Id, (o, i) => o);
        }
    }
}
