using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;

namespace RuhRoh.Core.ProxyGeneration
{
    internal class AffectorInterceptor : IInterceptor
    {
        private readonly MethodInfo _method;
        private readonly IAffector[] _affectors;
        private readonly ITrigger[] _triggers;

        public AffectorInterceptor(MethodInfo method, IEnumerable<IAffector> affectors, IEnumerable<ITrigger> triggers)
        {
            _method = method;
            _affectors = affectors.ToArray();
            _triggers = triggers.ToArray();
        }

        public void Intercept(IInvocation invocation)
        {
            if (!invocation.Method.Equals(_method))
            {
	            invocation.Proceed();
				return;
            }

            if (_triggers?.Length > 0)
            {
                foreach (var trigger in _triggers.OfType<IUpdatableTrigger>())
                {
                    trigger.Update();
                }

                if (_triggers.All(x => !x.WillAffect()))
                {
                    invocation.Proceed();
                    return;
                }
            }

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