using AdocicaMel.Catalog.Domain.CommandHandlers;
using AdocicaMel.Catalog.Domain.DomainServices;
using AdocicaMel.Catalog.Domain.Repositories;
using AdocicaMel.Catalog.Infra.Context;
using AdocicaMel.Catalog.Infra.Http;
using AdocicaMel.Catalog.Infra.Repositories;
using AdocicaMel.Core.Infra.DI;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AdocicaMel.Catalog.Api.DependencyInjection
{
    public class ServiceProviderBuilder : IServiceProviderBuilder
    {
        public IServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddScoped<CatalogContext>();
            services.AddScoped<VendorsApi>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IProductVendorRepository, ProductVendorRepository>();
            services.AddTransient<ITagRepository, TagRepository>();
            services.AddTransient<ITagService, TagService>();
            services.AddTransient<ProductCommandHandler>();

            return services.BuildServiceProvider(true);
        }
    }
}
