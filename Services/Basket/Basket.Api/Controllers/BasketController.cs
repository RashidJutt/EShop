using Asp.Versioning;
using Basket.Application.Commands;
using Basket.Application.Dtos;
using Basket.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    /// <summary>
    /// Controller for managing shopping cart operations.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasketController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator for handling requests.</param>
        public BasketController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Retrieves a shopping cart for the specified username.
        /// </summary>
        /// <param name="username">The username of the shopping cart owner.</param>
        /// <returns>The shopping cart details.</returns>
        [HttpGet("{username}")]
        [ProducesResponseType(typeof(ShoppingCartDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetBasketByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest("Username cannot be null or empty.");
            }

            var shoppingCart = await _mediator.Send(new GetShoppingCartQuery(username));
            return Ok(shoppingCart);
        }

        /// <summary>
        /// Updates a shopping cart with new details.
        /// </summary>
        /// <param name="shoppingCart">The shopping cart details to update.</param>
        /// <returns>The updated shopping cart.</returns>
        [HttpPut]
        [ProducesResponseType(typeof(ShoppingCartDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateShoppingCart([FromBody] ShoppingCartDto shoppingCart)
        {
            var updatedShoppingCart = await _mediator.Send(new UpdateShoppingCartCommand(shoppingCart));
            return Ok(updatedShoppingCart);
        }

        /// <summary>
        /// Deletes a shopping cart for the specified username.
        /// </summary>
        /// <param name="username">The username of the shopping cart owner to delete.</param>
        /// <returns>Result of the delete operation.</returns>
        [HttpDelete("{username}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteBasket(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest("Username cannot be null or empty.");
            }

            await _mediator.Send(new DeleteShoppingCartCommand(username));
            return Ok(new { Message = $"Basket of user {username} deleted successfully." });
        }
    }
}
