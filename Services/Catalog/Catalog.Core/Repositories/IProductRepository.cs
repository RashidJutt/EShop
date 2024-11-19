using Catalog.Core.Common;
using Catalog.Core.Entities;
using Catalog.Core.Specifications;

namespace Catalog.Core.Repositories;

public interface IProductRepository
{
    Task<PagedResult<Product>> GetAllAsync(CatalogFilterParams filters, CancellationToken cancellationToken);
    Task<Product> GetAsync(string id, CancellationToken cancellationToken);
    Task<IEnumerable<Product>> GetAllAsync(string name, CancellationToken cancellationToken);
    Task<IEnumerable<Product>> GetByBrandAsync(string brandName, CancellationToken cancellationToken);
    Task<Product> CreateAsync(Product product, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(Product product, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(string id, CancellationToken cancellationToken);
}
