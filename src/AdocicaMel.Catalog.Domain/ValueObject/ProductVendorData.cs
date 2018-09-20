using AdocicaMel.Core.Domain.ValueObjects;

namespace AdocicaMel.Catalog.Domain.ValueObjects
{
    public class ProductVendorData : ValueObject
    {
        public ProductVendorData(string productName, string productDescription,
            string productImage, decimal productCost)
        {
            ProductName = productName;
            ProductDescription = productDescription;
            ProductImage = productImage;
            ProductCost = productCost;
        }

        public string ProductName { get; private set; }
        public string ProductDescription { get; private set; }
        public string ProductImage { get; private set; }
        public decimal ProductCost { get; private set; }
    }
}
