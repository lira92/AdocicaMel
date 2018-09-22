using System.Collections.Generic;

namespace AdocicaMel.Catalog.Domain.DomainServices
{
    public interface ITagService
    {
        void CreateTagIfNotExists(IEnumerable<string> tags);
    }
}
