using System;
using System.Collections.Generic;

namespace AdocicaMel.Catalogo.Api.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public string Vendor { get; set; }
        public string ProductVendorIdentifier { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public ProductVendorData ProductVendorData { get; set; }
    }
}
