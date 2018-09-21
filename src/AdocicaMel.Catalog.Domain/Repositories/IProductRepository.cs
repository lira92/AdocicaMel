using AdocicaMel.Catalog.Domain.Entities;
using System.Collections.Generic;

namespace AdocicaMel.Catalog.Domain.Repositories
{
    public interface IProductRepository
    {
        void Create(Product product);
        IEnumerable<Product> GetProducts();
        Product GetProductByVendorAndProductIdentifier(string vendor, string productIdentifier);
    }
}
