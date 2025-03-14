using Asp.Versioning;
using Catalog.Application.Commands;
using Catalog.Application.Dtos;
using Catalog.Application.Queries;
using Catalog.Core.Common;
using Catalog.Core.Specifications;
using DnsClient.Internal;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Catalog.API.Controllers
{
    /// <summary>
    /// Controller for managing catalog operations.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator for handling requests.</param>
        public CatalogController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Retrieves a product by its ID.
        /// </summary>
        /// <param name="id">The product ID.</param>
        /// <returns>The product details.</returns>
        [HttpGet("{id}", Name = "GetProductById")]
        [ProducesResponseType(typeof(ProductDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetProductById(string id)
        {
            var query = new GetProductByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves products by name.
        /// </summary>
        /// <param name="productName">The product name to search.</param>
        /// <returns>A list of matching products.</returns>
        [HttpGet("search/{productName}")]
        [ProducesResponseType(typeof(IList<ProductDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetProductByProductName(string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
            {
                return BadRequest("Product name cannot be empty.");
            }

            var query = new GetProductsByNameQuery(productName);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves all products with optional filters.
        /// </summary>
        /// <param name="catalogFilterParams">Filter parameters.</param>
        /// <returns>A paginated list of products.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<ProductDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllProducts([FromQuery] CatalogFilterParams catalogFilterParams)
        {
            var query = new GetAllProductsQuery(catalogFilterParams);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves all available brands.
        /// </summary>
        /// <returns>A list of brands.</returns>
        [HttpGet("brands")]
        [ProducesResponseType(typeof(IList<BrandDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllBrands()
        {
            var query = new GetAllBrandsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves all available product types.
        /// </summary>
        /// <returns>A list of types.</returns>
        [HttpGet("types")]
        [ProducesResponseType(typeof(IList<TypeDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllTypes()
        {
            var query = new GetAllTypesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves products by brand name.
        /// </summary>
        /// <param name="brand">The brand name.</param>
        /// <returns>A list of products under the specified brand.</returns>
        [HttpGet("brand/{brand}")]
        [ProducesResponseType(typeof(IList<ProductDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetProductsByBrandName(string brand)
        {
            if (string.IsNullOrWhiteSpace(brand))
            {
                return BadRequest("Brand name cannot be empty.");
            }

            var query = new GetProductsByBrandQuery(brand);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="productCommand">The product details.</param>
        /// <returns>The created product.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ProductDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto product)
        {
            var result = await _mediator.Send(new CreateProductCommand(product));
            return CreatedAtRoute("GetProductById", new { id = result.Id }, result);
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="request">The product details to update.</param>
        /// <returns>Result of the update operation.</returns>
        [HttpPut]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductDto request)
        {
            var result = await _mediator.Send(new UpdateProductCommand(request));

            if (!result)
            {
                return BadRequest(new { Message = $"Failed to update product with ID {request.Id}." });
            }

            return Ok(new { Message = $"Product with ID {request.Id} updated successfully." });
        }
        /// <summary>
        /// Deletes a product by ID.
        /// </summary>
        /// <param name="id">The product ID to delete.</param>
        /// <returns>Result of the delete operation.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Product ID cannot be empty.");
            }

            var command = new DeleteProductCommand(id);
            var result = await _mediator.Send(command);
            return Ok(new { Message = $"Product with ID {id} deleted successfully." });
        }
    }
}