using AdocicaMel.Catalog.Domain.Repositories;
using AdocicaMel.Catalog.Domain.ValueObjects;
using AdocicaMel.Catalog.Infra.Dto;
using AdocicaMel.Catalog.Infra.Http;
using RestSharp;
using System.Net;

namespace AdocicaMel.Catalog.Infra.Repositories
{
    public class ProductVendorRepository : IProductVendorRepository
    {
        private readonly VendorsApi _vendorsApi;

        public ProductVendorRepository(VendorsApi vendorsApi)
        {
            _vendorsApi = vendorsApi;
        }

        public ProductVendorData GetProductDataFromVendor(string vendor, 
            string productIdentifier, string authorization)
        {
            var request = new RestRequest
            {
                Resource = $"vendors/{vendor}/products/{productIdentifier}"
            };

            var response = _vendorsApi.Execute<ProductVendorResponseDto>(request, authorization);
            if(response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            return new ProductVendorData(
                productName: response.Data.ProductName,
                productDescription: response.Data.ProductDescription,
                productImage: response.Data.ProductImage,
                productCost: response.Data.Price
            );
        }
    }
}
