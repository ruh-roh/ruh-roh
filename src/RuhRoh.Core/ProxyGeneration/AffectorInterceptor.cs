using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;

namespace RuhRoh.Core.ProxyGeneration
{
    internal class AffectorInterceptor : IInterceptor
    {
        private readonly IAffector[] _affectors;

        public AffectorInterceptor(IEnumerable<IAffector> affectors)
        {
            _affectors = affectors.ToArray();
        }

        public void Intercept(IInvocation invocation)
        {
            if (_affectors?.Length > 0)
            {
                foreach (var affector in _affectors)
                {
                    affector.Affect();
                }
            }

            invocation.Proceed();
        }
    }
}