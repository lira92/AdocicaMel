using AdocicaMel.Catalog.Api.Models;
using AdocicaMel.Catalog.Api.ViewModels;
using AdocicaMel.Catalog.Infra.Context;
using AdocicaMel.Catalog.Infra.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace AdocicaMel.Catalog.Api
{
    public static class GetCatalogProductsFunction
    {
        [FunctionName("GetCatalogProductsFunction")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]HttpRequest req, ILogger log, ExecutionContext context)
        {
            var catalogContext = new CatalogContext();
            var productRepository = new ProductRepository(catalogContext);

            var products = productRepository.GetProducts();

            var response = products.AsQueryable().Select(x => new CatalogProductViewModel
            {
                Id = x.Id,
                Price = x.Price,
                ProductDescription = x.ProductVendorData.ProductDescription,
                ProductImage = x.ProductVendorData.ProductImage,
                ProductName = x.ProductVendorData.ProductName,
                Tags = x.Tags
            }).ToList();

            return new OkObjectResult(response);
        }
    }
}
