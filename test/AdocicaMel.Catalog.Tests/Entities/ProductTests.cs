using AdocicaMel.Catalog.Domain.Entities;
using AdocicaMel.Catalog.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AdocicaMel.Catalog.Tests.Entities
{
    public class ProductTests
    {
        private readonly ProductVendorData _productVendor = new ProductVendorData("Product Test", "Product Test Description", "image.jpg", 35);
        private readonly string[] _tags = new string[] { "tag1", "tag2" };
        private const string VENDOR = "testvendor";
        private const string PRODUCT_IDENTIFIER = "123";

        [Fact]
        public void Construct_ValidParameters_BeValid()
        {
            var product = new Product(50, VENDOR, PRODUCT_IDENTIFIER, _tags, _productVendor);

            Assert.True(product.Valid);
        }

        [Theory]
        [InlineData(0, VENDOR, PRODUCT_IDENTIFIER, "tag1,tag2")]
        [InlineData(50, "", PRODUCT_IDENTIFIER, "tag1,tag2")]
        [InlineData(50, VENDOR, "", "tag1,tag2")]
        [InlineData(50, VENDOR, "", null)]
        [InlineData(50, VENDOR, "", "")]
        public void Construct_InvalidParameters_BeInvalid(decimal price, string vendor, string productIdentifier, string tags)
        {
            var tagsArray = tags?.Split(",");

            var product = new Product(price, vendor, productIdentifier, tagsArray, _productVendor);

            Assert.False(product.Valid);
        }
    }
}
