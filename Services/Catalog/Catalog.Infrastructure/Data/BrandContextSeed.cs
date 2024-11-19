using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data;

public static class BrandContextSeed
{
    // For IIS. 
    //private const string BrandsSeedFilePath = "../Catalog.Infrastructure/Data/SeedData/brands.json";
    
    // For Container. 
    private const string BrandsSeedFilePath = "Data/SeedData/brands.json";

    public static async Task SeedBrandsAsync(IMongoCollection<ProductBrand> brandCollection)
    {
        if (await HasExistingBrandsAsync(brandCollection))
        {
            return;
        }

        var brands = await ReadBrandsFromFileAsync(BrandsSeedFilePath);
        if (brands != null && brands.Any())
        {
            await InsertBrandsAsync(brandCollection, brands);
        }
    }

    private static async Task<bool> HasExistingBrandsAsync(IMongoCollection<ProductBrand> brandCollection)
    {
        return await brandCollection.Find(b => true).AnyAsync();
    }

    private static async Task<List<ProductBrand>> ReadBrandsFromFileAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"The brands seed file could not be found at: {filePath}");
        }

        try
        {
            var brandsData = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<List<ProductBrand>>(brandsData) ?? new List<ProductBrand>();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while reading or deserializing the brands seed file.", ex);
        }
    }

    private static async Task InsertBrandsAsync(IMongoCollection<ProductBrand> brandCollection, List<ProductBrand> brands)
    {
        foreach (var brand in brands)
        {
            try
            {
                await brandCollection.InsertOneAsync(brand);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while inserting the brand with ID: {brand.Id}", ex);
            }
        }
    }
}
