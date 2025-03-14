
using Asp.Versioning;
using Basket.Application.Commands;
using Basket.Application.Dtos;
using Basket.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

/// <summary>
/// Controller for managing shopping cart operations.
/// </summary>
[ApiController]
[ApiVersion("2.0")]
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
        _mediator = mediator;
    }

    /// <summary>
    /// Deletes a shopping cart for the specified username.
    /// </summary>
    /// <param name="username">The username of the shopping cart owner to delete.</param>
    /// <returns>Result of the delete operation.</returns>
    [HttpPost("checkout")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Checkout(BasketCheckoutV2Dto basketCheckout)
    {
        var query = new GetShoppingCartQuery(basketCheckout.UserName);
        var shopingCart = await _mediator.Send(query);
        if (shopingCart == null)
        {
            return BadRequest();
        }
        basketCheckout.TotalPrice = shopingCart.TotalPrice;
        await _mediator.Send(new BasketCheckoutV2Command(basketCheckout));
        return Accepted();
    }
}
