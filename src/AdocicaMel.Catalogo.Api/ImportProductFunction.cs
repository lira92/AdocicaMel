using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using AdocicaMel.Catalogo.Api.Models;
using System;
using AdocicaMel.Catalogo.Api.ViewModels;
using RestSharp;
using AdocicaMel.Catalogo.Api.Dtos;

namespace AdocicaMel.Catalogo.Api
{
    public static class ImportProductFunction
    {
        [FunctionName("ImportarProdutos")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequest req, TraceWriter log, ExecutionContext context)
        {
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<ImportProductViewModel>(requestBody);

            if(data == null)
            {
                return new BadRequestObjectResult("Dados inválidos");
            }

            log.Info("Carregando configurações");
            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            log.Info("Carregando carregadas");


            // get Product Data from vendors api
            var client = new RestClient();
            client.BaseUrl = new Uri(config["VendorsApi"]);

            var request = new RestRequest();
            request.AddHeader("Ocp-Apim-Subscription-Key", config["VendorsSubscriptionKey"]);
            request.Resource = $"vendors/{data.Vendor}/products/{data.ProductIdentifier}";

            var response = client.Execute<ProductVendorResponseDto>(request);

            var productVendorData = response.Data;

            //connect to MongoDb
            var connectionString = config["ProductsCatalogDb"];
            log.Info("Conectando no mongodb");
            var mongoClient = new MongoClient(connectionString);

            log.Info("pegando base catalog");
            var db = mongoClient.GetDatabase("catalog");

            var collection = db.GetCollection<Product>("products");

            var exampleProduct = new Product
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.Now,
                Price = data.Price,
                ProductVendorIdentifier = data.ProductIdentifier,
                Vendor = data.Vendor,
                Tags = data.Tags,
                ProductVendorData = new ProductVendorData
                {
                    ProductDescription = productVendorData.ProductDescription,
                    ProductImage = productVendorData.ProductImage,
                    ProductName = productVendorData.ProductName
                }
            };

            try
            {
                collection.InsertOne(exampleProduct);
                log.Info("Produto salvo");
                return new OkResult();
            }
            catch (Exception error)
            {
                log.Error("Erro ao salvar produto", error);
                return new BadRequestObjectResult("Please pass a name on the query string or in the request body");
            }
        }
    }
}
