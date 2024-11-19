using Catalog.Core.Common;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specifications;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;

        public ProductRepository(ICatalogContext context)
        {
            _context = context;
        }

        public async Task<Product> GetAsync(string id, CancellationToken cancellationToken)
        {
            return await _context
                .Products
                .Find(p => p.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }
        
        public async Task<PagedResult<Product>> GetAllAsync(CatalogFilterParams filters, CancellationToken cancellationToken)
        {
            var filter = BuildFilter(filters);

            var totalItems = await _context.Products.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
            var data = await GetFilteredDataAsync(filters, filter, cancellationToken);

            return new PagedResult<Product>(
                filters.PageIndex,
                filters.PageSize,
                (int)totalItems,
                data
            );
        }

        private FilterDefinition<Product> BuildFilter(CatalogFilterParams filters)
        {
            var builder = Builders<Product>.Filter;
            var filter = builder.Empty;

            if (!string.IsNullOrEmpty(filters.Search))
            {
                var searchFilter = builder.Where(p => p.Name.ToLower().Contains(filters.Search.ToLower()));
                filter &= searchFilter;
            }

            if (!string.IsNullOrEmpty(filters.BrandId))
            {
                var brandFilter = builder.Eq(p => p.Brands.Id, filters.BrandId);
                filter &= brandFilter;
            }

            if (!string.IsNullOrEmpty(filters.TypeId))
            {
                var typeFilter = builder.Eq(p => p.Types.Id, filters.TypeId);
                filter &= typeFilter;
            }

            return filter;
        }

        private async Task<IReadOnlyList<Product>> GetFilteredDataAsync(
            CatalogFilterParams filters,
            FilterDefinition<Product> filter,
            CancellationToken cancellationToken)
        {
            var sortDefinition = GetSortDefinition(filters.Sort);

            return await _context.Products
                .Find(filter)
                .Sort(sortDefinition)
                .Skip(filters.PageSize * (filters.PageIndex - 1))
                .Limit(filters.PageSize)
                .ToListAsync(cancellationToken);
        }

        private SortDefinition<Product> GetSortDefinition(string? sortOption)
        {
            return sortOption?.ToLower() switch
            {
                "priceasc" => Builders<Product>.Sort.Ascending(p => p.Price),
                "pricedesc" => Builders<Product>.Sort.Descending(p => p.Price),
                _ => Builders<Product>.Sort.Ascending(p => p.Name)
            };
        }

        public async Task<IEnumerable<Product>> GetByBrandAsync(string brandName, CancellationToken cancellationToken)
        {
            return await _context
                .Products
                .Find(p => p.Brands.Name.ToLower() == brandName.ToLower())
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetAllAsync(string name, CancellationToken cancellationToken)
        {
            return await _context
                .Products
                .Find(p => p.Name.ToLower() == name.ToLower())
                .ToListAsync(cancellationToken);
        }

        public async Task<Product> CreateAsync(Product product, CancellationToken cancellationToken)
        {
            await _context.Products.InsertOneAsync(product, cancellationToken: cancellationToken);
            return product;
        }

        public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken)
        {
            var result = await _context
                 .Products
                .DeleteOneAsync(p => p.Id == id, cancellationToken);

            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<bool> UpdateAsync(Product product, CancellationToken cancellationToken)
        {
            var result = await _context
                .Products
                .ReplaceOneAsync(p => p.Id == product.Id, product, cancellationToken: cancellationToken);

            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
    }
}
