using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using Castle.DynamicProxy;
using RuhRoh.ProxyGeneration;

namespace RuhRoh
{
    /// <inheritdoc cref="IAffectedMethod"/>
    public abstract class AffectedMethod : IAffectedMethod
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        protected AffectedMethod()
        {
            Affectors = new Collection<IAffector>();
        }

        /// <inheritdoc cref="IAffectedMethod.Name"/>
        public string Name => Method?.Name;

        internal Expression OriginalExpression { get; set; }
        internal MethodInfo Method { get; set; }
        internal IReadOnlyCollection<Expression> Arguments { get; set; }

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
            return new AffectorInterceptor(Method, Arguments, Affectors);
        }
    }

    /// <inheritdoc cref="IAffectedMethod"/>
    public class AffectedMethod<T> : AffectedMethod
         where T : class
    {
        internal AffectedMethod(MethodCallExpression methodCall)
        {
            Method = methodCall.Method;
            Arguments = methodCall.Arguments;
            Object = methodCall.Object;
        }

        internal Expression Object { get; }
    }

    /// <inheritdoc cref="IAffectedMethod"/>
    public class AffectedMethod<T, TOut> : AffectedMethod
        where T : class
    {
        internal AffectedMethod(AffectedType<T> affectedType, Expression originalExpression, MethodInfo methodInfo, Expression[] arguments)
        {
            AffectedType = affectedType;
            OriginalExpression = originalExpression;
            Method = methodInfo;
            Arguments = arguments;
        }

        internal AffectedType<T> AffectedType { get; }
    }
}