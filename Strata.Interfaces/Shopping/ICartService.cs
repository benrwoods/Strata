using Strata.Data.Shopping;
using System.Threading.Tasks;

namespace Strata.Interfaces.Shopping {
    public interface ICartService {

        Task<Cart> GetOrCreateCart();

        Task<bool> BuyCart();

        Task<bool> TryAddItem(int itemId);

        Task<bool> TryUpdateQuantity(int itemId, int newQuatity);

        Task<bool> TryRemoveItem(int itemId);
    }
}
