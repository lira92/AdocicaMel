using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdocicaMel.Catalog.Infra.Http
{
    public class VendorsApi
    {
        const string BaseUrl = "https://adocicamel.azure-api.net/";
        private readonly string _secretKey = "b19ad0ad208345e986dcbe1abe38fbbb";

        public T Execute<T>(RestRequest request) where T : new()
        {
            var client = new RestClient();
            client.BaseUrl = new Uri(BaseUrl);
            request.AddHeader("Ocp-Apim-Subscription-Key", _secretKey);

            var response = client.Execute<T>(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var vendorsApiException = new ApplicationException(message, response.ErrorException);
                throw vendorsApiException;
            }
            return response.Data;
        }
    }
}
