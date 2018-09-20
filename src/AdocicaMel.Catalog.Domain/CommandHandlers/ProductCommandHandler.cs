using AdocicaMel.Catalog.Domain.Commands;
using AdocicaMel.Catalog.Domain.Entities;
using AdocicaMel.Catalog.Domain.Repositories;
using AdocicaMel.Core.Domain.Commands;
using System;

namespace AdocicaMel.Catalog.Domain.CommandHandlers
{
    public class ProductCommandHandler : ICommandHandler<ImportProductCommand>
    {
        private readonly IProductVendorRepository _productVendorRepository;
        private readonly IProductRepository _productRepository;

        public ProductCommandHandler(IProductVendorRepository productVendorRepository,
            IProductRepository productRepository)
        {
            _productVendorRepository = productVendorRepository;
            _productRepository = productRepository;
        }

        public void Handle(ImportProductCommand command)
        {
            var productVendorData = _productVendorRepository
                .GetProductDataFromVendor(command.Vendor, command.ProductIdentifier);

            if (productVendorData == null)
            {
                throw new Exception("Não foi possível recuperar o produto do fornecedor");
            }

            var product = new Product(
                command.Price,
                command.Vendor,
                command.ProductIdentifier,
                command.Tags,
                productVendorData
            );

            _productRepository.Create(product);
        }
    }
}
