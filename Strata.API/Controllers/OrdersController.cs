using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Strata.API.Converter;
using Strata.API.Model.Shopping;
using Strata.Interfaces.Shopping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Strata.API.Controllers {

    [Route("api/[controller]")]
    public class OrdersController : Controller {

        private IOrderService OrderService { get; }

        public OrdersController(IOrderService orderService) {
            this.OrderService = orderService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrderDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetOrder(int id) {
            var order = await this.OrderService.GetOrders()
                .Where(x => x.Id == id)
                .Include(x => x.Items)
                .ThenInclude(x => x.Item)
                .FirstOrDefaultAsync();

            if (order == null) {
                return NotFound();
            }

            return Ok(order.ToDTO());
        }

        [HttpGet("pending")]
        [ProducesResponseType(typeof(IEnumerable<OrderDTO>), 200)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> GetPendingOrders() {
            var orders = await this.OrderService.GetOrders()
                .Where(x => x.ShippingDate > DateTime.Now)
                .ToListAsync();

            if (orders == null || !orders.Any()) {
                return NoContent();
            }

            return Ok(orders.Select(x => x.ToDTOList()));
        }

        [HttpGet("range/{from}/{to}")]
        [ProducesResponseType(typeof(IEnumerable<OrderDTO>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetOrdersInRange(DateTime from, DateTime to) {
            if (from > to) {
                return BadRequest("Invalid date range");
            }

            var orders = await this.OrderService.GetOrders()
                .Where(x => x.OrderDate > from)
                .Where(x => x.OrderDate < to)
                .ToListAsync();

            if (orders == null || !orders.Any()) {
                return NoContent();
            }

            return Ok(orders.Select(x => x.ToDTOList()));
        }
    }
}
