using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Strata.API.Model.Security;
using Strata.API.Security;
using Strata.Interfaces.Security;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Strata.API.Controllers {

    [Route("api/[controller]")]
    public class TokenController : Controller {

        private JwtIssuerOptions JwtOptions { get; }
        private IUserService UserService { get; }

        public TokenController(IOptions<JwtIssuerOptions> jwtOptions, IUserService userService) {
            this.JwtOptions = jwtOptions.Value;
            this.UserService = userService;
        }

        /// <summary>
        /// Request an access token.
        /// </summary>
        /// <param name="userLogin"></param>
        /// <remarks>
        /// Once acquired, the access token must be provided with every request in an 'Authorize' request header.
        /// The value of the header being formatted like 'bearer {accessToken}'.
        /// </remarks>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Token), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Authenticate([FromBody] UserLoginDTO userLogin) {
            var identity = await GetClaimsIdentity(userLogin);
            if (identity == null) {
                return Unauthorized();
            }

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, userLogin.Login),
                new Claim(JwtRegisteredClaimNames.Jti, await JwtOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat,
                          ToUnixEpochDate(JwtOptions.IssuedAt).ToString(),
                          ClaimValueTypes.Integer64)
            };

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: JwtOptions.Issuer,
                audience: JwtOptions.Audience,
                claims: claims,
                notBefore: JwtOptions.NotBefore,
                expires: JwtOptions.Expiration,
                signingCredentials: JwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            // Serialize and return the response
            var response = new Token {
                AccessToken = encodedJwt,
                ExpiresIn = JwtOptions.ValidFor.TotalSeconds
            };

            return Ok(response);
        }

        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);


        private async Task<ClaimsIdentity> GetClaimsIdentity(UserLoginDTO userLogin) {
            var dbUser = await this.UserService.GetUser(userLogin.Login, userLogin.Password);

            if (dbUser == null) {
                return null;
            }

            return new ClaimsIdentity(new GenericIdentity(userLogin.Login), null);

        }
    }
}
