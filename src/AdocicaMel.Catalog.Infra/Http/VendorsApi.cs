using RestSharp;
using System;

namespace AdocicaMel.Catalog.Infra.Http
{
    public class VendorsApi
    {
        private readonly string _authorization;
        const string BaseUrl = "https://adocicamel.azure-api.net/";

        public VendorsApi(string authorization)
        {
            _authorization = authorization;
        }

        public IRestResponse<T> Execute<T>(RestRequest request) where T : new()
        {
            var client = new RestClient();
            client.BaseUrl = new Uri(BaseUrl);
            request.AddHeader("Authorization", _authorization);

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
