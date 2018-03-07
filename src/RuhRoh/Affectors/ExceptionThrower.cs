using System;
using RuhRoh.Affectors.Internal;

namespace RuhRoh.Affectors
{
    internal class ExceptionThrower : Affector
    {
        private readonly Exception _exception;

        public ExceptionThrower(Exception exception)
        {
            _exception = exception;
        }

        protected internal sealed override void Affect()
        {
            throw _exception;
        }
    }
}