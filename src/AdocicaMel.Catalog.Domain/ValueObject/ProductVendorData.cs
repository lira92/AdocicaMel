using AdocicaMel.Core.Domain.ValueObjects;
using Flunt.Validations;

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

            AddNotifications(
                new Contract()
                    .Requires()
                    .IsNotNullOrEmpty(ProductName, nameof(ProductName), "O produto deve possuir um nome")
                    .IsGreaterThan(ProductCost, 0, nameof(productCost), "O produto deve ter um custo")
            );
        }

        public string ProductName { get; private set; }
        public string ProductDescription { get; private set; }
        public string ProductImage { get; private set; }
        public decimal ProductCost { get; private set; }
    }
}
