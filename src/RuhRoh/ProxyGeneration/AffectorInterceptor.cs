using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;

namespace RuhRoh.ProxyGeneration
{
    internal class AffectorInterceptor : IInterceptor
    {
        private readonly MethodInfo _method;
        private readonly IAffector[] _affectors;

        public AffectorInterceptor(MethodInfo method, IEnumerable<IAffector> affectors)
        {
            _method = method;
            _affectors = affectors.ToArray();
        }

        public void Intercept(IInvocation invocation)
        {
            // Don't intercept the wrong methods!
            if (!invocation.Method.Equals(_method))
            {
	            invocation.Proceed();
				return;
            }

            if (_affectors?.Length > 0)
            {
                // Get all triggers for all affectors that are updateable
                var updateableTriggers = _affectors.SelectMany(x => x.Triggers).OfType<IUpdatableTrigger>();
                foreach (var trigger in updateableTriggers)
                {
                    trigger.Update(); // Some triggers require updating to check if they need to trigger
                }

                // For each affector (exception thrower, delayer, ...)
                foreach (var affector in _affectors)
                {
                    var triggers = affector.Triggers; // Get the triggers configured on this affector, if any
                    if (triggers?.Length > 0)
                    {
                        // If any of the triggers would affect the call in their current state, execute the attached affector
                        if (triggers.Any(x => x.WillAffect()))
                        {
                            affector.Affect();
                        }
                    }
                    else
                    {
                        // No triggers? Then the affector will affect the call every time
                        affector.Affect();
                    }
                }
            }

            // Allow the call to proceed
            invocation.Proceed();
        }
    }
}