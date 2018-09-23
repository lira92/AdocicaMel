using AdocicaMel.Catalog.Domain.CommandHandlers;
using AdocicaMel.Catalog.Domain.Commands;
using AdocicaMel.Catalog.Domain.Repositories;
using AdocicaMel.Catalog.Infra.Context;
using AdocicaMel.Catalog.Infra.Http;
using AdocicaMel.Catalog.Infra.Repositories;
using AdocicaMel.Core.Infra.DI;
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
using System.Threading.Tasks;

namespace AdocicaMel.Catalog.Api
{
    public static class ImportProductFunction
    {
        [FunctionName("ImportarProdutos")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequest req,
            [Inject]ProductCommandHandler handler,
            ILogger log
        )
        {
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            var command = JsonConvert.DeserializeObject<ImportProductCommand>(requestBody);
            var authorization = req.Headers["Authorization"];

            if (command == null)
            {
                return new BadRequestObjectResult("Dados inválidos");
            }

            try
            {
                handler.Authorization = authorization;
                await handler.Handle(command);
                return DefaultResponse(null, handler.Notifications);
            }
            catch (Exception error)
            {
                log.LogError("Erro ao salvar produto", error);
                return DefaultResponse(null, CreateNotificationsByMessage("error", "Ocorreu um erro ao importar o produto"));
            }
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
