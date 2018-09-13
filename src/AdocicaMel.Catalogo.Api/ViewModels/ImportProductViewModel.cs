using System;
using System.Collections.Generic;
using System.Text;

namespace AdocicaMel.Catalogo.Api.ViewModels
{
    public class ImportProductViewModel
    {
        public string Vendor { get; set; }
        public string ProductIdentifier { get; set; }
        public decimal Price { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}
