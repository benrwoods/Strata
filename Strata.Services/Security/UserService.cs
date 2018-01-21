using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Strata.Data;
using Strata.Data.Security;
using Strata.Interfaces.Security;
using Strata.Utils;
using System;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace Strata.Services.Security {
    public class UserService : IUserService {

        private ShoppingCartContext DbContext { get; }
        private IHttpContextAccessor HttpContext { get; }


        public UserService(ShoppingCartContext dbContext, IHttpContextAccessor context) {
            this.DbContext = dbContext;
            this.HttpContext = context;
        }

        public IQueryable<User> CurrentUserQuery => this.DbContext.Users.Where(x => x.Login == CurrentUserLogin);


        public async Task<User> GetUser(string login, string password) {
            var encryptedPassword = EncryptionUtil.Encrypt(password, "passphrase");

            return await this.DbContext.Users
                .Where(x => x.Login == login && x.Password == encryptedPassword)
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetUserDiscount() {
            var level = await this.GetUserLevel();
            switch (level) {
                case UserLevel.Gold:
                    return 5;
                case UserLevel.Silver:
                    return 2;
                default:
                    return 0;
            }
        }

        public async Task<double> GetAvaliableBalance() {
            var user = await this.CurrentUserQuery.FirstOrDefaultAsync();
            var level = await this.GetUserLevel();
            switch (level) {
                case UserLevel.Gold:
                    return user.Balance + 1000;
                default:
                    return user.Balance;
            }
        }

        private string CurrentUserLogin {
            get {
                var user = this.HttpContext.HttpContext.User;
                if (user?.Identity == null || !user.Identity.IsAuthenticated) {
                    throw new AuthenticationException();
                }

                return user.Identity.Name;
            }
        }

        public async Task<UserLevel> GetUserLevel() {
            var lastYear = DateTime.Now.AddYears(-1);
            var spentLastYear = await this.DbContext.Orders
                .Join(this.CurrentUserQuery, ok => ok.UserId, ik => ik.Id, (o, i) => o)
                .Where(x => x.OrderDate > lastYear)
                .SumAsync(x => x.Cost);

            if (spentLastYear > 1000) {
                return UserLevel.Gold;
            }
            else if (spentLastYear > 500) {
                return UserLevel.Silver;
            }

            return UserLevel.Standard;
        }

    }
}
