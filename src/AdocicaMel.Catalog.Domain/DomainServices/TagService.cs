using AdocicaMel.Catalog.Domain.Entities;
using AdocicaMel.Catalog.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdocicaMel.Catalog.Domain.DomainServices
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;

        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task CreateTagIfNotExists(IEnumerable<string> tags)
        {
            var existingTags = await _tagRepository.GetTagsByName(tags);

            var tagsToBeInserted = tags.Where(tagName => !existingTags.Any(x => x.Name == tagName))
                .Select(tagName => new Tag(tagName));

            await _tagRepository.CreateManyTags(tagsToBeInserted);
        }
    }
}
