using AdocicaMel.Catalog.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdocicaMel.Catalog.Domain.Repositories
{
    public interface IProductVendorRepository
    {
        ProductVendorData GetProductDataFromVendor(string vendor,
            string productIdentifier, string authorization);
    }
}
