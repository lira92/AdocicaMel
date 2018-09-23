using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdocicaMel.Catalog.Domain.DomainServices
{
    public interface ITagService
    {
        Task CreateTagIfNotExists(IEnumerable<string> tags);
    }
}
