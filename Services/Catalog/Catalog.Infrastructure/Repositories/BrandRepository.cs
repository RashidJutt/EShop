using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly ICatalogContext _context;

        public BrandRepository(ICatalogContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductBrand>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await GetAllEntitiesAsync(_context.Brands, cancellationToken);
        }

        private async Task<IEnumerable<T>> GetAllEntitiesAsync<T>(IMongoCollection<T> collection, CancellationToken cancellationToken)
        {
            return await collection
                .Find(_ => true)
                .ToListAsync(cancellationToken);
        }
    }
}
