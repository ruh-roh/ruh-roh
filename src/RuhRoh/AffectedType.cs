using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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
        private readonly Func<T> _factoryMethod;

        internal AffectedType() 
        {
            // When no factory method is given, generate a default one using Activator
            // This is not really intended to be used in the real world though.
            _factoryMethod = Activator.CreateInstance<T>;
        }

        internal AffectedType(Func<T> factoryMethod)
        {
            _factoryMethod = factoryMethod;
        }

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

            var proxyGenOptions = new ProxyGenerationOptions(new AffectorProxyGenerationHook());
            
            if (typeof(T).GetTypeInfo().IsInterface)
            {
                return proxyGen.CreateInterfaceProxyWithTarget(
                    _factoryMethod(),
                    proxyGenOptions,
                    interceptors.ToArray());
            }

            return proxyGen.CreateClassProxyWithTarget(
                _factoryMethod(), 
                proxyGenOptions, 
                interceptors.ToArray());
        }
    }
}