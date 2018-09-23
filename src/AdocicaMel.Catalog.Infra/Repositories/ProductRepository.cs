using AdocicaMel.Catalog.Domain.Dto;
using AdocicaMel.Catalog.Domain.Entities;
using AdocicaMel.Catalog.Domain.Enums;
using AdocicaMel.Catalog.Domain.Repositories;
using AdocicaMel.Catalog.Infra.Context;
using AdocicaMel.Core.Domain.Pagination;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdocicaMel.Catalog.Infra.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly CatalogContext _context;

        public ProductRepository(CatalogContext context)
        {
            _context = context;
        }
        public async Task Create(Product product)
        {
            await _context.Products.InsertOneAsync(product);
        }

        public async Task<Product> GetProductByVendorAndProductIdentifier(string vendor, string productIdentifier)
        {
            var filter = new FilterDefinitionBuilder<Product>()
                .Where(x => x.Vendor == vendor && x.ProductVendorIdentifier == productIdentifier);

            return await _context.Products.FindSync(filter).FirstOrDefaultAsync();
        }

        public async Task<PagedResult<Product>> GetProducts(CatalogProductParamsDto param)
        {
            var filter = new FilterDefinitionBuilder<Product>().Empty;

            if (!string.IsNullOrEmpty(param.Name))
            {
                var filterByName = new FilterDefinitionBuilder<Product>()
                    .Where(x => x.ProductVendorData.ProductName.ToLower().Contains(param.Name.ToLower()));
                filter = filter & filterByName;
            }
            if(param.Tags?.Length > 0)
            {
                var filterByTags = new FilterDefinitionBuilder<Product>()
                    .Where(x => param.Tags.Any(tag => x.Tags.Contains(tag)));
                filter = filter & filterByTags;
            }

            var query = _context
                .Products
                .Find(filter);

            if(param.Sort != null)
            {
                if(param.Sort.Field == ECatalogProductsSortingFields.Price)
                {
                    query = param.Sort.Order == ESortingOrder.ASC 
                        ? query.SortBy(x => x.Price)
                        : query.SortByDescending(x => x.Price);
                }
            }

            var total = await query.CountDocumentsAsync();
            var items = await query.Paginate(param.Page, param.PageSize).ToListAsync();

            return new PagedResult<Product>(items, total);
        }
    }
}
