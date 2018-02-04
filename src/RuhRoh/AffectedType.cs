using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Castle.DynamicProxy;
using RuhRoh.ProxyGeneration;

namespace RuhRoh
{
    /// <summary>
    /// Represents an affected service.
    /// </summary>
    /// <typeparam name="T">Type of the original service.</typeparam>
    public class AffectedType<T>
        where T : class
    {
        private readonly Dictionary<string, IAffectedMethod> _affectedMethods = new Dictionary<string, IAffectedMethod>();

        internal AffectedType() { }

        /// <summary>
        /// Configures the method defined by <paramref name="expression"/>.
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="expression"></param>
        public AffectedMethod<T, TOut> WhenCalling<TOut>(Expression<Func<T, TOut>> expression)
        {
            if (!(expression.Body is MethodCallExpression mc))
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

        /// <summary>
        /// Retrieves an instance of the configured service.
        /// </summary>
        public T Instance => BuildInstance();

        private T BuildInstance()
        {
            var proxyGen = new ProxyGenerator();
            var interceptors = new List<IInterceptor>();

            foreach (var affectedMethod in _affectedMethods.Values)
            {
                interceptors.Add(((AffectedMethod)affectedMethod).GetInterceptor());
            }

            return proxyGen.CreateClassProxy<T>(
                new ProxyGenerationOptions(new AffectorProxyGenerationHook()),
                interceptors.ToArray());
        }
    }
}