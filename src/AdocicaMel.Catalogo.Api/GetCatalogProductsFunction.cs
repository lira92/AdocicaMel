
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using AdocicaMel.Catalogo.Api.Models;
using MongoDB.Bson;
using AdocicaMel.Catalogo.Api.ViewModels;
using System.Collections.Generic;

namespace AdocicaMel.Catalogo.Api
{
    public static class GetCatalogProductsFunction
    {
        [FunctionName("GetCatalogProductsFunction")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]HttpRequest req, ILogger log, ExecutionContext context)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = config["ProductsCatalogDb"];
            var mongoClient = new MongoClient(connectionString);

            var db = mongoClient.GetDatabase("catalog");

            var collection = db.GetCollection<Product>("products");

            var products = collection.Find(new BsonDocument()).ToList();

            var listResponse = new List<ListCatalogProductsViewModel>();

            foreach(var product in products)
            {
                listResponse.Add(
                    new ListCatalogProductsViewModel
                    {
                        Id = product.Id,
                        Price = product.Price,
                        ProductDescription = product.ProductVendorData.ProductDescription,
                        ProductName = product.ProductVendorData.ProductName,
                        ProductImage = product.ProductVendorData.ProductImage,
                        Tags = product.Tags
                    }
                );
            }

            return new OkObjectResult(listResponse);
        }
    }
}
