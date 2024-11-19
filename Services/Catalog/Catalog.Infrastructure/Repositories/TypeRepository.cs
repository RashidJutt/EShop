using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class TypeRepository : ITypeRepository
    {
        private readonly ICatalogContext _context;

        public TypeRepository(ICatalogContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductType>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await GetAllEntitiesAsync(_context.Types, cancellationToken);
        }

        private async Task<IEnumerable<T>> GetAllEntitiesAsync<T>(IMongoCollection<T> collection, CancellationToken cancellationToken)
        {
            return await collection
                .Find(_ => true)
                .ToListAsync(cancellationToken);
        }
    }
}
