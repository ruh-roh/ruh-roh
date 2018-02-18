using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using Castle.DynamicProxy;
using RuhRoh.ArgumentMatchers;
using RuhRoh.ProxyGeneration;

namespace RuhRoh
{
    /// <inheritdoc cref="IAffectedMethod"/>
    public abstract class AffectedMethod : IAffectedMethod
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

        /// <inheritdoc cref="IAffectedMethod.Name"/>
        public string Name => Method?.Name;

        internal Expression OriginalExpression { get; set; }
        internal MethodInfo Method { get; set; }
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