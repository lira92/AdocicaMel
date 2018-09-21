using Microsoft.Azure.WebJobs.Description;
using System;

namespace AdocicaMel.Core.Infra.DI
{
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class InjectAttribute : Attribute
    {
    }
}
