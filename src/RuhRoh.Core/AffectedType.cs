using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace RuhRoh.Core
{
    public class AffectedType<T> where T : class
    {
        private readonly List<IAffectedMethod> _affectedMethods = new List<IAffectedMethod>();

        internal AffectedType() { }

        public AffectedMethod<T, TOut> When<TOut>(Expression<Func<T, TOut>> functionExpression)
        {
            var expression = functionExpression.Body as MethodCallExpression;
            if (expression == null)
            {
                // TODO Move to constants/resx
                throw new ArgumentException("invalid expression type");
            }

            var affectedMethod = new AffectedMethod<T, TOut>(this, expression, expression.Method, expression.Arguments.ToArray());
            _affectedMethods.Add(affectedMethod);

            return affectedMethod;
        }

        public T Instance => BuildInstance();

        private T BuildInstance()
        {
            var typeInfo = typeof(T).GetTypeInfo();

            foreach (var affectedMethod in _affectedMethods)
            {
                
            }


            return default(T);
        }
    }
}