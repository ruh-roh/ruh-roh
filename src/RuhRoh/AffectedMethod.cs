using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using Castle.DynamicProxy;
using RuhRoh.Affectors;
using RuhRoh.ArgumentMatchers;
using RuhRoh.ProxyGeneration;

namespace RuhRoh
{
    /// <inheritdoc cref="IAffectedMethod"/>
    public abstract class AffectedMethod : IAffectedMethod, IEquatable<AffectedMethod>
    {
        private readonly List<IArgumentMatcher> _argumentMatchers = new List<IArgumentMatcher>();

        /// <summary>
        /// Constructor.
        /// </summary>
        protected AffectedMethod(Expression originalExpression, MethodInfo methodInfo, params Expression[] arguments)
        {
            Affectors = new Collection<IAffector>();

            OriginalExpression = originalExpression;
            Method = methodInfo;

            InitializeArgumentMatchers(arguments);
        }

        internal Expression OriginalExpression { get; }
        internal MethodInfo Method { get; }
        internal IReadOnlyCollection<IArgumentMatcher> ArgumentMatchers => _argumentMatchers;

        internal ICollection<IAffector> Affectors { get; }
        
        IAffector IAffectedMethod.AddAffector(IAffector affector)
        {
            Affectors.Add(affector);
            return affector;
        }

        void IAffectedMethod.AddTrigger(IAffector affector, ITrigger trigger)
        {
            affector.AddTrigger(trigger);
        }

        internal IInterceptor GetInterceptor()
        {
            return new AffectorInterceptor(Method, ArgumentMatchers, Affectors);
        }

        private void InitializeArgumentMatchers(Expression[] arguments)
        {
            var methodParams = Method.GetParameters();
            for (var i = 0; i < methodParams.Length; i++)
            {
                var argument = arguments[i];

                _argumentMatchers.Add(ArgumentMatcher.Create(argument));
            }
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            unchecked
            {
                int hc = 0;
                if (Method != null)
                {
                    hc = Method.GetHashCode();
                }

                if (_argumentMatchers != null)
                {
                    foreach (var argumentMatcher in _argumentMatchers)
                    {
                        hc = (hc * 397) ^ argumentMatcher.GetHashCode();
                    }
                }

                return hc;
            }
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(AffectedMethod other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(_argumentMatchers, other._argumentMatchers) && Equals(Method, other.Method);
        }

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return Equals((AffectedMethod) obj);
        }
    }

    /// <inheritdoc cref="IAffectedMethod"/>
    public class AffectedMethod<T> : AffectedMethod
         where T : class
    {
        internal AffectedMethod(Expression originalExpression, MethodInfo methodInfo, params Expression[] arguments)
            : base(originalExpression, methodInfo, arguments)
        {
        }
    }

    /// <inheritdoc cref="IAffectedMethod"/>
    public class AffectedMethod<T, TOut> : AffectedMethod
        where T : class
    {
        internal AffectedMethod(AffectedType<T> affectedType, Expression originalExpression, MethodInfo methodInfo, params Expression[] arguments) 
            : base(originalExpression, methodInfo, arguments)
        
        {
            AffectedType = affectedType;
        }

        internal AffectedType<T> AffectedType { get; }
    }
}