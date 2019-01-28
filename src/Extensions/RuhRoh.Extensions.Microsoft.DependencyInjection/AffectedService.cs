using System;
using Microsoft.Extensions.DependencyInjection;

namespace RuhRoh.Extensions.Microsoft.DependencyInjection
{
    internal class AffectedService<T, TImplementation> : AffectedType<T>
        where T : class
        where TImplementation : T
    {
        internal T GetInstance(IServiceProvider serviceProvider)
        {
            return BuildInstance(() => serviceProvider.GetService<TImplementation>());
        }
    }
}