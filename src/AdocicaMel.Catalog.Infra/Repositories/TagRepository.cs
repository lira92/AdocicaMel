using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdocicaMel.Catalog.Domain.Entities;
using AdocicaMel.Catalog.Domain.Repositories;
using AdocicaMel.Catalog.Infra.Context;
using MongoDB.Driver;
using Tag = AdocicaMel.Catalog.Domain.Entities.Tag;

namespace AdocicaMel.Catalog.Infra.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly CatalogContext _context;

        public TagRepository(CatalogContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tag>> GetTagsByName(IEnumerable<string> tagNames)
        {
            var filter = new FilterDefinitionBuilder<Tag>().Where(x => tagNames.Contains(x.Name));

            return await _context.Tags.FindSync(filter).ToListAsync();
        }

        public async Task CreateManyTags(IEnumerable<Tag> tags)
        {
            await _context.Tags.InsertManyAsync(tags);
        }

        public async Task<IEnumerable<Tag>> SearchTagsByName(string name)
        {
            var filter = new FilterDefinitionBuilder<Tag>().Empty;

            if(!string.IsNullOrEmpty(name))
            {
                filter = new FilterDefinitionBuilder<Tag>().Where(x => x.Name.ToLower().Contains(name.ToLower()));
            }

            return await _context.Tags.FindSync(filter).ToListAsync();
        }
    }
}
