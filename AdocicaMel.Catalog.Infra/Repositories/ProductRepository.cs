using AdocicaMel.Catalog.Domain;
using AdocicaMel.Catalog.Domain.Entities;
using AdocicaMel.Catalog.Domain.Repositories;
using AdocicaMel.Catalog.Infra.Context;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdocicaMel.Catalog.Infra.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly CatalogContext _context;

        public ProductRepository(CatalogContext context)
        {
            _context = context;
        }
        public void Create(Product product)
        {
            _context.Products.InsertOne(product);
        }

        public IEnumerable<Product> GetProducts()
        {
            var filter = new FilterDefinitionBuilder<Product>().Empty;

            return _context.Products.FindSync(filter).ToList();
        }
    }
}
