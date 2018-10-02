using AdocicaMel.Catalog.Domain.CommandHandlers;
using AdocicaMel.Catalog.Domain.Commands;
using AdocicaMel.Catalog.Domain.DomainServices;
using AdocicaMel.Catalog.Domain.Entities;
using AdocicaMel.Catalog.Domain.Repositories;
using AdocicaMel.Catalog.Domain.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace AdocicaMel.Catalog.Tests.CommandHandlers
{
    public class ProductCommandHandlerTests
    {
        private readonly Mock<IProductVendorRepository> _productVendorRepositoryMock = new Mock<IProductVendorRepository>();
        private readonly Mock<IProductRepository> _productRepository = new Mock<IProductRepository>();
        private readonly Mock<ITagService> _tagService = new Mock<ITagService>();
        private readonly ProductVendorData _productVendor = new ProductVendorData("Product Test", "Product Test Description", "image.jpg", 35);
        private readonly string[] _tags = new string[] { "tag1", "tag2" };
        private const string VENDOR = "testvendor";
        private const string PRODUCT_IDENTIFIER = "123";

        [Fact]
        public async void Handle_ValidProduct_ImportSuccessfully()
        {
            // Arrange
            _productVendorRepositoryMock
                .Setup(x => x.GetProductDataFromVendor(VENDOR, PRODUCT_IDENTIFIER, null))
                .Returns(_productVendor);

            var productCommandHandler = new ProductCommandHandler(
                _productVendorRepositoryMock.Object,
                _productRepository.Object,
                _tagService.Object
            );

            // Act
            await productCommandHandler.Handle(new ImportProductCommand
            {
                Price = 50,
                ProductIdentifier = PRODUCT_IDENTIFIER,
                Tags = _tags,
                Vendor = VENDOR
            });

            // Assert
            Assert.True(productCommandHandler.Valid);
            _tagService.Verify(x => x.CreateTagIfNotExists(_tags), Times.Once());
            _productRepository.Verify(x => x.Create(It.IsAny<Product>()), Times.Once());
        }

        [Fact]
        public async void Handle_ProductAlreadyImported_ReturnNotification()
        {
            // Arrange
            _productRepository
                .Setup(x => x.GetProductByVendorAndProductIdentifier(VENDOR, PRODUCT_IDENTIFIER))
                .ReturnsAsync(new Product(50, VENDOR, PRODUCT_IDENTIFIER, _tags, _productVendor));

            var productCommandHandler = new ProductCommandHandler(
                _productVendorRepositoryMock.Object,
                _productRepository.Object,
                _tagService.Object
            );

            // Act
            await productCommandHandler.Handle(new ImportProductCommand
            {
                Price = 50,
                ProductIdentifier = PRODUCT_IDENTIFIER,
                Tags = _tags,
                Vendor = VENDOR
            });

            // Assert
            Assert.True(productCommandHandler.Invalid);
            Assert.Contains(productCommandHandler.Notifications, x => x.Message == "Este produto já foi importado");
            _tagService.Verify(x => x.CreateTagIfNotExists(_tags), Times.Never());
            _productRepository.Verify(x => x.Create(It.IsAny<Product>()), Times.Never());
        }

        [Fact]
        public async void Handle_ProductVendorRepositoryThrowsException_ReturnNotification()
        {
            // Arrange
            _productVendorRepositoryMock
                .Setup(x => x.GetProductDataFromVendor(VENDOR, PRODUCT_IDENTIFIER, null))
                .Throws<Exception>();

            var productCommandHandler = new ProductCommandHandler(
                _productVendorRepositoryMock.Object,
                _productRepository.Object,
                _tagService.Object
            );

            // Act
            await productCommandHandler.Handle(new ImportProductCommand
            {
                Price = 50,
                ProductIdentifier = PRODUCT_IDENTIFIER,
                Tags = _tags,
                Vendor = VENDOR
            });

            // Assert
            Assert.True(productCommandHandler.Invalid);
            Assert.Contains(productCommandHandler.Notifications, x => x.Message == "Não foi possível recuperar o produto do fornecedor");
            _tagService.Verify(x => x.CreateTagIfNotExists(_tags), Times.Never());
            _productRepository.Verify(x => x.Create(It.IsAny<Product>()), Times.Never());
        }
    }
}
