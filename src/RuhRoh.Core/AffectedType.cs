using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Castle.DynamicProxy;
using RuhRoh.Core.ProxyGeneration;

namespace RuhRoh.Core
{
    public class AffectedType
    {
    }

    public class AffectedType<T> : AffectedType
        where T : class
    {
        private readonly Dictionary<string, IAffectedMethod> _affectedMethods = new Dictionary<string, IAffectedMethod>();

        internal AffectedType() { }

        public AffectedMethod<T, TOut> WhenCalling<TOut>(Expression<Func<T, TOut>> expression)
        {
            var mc = expression.Body as MethodCallExpression;
            if (mc == null)
            {
                // TODO Move to constants/resx
                throw new ArgumentException("invalid expression type");
            }
            // throw if you can't override the method

            var affectedMethod = new AffectedMethod<T, TOut>(this, expression, mc.Method, mc.Arguments.ToArray());
            if (!_affectedMethods.TryGetValue(affectedMethod.Name, out var af2))
            {
                _affectedMethods.Add(affectedMethod.Name, affectedMethod);
            }
            else
            {
                // Return the existing affected method instance to add new affectors to it
                affectedMethod = (AffectedMethod<T, TOut>) af2;
            }

            return affectedMethod;
        }

        public T Instance => BuildInstance();

        private T BuildInstance()
        {
            var proxyGen = new ProxyGenerator();
            var interceptors = new List<IInterceptor>();

            foreach (var affectedMethod in _affectedMethods.Values)
            {
                interceptors.Add(affectedMethod.GetInterceptor());
            }

            return proxyGen.CreateClassProxy<T>(
                new ProxyGenerationOptions(new AffectorProxyGenerationHook()),
                interceptors.ToArray());
        }
    }
}