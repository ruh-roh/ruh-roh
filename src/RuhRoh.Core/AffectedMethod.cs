using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

namespace RuhRoh.Core
{
    public abstract class AffectedMethod : IAffectedMethod
    {
        protected AffectedMethod()
        {
            Affectors = new Collection<IAffector>();
        }

        protected internal ICollection<IAffector> Affectors { get; }

        public void AddAffector(IAffector affector)
        {
            Affectors.Add(affector);
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

        public MethodInfo Method { get; }
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
        internal MethodInfo Method { get; }
        internal IReadOnlyCollection<Expression> Arguments { get; }
    }
}