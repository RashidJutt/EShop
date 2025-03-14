using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Reflection;
using System.Text.Json;


namespace Catalog.Infrastructure.Data;

public static class ProductContextSeed
{
    // For IIS. 
    //private const string ProductsSeedFilePath = "../Catalog.Infrastructure/Data/SeedData/products.json";
    // For container.
    private const string ProductsSeedFilePath = "Data/SeedData/products.json";

    public static async Task SeedProductsAsync(IMongoCollection<Product> productCollection)
    {
        if (await HasExistingProductsAsync(productCollection))
        {
            return;
        }

        var products = await ReadProductsFromFileAsync(ProductsSeedFilePath);
        if (products != null && products.Any())
        {
            await InsertProductsAsync(productCollection, products);
        }
    }

    private static async Task<bool> HasExistingProductsAsync(IMongoCollection<Product> productCollection)
    {
        return await productCollection.Find(p => true).AnyAsync();
    }

    private static async Task<List<Product>> ReadProductsFromFileAsync(string filePath)
    {
        var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, filePath);
        if (!File.Exists(path: path))
        {
            throw new FileNotFoundException($"The products seed file could not be found at: {path}");
        }

        try
        {
            var productsData = await File.ReadAllTextAsync(path);
            return JsonSerializer.Deserialize<List<Product>>(productsData) ?? new List<Product>();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while reading or deserializing the products seed file.", ex);
        }
    }

    private static async Task InsertProductsAsync(IMongoCollection<Product> productCollection, List<Product> products)
    {
        foreach (var product in products)
        {
            try
            {
                await productCollection.InsertOneAsync(product);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while inserting the product with ID: {product.Id}", ex);
            }
        }
    }
}
