using Strata.Data.Security;
using System.Linq;
using System.Threading.Tasks;

namespace Strata.Interfaces.Security {
    public interface IUserService {

        Task<User> GetUser(string login, string password);

        IQueryable<User> CurrentUserQuery { get; }

        Task<int> GetUserDiscount();

        Task<UserLevel> GetUserLevel();

        Task<double> GetAvaliableBalance();
    }
}
