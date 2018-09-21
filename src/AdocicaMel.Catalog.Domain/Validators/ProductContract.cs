using Flunt.Validations;
using System.Collections.Generic;
using System.Linq;

namespace AdocicaMel.Catalog.Domain.Validators
{
    internal class ProductContract : Contract
    {
        public ProductContract(string vendor, string productIdentifier, decimal price, IEnumerable<string> tags)
        {
            IsNotNullOrEmpty(vendor, nameof(vendor), "Informe o fornecedor")
            .IsNotNullOrEmpty(productIdentifier, nameof(productIdentifier), "Informe o identificador do produto do fornecedor")
            .IsGreaterThan(price, 0, nameof(price), "Informe um preço maior que zero")
            .IsNotNull(tags, nameof(tags), "Informe pelo menos uma tag para o produto");

            if (tags != null)
            {
                IsGreaterThan(tags.Count(), 0, nameof(tags), "Informe pelo menos uma tag para o produto");
            }
        }
    }
}
