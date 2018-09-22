using AdocicaMel.Catalog.Domain.Entities;
using System.Collections.Generic;

namespace AdocicaMel.Catalog.Domain.Repositories
{
    public interface ITagRepository
    {
        IEnumerable<Tag> GetTagsByName(IEnumerable<string> tagNames);
        void CreateManyTags(IEnumerable<Tag> tags);
        IEnumerable<Tag> SearchTagsByName(string name);
    }
}
