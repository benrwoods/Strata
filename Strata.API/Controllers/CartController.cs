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

        [HttpGet("buy")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> BuyCart() {
            var success = await this.CartService.BuyCart();

            if (success) {
                return Ok();
            }

            return BadRequest("Balance is too small");
        }

        [HttpPost("add")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddItem([FromBody] AddItemDTO requestModel) {

            if (!ModelState.IsValid || !await this.CartService.TryAddItem(requestModel.ItemId)) {
                return BadRequest("Request is not valid");
            }

            return Ok();
        }

        [HttpPut("update")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateQuantity([FromBody] UpdateQuantityDTO requestModel) {

            if (!ModelState.IsValid || !await this.CartService.TryUpdateQuantity(requestModel.ItemId, requestModel.NewQuantity)) {
                return BadRequest("Request is not valid");
            }

            return Ok();
        }

        [HttpDelete("remove")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RemoveItem([FromBody] DeleteItemDTO requestModel) {

            if (!ModelState.IsValid || !await this.CartService.TryRemoveItem(requestModel.ItemId)) {
                return BadRequest("Request is not valid");
            }

            return Ok();
        }
    }
}
