using AdocicaMel.Catalog.Domain.Dto;
using AdocicaMel.Catalog.Domain.Entities;
using AdocicaMel.Core.Domain.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdocicaMel.Catalog.Domain.Repositories
{
    public interface IProductRepository
    {
        Task Create(Product product);
        Task<PagedResult<Product>> GetProducts(CatalogProductParamsDto param);
        Task<Product> GetProductByVendorAndProductIdentifier(string vendor, string productIdentifier);
    }
}
