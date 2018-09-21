using AdocicaMel.Core.Infra.DI;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: WebJobsStartup(typeof(WebJobsExtensionStartup))]
namespace AdocicaMel.Core.Infra.DI
{
    public class WebJobsExtensionStartup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.Services.AddSingleton<InjectBindingProvider>();

            //Registering a filter
            builder.Services.AddSingleton<IFunctionFilter, ScopeCleanupFilter>();

            //Registering an extension
            builder.AddExtension<InjectConfiguration>(); 
        }
    }
}
