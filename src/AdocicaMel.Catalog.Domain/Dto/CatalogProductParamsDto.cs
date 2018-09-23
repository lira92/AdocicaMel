using AdocicaMel.Core.Domain.Pagination;

namespace AdocicaMel.Catalog.Domain.Dto
{
    public class CatalogProductParamsDto
    {
        public string Name { get; set; }
        public string[] Tags { get; set; }
        public int Page { get; set; } = PaginationDefaults.DEFAULT_PAGE;
        public int PageSize { get; set; } = PaginationDefaults.DEFAULT_PAGE_SIZE;
    }
}
