using AdocicaMel.Catalog.Domain.Entities;
using MongoDB.Driver;

namespace AdocicaMel.Catalog.Infra.Context
{
    public class CatalogContext
    {
        private readonly IMongoDatabase _database;

        public CatalogContext()
        {
            var clienteMongo = new MongoClient("mongodb://adocicamel-products:Dj7EEFWo0ZUc3UDSctabbKTTZnSsUWhsrmYsWGcGVcDiWhHaBIILA9s4eVOsKq5aIgncraFwovZt0Hh1wvabtA==@adocicamel-products.documents.azure.com:10255/?ssl=true&replicaSet=globaldb");

            if (clienteMongo != null)
            {
                _database = _database ?? clienteMongo.GetDatabase("catalog");
            }
        }

        public IMongoCollection<Product> Products
            => _database.GetCollection<Product>("products");
    }
}
