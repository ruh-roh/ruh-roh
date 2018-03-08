using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using RuhRoh.Affectors;
using RuhRoh.ArgumentMatchers;

namespace RuhRoh.ProxyGeneration
{
    /// <summary>
    /// Used to intercept configured affected methods and change their behavior when certain conditions have been met.
    /// </summary>
    internal class AffectorInterceptor : IInterceptor
    {
        private readonly MethodInfo _method;
        private readonly IArgumentMatcher[] _matchers;
        private readonly IAffector[] _affectors;

        public AffectorInterceptor(MethodInfo method, IEnumerable<IArgumentMatcher> matchers, IEnumerable<IAffector> affectors)
        {
            _method = method;
            _matchers = matchers.ToArray();
            _affectors = affectors.ToArray();
        }

        public void Intercept(IInvocation invocation)
        {
            if (!ShouldIntercept(invocation))
            {
                invocation.Proceed();
                return;
            }

            if (_affectors?.Length > 0)
            {
                foreach (var updatableTrigger in _affectors.SelectMany(x => x.Triggers).OfType<IUpdatableTrigger>())
                {
                    updatableTrigger.Update();
                }

                // For each affector (exception thrower, delayer, ...)
                foreach (var affector in _affectors)
                {
                    var triggers = affector.Triggers; // Get the triggers configured on this affector, if any
                    if (triggers?.Length > 0)
                    {
                        foreach (var trigger in triggers)
                        {
                            if (trigger.WillAffect())
                            {
                                affector.Affect();
                                break;
                            }
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

        /// <summary>
        /// Returns <c>true</c> if the <paramref name="invocation"/> should be intercepted by this <see cref="AffectorInterceptor"/>.
        /// </summary>
        /// <param name="invocation">Information about the method being invoked.</param>
        private bool ShouldIntercept(IInvocation invocation)
        {
            // Don't intercept the wrong methods!
            if (!invocation.Method.Equals(_method))
            {
                return false;
            }

            // Don't intercept when the method has arguments that are different than the ones we were given
            // This allows users to configure different interceptors for different arguments (or allow methods
            // to pass through for certain argument values).
            if (invocation.Arguments.Length > 0 && invocation.Arguments.Length == _matchers.Length)
            {
                for (var i = 0; i < invocation.Arguments.Length; i++)
                {
                    var invokedValue = invocation.Arguments[i];
					if (!_matchers[i].Matches(invokedValue)) 
					{
                        // Arguments at the same index mismatch => this method call should not be intercepted.
                        return false; 
                    }
                }
            }

            return true;
        }
    }
}