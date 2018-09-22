using AdocicaMel.Catalog.Api.ViewModels;
using AdocicaMel.Catalog.Domain.Repositories;
using AdocicaMel.Core.Infra.DI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Linq;

namespace AdocicaMel.Catalog.Api
{
    public static class GetCatalogProductsFunction
    {
        [FunctionName("GetCatalogProductsFunction")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]HttpRequest req,
            [Inject]IProductRepository productRepository,
            ILogger log)
        {
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
