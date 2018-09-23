using AdocicaMel.Catalog.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdocicaMel.Catalog.Domain.Repositories
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetTagsByName(IEnumerable<string> tagNames);
        Task CreateManyTags(IEnumerable<Tag> tags);
        Task<IEnumerable<Tag>> SearchTagsByName(string name);
    }
}
