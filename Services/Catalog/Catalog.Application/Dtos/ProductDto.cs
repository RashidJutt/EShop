using Catalog.Core.Entities;

namespace Catalog.Application.Dtos;

public class ProductDto : EntityDto
{
    public string Name { get; set; } = default!;
    public string Summary { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string ImageFile { get; set; } = default!;
    public ProductBrand Brands { get; set; } = default!;
    public ProductType Types { get; set; } = default!;
    public decimal Price { get; set; } = default!;
}
