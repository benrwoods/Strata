using System.Threading.Tasks;

namespace Strata.Interfaces.Messaging {
    public interface IMessagingService {

        Task SendOrderConfirmation(int orderId);
    }
}
