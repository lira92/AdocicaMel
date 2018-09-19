using AdocicaMel.Catalog.Domain.Repositories;
using AdocicaMel.Catalog.Domain.ValueObjects;
using AdocicaMel.Catalog.Infra.Dto;
using AdocicaMel.Catalog.Infra.Http;
using RestSharp;

namespace AdocicaMel.Catalog.Infra.Repositories
{
    public class ProductVendorRepository : IProductVendorRepository
    {
        private readonly VendorsApi _vendorsApi;

        public ProductVendorRepository(VendorsApi vendorsApi)
        {
            _vendorsApi = vendorsApi;
        }

        public ProductVendorData GetProductDataFromVendor(string vendor, string productIdentifier)
        {
            var request = new RestRequest
            {
                Resource = $"vendors/{vendor}/products/{productIdentifier}"
            };

            var response = _vendorsApi.Execute<ProductVendorResponseDto>(request);

            return new ProductVendorData(
                productName: response.ProductName,
                productDescription: response.ProductDescription,
                productImage: response.ProductImage,
                productCost: response.Price
            );
        }
    }
}
