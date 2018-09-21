using AdocicaMel.Catalog.Domain.Commands;
using AdocicaMel.Catalog.Domain.Entities;
using AdocicaMel.Catalog.Domain.Repositories;
using AdocicaMel.Catalog.Domain.ValueObjects;
using AdocicaMel.Core.Domain.Commands;
using Flunt.Notifications;
using System;

namespace AdocicaMel.Catalog.Domain.CommandHandlers
{
    public class ProductCommandHandler : Notifiable, ICommandHandler<ImportProductCommand>
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
            command.Validate();
            if(command.Invalid)
            {
                AddNotifications(command);
                return;
            }
            var productAlreadyImported = _productRepository.GetProductByVendorAndProductIdentifier(command.Vendor, command.ProductIdentifier);

            if(productAlreadyImported != null)
            {
                AddNotification("Product", "Este produto já foi importado");
                return;
            }

            ProductVendorData productVendorData = null;
            try
            {
                productVendorData = _productVendorRepository
                    .GetProductDataFromVendor(command.Vendor, command.ProductIdentifier);

                if (productVendorData == null)
                {
                    AddNotification("Product", "Não foi possível recuperar o produto do fornecedor");
                    return;
                }
            }
            catch (Exception)
            {
                AddNotification("Product", "Não foi possível recuperar o produto do fornecedor");
                return;
            }
            
            var product = new Product(
                command.Price,
                command.Vendor,
                command.ProductIdentifier,
                command.Tags,
                productVendorData
            );

            AddNotifications(product);

            if(Invalid)
            {
                return;
            }

            _productRepository.Create(product);
        }
    }
}
