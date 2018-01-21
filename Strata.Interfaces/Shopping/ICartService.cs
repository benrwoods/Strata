using Strata.Data.Shopping;
using System.Threading.Tasks;

namespace Strata.Interfaces.Shopping {
    public interface ICartService {

        Task<Cart> GetOrCreateCart();

        Task<bool> BuyCart();
    }
}
