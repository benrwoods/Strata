using Strata.Data.Shopping;
using System.Linq;

namespace Strata.Interfaces.Shopping {
    public interface IOrderService {

        IQueryable<Order> GetOrders();
    }
}
