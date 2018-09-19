using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using RestSharp;
using AdocicaMel.Catalog.Domain.Commands;
using AdocicaMel.Catalog.Domain.CommandHandlers;
using AdocicaMel.Catalog.Infra.Context;
using AdocicaMel.Catalog.Infra.Repositories;
using AdocicaMel.Catalog.Infra.Http;

namespace AdocicaMel.Catalog.Api
{
    public static class ImportProductFunction
    {
        [FunctionName("ImportarProdutos")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequest req,
            TraceWriter log,
            ExecutionContext context
        )
        {
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            var command = JsonConvert.DeserializeObject<ImportProductCommand>(requestBody);

            if(command == null)
            {
                return new BadRequestObjectResult("Dados inválidos");
            }

            var catalogContext = new CatalogContext();
            var productVendorRepository = new ProductVendorRepository(new VendorsApi());
            var productRepository = new ProductRepository(catalogContext);
            var handler = new ProductCommandHandler(productVendorRepository, productRepository);

            try
            {
                handler.Handle(command);
                return new OkResult();
            }
            catch (Exception error)
            {
                log.Error("Erro ao salvar produto", error);
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
