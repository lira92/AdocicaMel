﻿using AdocicaMel.Catalog.Domain.Validators;
using AdocicaMel.Catalog.Domain.ValueObjects;
using AdocicaMel.Core.Domain.Entities;
using Flunt.Validations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdocicaMel.Catalog.Domain.Entities
{
    public class Product : Entity
    {
        public Product(decimal price, string vendor,
            string productVendorIdentifier, IEnumerable<string> tags,
            ProductVendorData productVendorData)
        {
            Price = price;
            Vendor = vendor;
            ProductVendorIdentifier = productVendorIdentifier;
            CreatedAt = DateTime.Now;
            LastSync = DateTime.Now;
            Tags = tags;
            ProductVendorData = productVendorData;

            var contract = new ProductContract(Vendor, ProductVendorIdentifier, Price, Tags);

            AddNotifications(contract, productVendorData);
        }

        public decimal Price { get; private set; }
        public string Vendor { get; private set; }
        public string ProductVendorIdentifier { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime LastSync { get; set; }
        public IEnumerable<string> Tags { get; private set; }
        public ProductVendorData ProductVendorData { get; private set; }

        public void UpdateVendorData(ProductVendorData productVendorData)
        {
            LastSync = DateTime.Now;
            ProductVendorData = productVendorData;
        }
    }
}
