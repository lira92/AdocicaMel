using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.IO;

namespace AdocicaMel.Catalog.Infra.Http
{
    public class VendorsApi
    {
        private readonly string _baseUrl;

        public VendorsApi()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            _baseUrl = config["VendorsApi"];
        }

        public IRestResponse<T> Execute<T>(RestRequest request, string authorization) where T : new()
        {
            var client = new RestClient();
            client.BaseUrl = new Uri(_baseUrl);
            request.AddHeader("Authorization", authorization);

            var response = client.Execute<T>(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var vendorsApiException = new ApplicationException(message, response.ErrorException);
                throw vendorsApiException;
            }
            return response;
        }
    }
}
