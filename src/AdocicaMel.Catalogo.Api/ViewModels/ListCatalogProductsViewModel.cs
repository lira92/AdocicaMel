using System;
using System.Collections.Generic;
using System.Text;

namespace AdocicaMel.Catalogo.Api.ViewModels
{
    public class ListCatalogProductsViewModel
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductImage { get; set; }
        public decimal Price { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}
