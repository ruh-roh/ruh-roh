using System;
using System.Linq.Expressions;
using Castle.DynamicProxy;

namespace RuhRoh.Affectors
{
    internal class ReturnValueChangerWithContext<T> : Affector
    {
        private readonly Expression<Func<T, T>> _valueExpression;
        private readonly object _lock = new object();
        private Func<T, T> _compiledExpression;

        public ReturnValueChangerWithContext(Expression<Func<T, T>> valueExpression)
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

            var returnValue = _compiledExpression((T)invocation.ReturnValue);
            invocation.ReturnValue = returnValue;
        }

        protected internal override bool RunsBeforeMethodExecution => false;
    }
}
