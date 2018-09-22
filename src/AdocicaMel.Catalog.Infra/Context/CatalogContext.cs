using AdocicaMel.Catalog.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.IO;
using Tag = AdocicaMel.Catalog.Domain.Entities.Tag;

namespace AdocicaMel.Catalog.Infra.Context
{
    public class CatalogContext
    {
        private readonly IMongoDatabase _database;

        public CatalogContext()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            
            var clienteMongo = new MongoClient(config["ProductsCatalogDb"]);

            if (clienteMongo != null)
            {
                _database = _database ?? clienteMongo.GetDatabase("catalog");
            }
        }

        public IMongoCollection<Product> Products
            => _database.GetCollection<Product>("products");

        public IMongoCollection<Tag> Tags
            => _database.GetCollection<Tag>("tags");
    }
}
