using AdocicaMel.Catalog.Domain.Validators;
using AdocicaMel.Core.Domain.Commands;
using Flunt.Notifications;
using Flunt.Validations;
using System.Collections.Generic;

namespace AdocicaMel.Catalog.Domain.Commands
{
    public class ImportProductCommand : Notifiable, IValidableCommand
    {
        public string Vendor { get; set; }
        public string ProductIdentifier { get; set; }
        public decimal Price { get; set; }
        public IEnumerable<string> Tags { get; set; }

        public void Validate()
        {
            AddNotifications(new ProductContract(Vendor, ProductIdentifier, Price, Tags));
        }
    }
}
