using AdocicaMel.Catalog.Api.ViewModels;
using AdocicaMel.Catalog.Domain.Dto;
using AdocicaMel.Catalog.Domain.Enums;
using AdocicaMel.Catalog.Domain.Repositories;
using AdocicaMel.Core.Domain.Pagination;
using AdocicaMel.Core.Infra.DI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AdocicaMel.Catalog.Api
{
    public static class GetCatalogProductsFunction
    {
        [FunctionName("GetCatalogProductsFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]HttpRequest req,
            [Inject]IProductRepository productRepository,
            ILogger log)
        {
            var name = req.Query["name"];
            var tags = req.Query["tags"];
            var page = req.Query["page"];
            var pageSize = req.Query["pageSize"];
            var sort = req.Query["sort"];

            var products = await productRepository.GetProducts(new CatalogProductParamsDto
            {
                Name = name,
                Tags = tags,
                Page = string.IsNullOrEmpty(page) ? PaginationDefaults.DEFAULT_PAGE : Convert.ToInt32(page),
                PageSize = string.IsNullOrEmpty(pageSize) ? PaginationDefaults.DEFAULT_PAGE_SIZE : Convert.ToInt32(pageSize),
                Sort = string.IsNullOrEmpty(sort) ? null : new SortingDefinition<ECatalogProductsSortingFields>(sort)
            });

            var response = products.Items.AsQueryable().Select(x => new CatalogProductViewModel
            {
                Id = x.Id,
                Price = x.Price,
                ProductDescription = x.ProductVendorData.ProductDescription,
                ProductImage = x.ProductVendorData.ProductImage,
                ProductName = x.ProductVendorData.ProductName,
                Tags = x.Tags
            }).ToList();

            return new OkObjectResult(new PagedResult<CatalogProductViewModel>(response, products.TotalCount));
        }
    }
}
