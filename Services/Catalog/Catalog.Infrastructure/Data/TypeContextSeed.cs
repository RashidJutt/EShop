using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Reflection;
using System.Text.Json;

namespace Catalog.Infrastructure.Data
{
    public static class TypeContextSeed
    {
        // For IIS. 
        //private const string TypesSeedFilePath = "../Catalog.Infrastructure/Data/SeedData/types.json";

        // For container.
        private const string TypesSeedFilePath = "Data/SeedData/types.json";

        public static async Task SeedProductTypesAsync(IMongoCollection<ProductType> typeCollection)
        {
            if (await HasExistingProductTypesAsync(typeCollection))
            {
                return;
            }

            var types = await ReadProductTypesFromFileAsync(TypesSeedFilePath);
            if (types != null && types.Any())
            {
                await InsertProductTypesAsync(typeCollection, types);
            }
        }

        private static async Task<bool> HasExistingProductTypesAsync(IMongoCollection<ProductType> typeCollection)
        {
            return await typeCollection.Find(_ => true).AnyAsync();
        }

        private static async Task<List<ProductType>> ReadProductTypesFromFileAsync(string filePath)
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, filePath);
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"The product types seed file could not be found at: {path}");
            }

            try
            {
                var typesData = await File.ReadAllTextAsync(path);
                return JsonSerializer.Deserialize<List<ProductType>>(typesData) ?? new List<ProductType>();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while reading or deserializing the product types seed file.", ex);
            }
        }

        private static async Task InsertProductTypesAsync(IMongoCollection<ProductType> typeCollection, List<ProductType> productTypes)
        {
            foreach (var productType in productTypes)
            {
                try
                {
                    await typeCollection.InsertOneAsync(productType);
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error occurred while inserting the product type with ID: {productType.Id}", ex);
                }
            }
        }
    }
}
