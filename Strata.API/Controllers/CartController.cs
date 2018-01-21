using Microsoft.AspNetCore.Mvc;
using Strata.API.Converter;
using Strata.API.Model.Shopping;
using Strata.Interfaces.Shopping;
using System.Threading.Tasks;

namespace Strata.API.Controllers {

    [Route("api/[controller]")]
    public class CartController : Controller {

        private ICartService CartService { get; }

        public CartController(ICartService cartService) {
            this.CartService = cartService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CartDTO), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetCart() {

            var cart = await this.CartService.GetOrCreateCart();

            return Ok(cart.ToDTO());
        }

        [HttpGet("Buy")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> BuyCart() {
            var success = await this.CartService.BuyCart();

            if (success) {
                return Ok();
            }

            return BadRequest("Balance is too small");
        }
    }
}
