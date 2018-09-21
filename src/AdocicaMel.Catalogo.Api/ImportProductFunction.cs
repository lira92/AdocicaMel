using AdocicaMel.Catalog.Domain.CommandHandlers;
using AdocicaMel.Catalog.Domain.Commands;
using AdocicaMel.Catalog.Infra.Context;
using AdocicaMel.Catalog.Infra.Http;
using AdocicaMel.Catalog.Infra.Repositories;
using Flunt.Notifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
                return DefaultResponse(null, handler.Notifications);
            }
            catch (Exception error)
            {
                log.LogError("Erro ao salvar produto", error);
                return DefaultResponse(null, CreateNotificationsByMessage("error", "Ocorreu um erro ao importar o produto"));
            }

            /*var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();*/
        }

        public static IActionResult DefaultResponse(object result, IEnumerable<Notification> notifications)
        {
            if (!notifications.Any())
            {
                try
                {
                    if (result == null)
                    {
                        return new OkObjectResult(new
                        {
                            success = true
                        });
                    }
                    return new OkObjectResult(new
                    {
                        success = true,
                        data = result
                    });
                }
                catch
                {
                    return new BadRequestObjectResult(new
                    {
                        success = false,
                        errors = new[] { "Ocorreu uma falha interna no Servidor." }
                    });
                }
            }
            else
            {
                return new BadRequestObjectResult(new
                {
                    success = false,
                    errors = notifications
                });
            }
        }

        public static IEnumerable<Notification> CreateNotificationsByMessage(string type, string message)
        {
            return new List<Notification>
            {
                new Notification(type, message)
            };
        }
    }
}
