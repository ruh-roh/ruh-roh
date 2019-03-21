using Castle.DynamicProxy;
using System;

namespace RuhRoh.Affectors
{
    internal sealed class ExceptionThrower : Affector
    {
        private readonly Exception _exception;

        public ExceptionThrower(Exception exception)
        {
            _exception = exception;
        }

        protected internal override void Affect(IInvocation invocation)
        {
            throw _exception;
        }
    }
}