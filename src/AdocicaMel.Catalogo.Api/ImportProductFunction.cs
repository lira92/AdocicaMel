using AdocicaMel.Catalog.Domain.CommandHandlers;
using AdocicaMel.Catalog.Domain.Commands;
using AdocicaMel.Catalog.Infra.Context;
using AdocicaMel.Catalog.Infra.Http;
using AdocicaMel.Catalog.Infra.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;

namespace AdocicaMel.Catalog.Api
{
    public static class ImportProductFunction
    {
        [FunctionName("ImportarProdutos")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequest req,
            ILogger log,
            ExecutionContext context
        )
        {
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            var command = JsonConvert.DeserializeObject<ImportProductCommand>(requestBody);
            var authorization = req.Headers["Authorization"];

            if (command == null)
            {
                return new BadRequestObjectResult("Dados inválidos");
            }

            var catalogContext = new CatalogContext();
            var productVendorRepository = new ProductVendorRepository(new VendorsApi(authorization));
            var productRepository = new ProductRepository(catalogContext);
            var handler = new ProductCommandHandler(productVendorRepository, productRepository);

            try
            {
                handler.Handle(command);
                return new OkResult();
            }
            catch (Exception error)
            {
                log.LogError("Erro ao salvar produto", error);
                return new BadRequestObjectResult("Ocorreu um erro ao importar o produto");
            }


            /*var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();*/
        }
    }
}
