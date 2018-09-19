using AdocicaMel.Core.Domain.Commands;
using System.Collections.Generic;

namespace AdocicaMel.Catalog.Domain.Commands
{
    public class ImportProductCommand : ICommand
    {
        public string Vendor { get; set; }
        public string ProductIdentifier { get; set; }
        public decimal Price { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}
