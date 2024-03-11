using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data;

public class CatalogContextSeed
{
    public static void SeedData(IMongoCollection<Product> productCollection)
    {
        bool existProduct = productCollection.Find(p => true).Any();
        if (!existProduct)
        {
            productCollection.InsertManyAsync(GetMyProducts());
        }
    }

    private static IEnumerable<Product> GetMyProducts()
    {
        return new List<Product>()
        {
            new Product()
            {
                Id = "8d6a05c378f44f0a9fc42c1b",
                Name = "Nebula Nova Smart Projector",
                Category = "Video",
                Description = "Transform any space into a cinematic experience with the Nebula Nova Smart Projector. " +
                               "This compact and portable projector offers stunning HD resolution and smart connectivity features, " +
                               "allowing you to stream your favorite movies and shows with ease.",
                Image = "projectornebula.png",
                Price = 1500
            },
            new Product()
            {
                Id = "a7e9f4b18e534c42a2b19fee",
                Name = "QuantumX Pro Series Headphones",
                Category = "Audio",
                Description = "Immerse yourself in unparalleled audio quality with the QuantumX Pro Series Headphones. " +
                              "These cutting-edge headphones deliver a rich and crisp sound experience, making every beat " +
                              "and note come to life.",
                Image = "headphonesquantom.png",
                Price = 300
            },
            new Product()
            {
                Id = "c6d3a8e29b7441f1aa125d2f",
                Name = "SolarFlare X5000 Solar Charger",
                Category = "Energy",
                Description = "Stay charged on the go with the SolarFlare X5000 Solar Charger. Harness the power of the " +
                              "sun to keep your devices powered up wherever you are. This compact and efficient solar " +
                              "charger is perfect for outdoor enthusiasts and eco-conscious users.",
                Image = "solarflarecharger.png",
                Price = 900
            },
        };
    }
}