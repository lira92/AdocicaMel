using AdocicaMel.Catalog.Domain.Entities;
using AdocicaMel.Catalog.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdocicaMel.Catalog.Domain.DomainServices
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;

        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public void CreateTagIfNotExists(IEnumerable<string> tags)
        {
            var existingTags = _tagRepository.GetTagsByName(tags);

            var tagsToBeInserted = tags.Where(tagName => !existingTags.Any(x => x.Name == tagName))
                .Select(tagName => new Tag(tagName));

            _tagRepository.CreateManyTags(tagsToBeInserted);
        }
    }
}
