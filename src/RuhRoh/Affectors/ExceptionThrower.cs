using System;
using RuhRoh.Core.Affectors.Internal;

namespace RuhRoh.Core.Affectors
{
    public class ExceptionThrower : AffectorBase
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