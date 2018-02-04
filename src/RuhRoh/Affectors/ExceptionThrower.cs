using System;
using RuhRoh.Affectors.Internal;

namespace RuhRoh.Affectors
{
    internal class ExceptionThrower : AffectorBase
    {
        private readonly Exception _exception;

        public ExceptionThrower(Exception exception)
        {
            _exception = exception;
        }

        public sealed override void Affect()
        {
            throw _exception;
        }
    }
}