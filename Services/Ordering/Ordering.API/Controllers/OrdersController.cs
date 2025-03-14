using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Commands;
using Ordering.Application.Dtos;
using Ordering.Application.Queries;
using System.Net;

namespace Ordering.API.Controllers
{
    /// <summary>
    /// Controller for managing catalog operations.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrdersController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator for handling requests.</param>
        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Search orders by username.
        /// </summary>
        /// <param name="username">The username to search.</param>
        /// <returns>A list of matching orders.</returns>
        [HttpGet("search/{username}")]
        [ProducesResponseType(typeof(IList<OrderDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetOrdersByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest("username cannot be empty.");
            }

            var query = new GetOrdersQuery(username);
            var result = await _mediator.Send(query);
            return Ok(result);
        }


        /// <summary>
        /// Creates a new order.
        /// </summary>
        /// <param name="Order">The order to update.</param>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto order)
        {
            await _mediator.Send(new CreateOrderCommand(order));
            return Ok();
        }

        /// <summary>
        /// Updates an existing order.
        /// </summary>
        /// <param name="order">The order details to update.</param>
        /// <returns>Result of the update operation.</returns>
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateOrder([FromBody] OrderDto request)
        {
            await _mediator.Send(new UpdateOrderCommand(request));
            return Ok(new { Message = $"Order with ID {request.Id} updated successfully." });
        }

        /// <summary>
        /// Deletes a order by id.
        /// </summary>
        /// <param name="id">The order id to delete.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Order id cannot be 0 or negative.");
            }

            var command = new DeleteOrderCommand(id);
            await _mediator.Send(command);
            return Ok(new { Message = $"Order with id {id} deleted successfully." });
        }
    }
}