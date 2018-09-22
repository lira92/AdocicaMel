
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AdocicaMel.Core.Infra.DI;
using AdocicaMel.Catalog.Domain.Repositories;
using System.Linq;
using AdocicaMel.Catalog.Api.ViewModels;

namespace AdocicaMel.Catalog.Api
{
    public static class GetCatalogTagsFunction
    {
        [FunctionName("GetCatalogTagsFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, 
            [Inject]ITagRepository tagRepository,
            ILogger log)
        {
            string search = req.Query["tagName"];

            var tags = tagRepository.SearchTagsByName(search);

            var response = tags.AsQueryable().Select(x => new CatalogTagViewModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            return new OkObjectResult(response);
        }
    }
}
