using System;
using Microsoft.Extensions.DependencyInjection;

namespace RuhRoh.Extensions.Microsoft.DependencyInjection
{
    public class AffectedService<T, TImplementation> : AffectedType<T>
        where T : class
        where TImplementation : T
    {
        internal AffectedService()
        {

        }

        public T GetInstance(IServiceProvider serviceProvider)
        {
            return BuildInstance(() => serviceProvider.GetService<TImplementation>());
        }
    }
}