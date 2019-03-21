using Castle.DynamicProxy;
using System;
using System.Linq.Expressions;

namespace RuhRoh.Affectors
{
    internal class ReturnValueChanger<T> : Affector
    {
        private readonly Expression<Func<T>> _valueExpression;
        private readonly object _lock = new object();

        private Func<T> _compiledExpression = null;

        public ReturnValueChanger(Expression<Func<T>> valueExpression)
        {
            _valueExpression = valueExpression;
        }

        protected internal override void Affect(IInvocation invocation)
        {
            lock(_lock)
            {
                if (_compiledExpression == null)
                {
                    _compiledExpression = _valueExpression.Compile();
                }
            }

            invocation.ReturnValue = _compiledExpression();
        }

        /// <summary>
        /// This affector should execute after the affected method runs, because
        /// we need to change its output value.
        /// </summary>
        protected internal override bool RunsBeforeMethodExecution => false;
    }
}
