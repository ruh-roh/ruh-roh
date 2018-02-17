using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Castle.DynamicProxy;

namespace RuhRoh.ProxyGeneration
{
    /// <summary>
    /// Used to intercept configured affected methods and change their behavior when certain conditions have been met.
    /// </summary>
    internal class AffectorInterceptor : IInterceptor
    {
        private readonly MethodInfo _method;
        private readonly Expression[] _arguments;
        private readonly IAffector[] _affectors;

        public AffectorInterceptor(MethodInfo method, IEnumerable<Expression> arguments, IEnumerable<IAffector> affectors)
        {
            _method = method;
            _arguments = arguments.ToArray();
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
            if (invocation.Arguments.Length > 0 && invocation.Arguments.Length == _arguments.Length)
            {
                for (var i = 0; i < invocation.Arguments.Length; i++)
                {
                    var invokedValue = invocation.Arguments[i];
                    var definedValue = GetValueFromExpression(_arguments[i]);
                    if (!Equals(invokedValue, definedValue))
                    {
                        // Arguments at the same index mismatch => this method call should not be intercepted.
                        return false; 
                    }
                }
            }

            return true;
        }

        private object GetValueFromExpression(Expression e)
        {
            switch (e)
            {
                case ConstantExpression c:
                    return c.Value;
            }

            // TODO Move to resx
            throw new InvalidOperationException("Unsupported argument expression type");
        }
    }
}