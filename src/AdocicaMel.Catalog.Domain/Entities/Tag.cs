using AdocicaMel.Core.Domain.Entities;
using Flunt.Validations;

namespace AdocicaMel.Catalog.Domain.Entities
{
    public class Tag : Entity
    {
        public Tag(string name)
        {
            Name = name;

            AddNotifications(
                new Contract().Requires()
                    .IsNotNullOrEmpty(Name, nameof(Name), "Informe o nome da tag")
            );
        }

        public string Name { get; private set; }
    }
}
