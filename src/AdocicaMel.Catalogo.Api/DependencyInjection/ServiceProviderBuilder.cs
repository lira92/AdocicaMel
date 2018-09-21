using AdocicaMel.Catalog.Domain.Repositories;
using AdocicaMel.Catalog.Infra.Context;
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
            services.AddTransient<IProductRepository, ProductRepository>();

            return services.BuildServiceProvider(true);
        }
    }
}
