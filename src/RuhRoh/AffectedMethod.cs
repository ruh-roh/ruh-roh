using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using Castle.DynamicProxy;
using RuhRoh.Core.ProxyGeneration;

namespace RuhRoh.Core
{
    public abstract class AffectedMethod : IAffectedMethod
    {
        protected AffectedMethod()
        {
            Affectors = new Collection<IAffector>();
        }

        public string Name => Method?.Name;

        public MethodInfo Method { get; protected internal set; }
        protected internal ICollection<IAffector> Affectors { get; }

        public IAffector AddAffector(IAffector affector)
        {
            Affectors.Add(affector);
            return affector;
        }

        public void AddTrigger(IAffector affector, ITrigger trigger)
        {
            affector.AddTrigger(trigger);
        }

        public IInterceptor GetInterceptor()
        {
            return new AffectorInterceptor(Method, Affectors);
        }
    }

    public class AffectedMethod<T> : AffectedMethod
         where T : class
    {
        internal AffectedMethod(MethodCallExpression methodCall)
        {
            Method = methodCall.Method;
            Arguments = methodCall.Arguments;
            Object = methodCall.Object;
        }

        public IReadOnlyCollection<Expression> Arguments { get; }
        public Expression Object { get; }
    }

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
        internal Expression OriginalExpression { get; }
        internal IReadOnlyCollection<Expression> Arguments { get; }
    }
}