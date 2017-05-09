using System;

namespace RuhRoh.Core.Affectors
{
    public class ExceptionThrower : IAffector
    {
        private readonly Exception _exception;

        public ExceptionThrower(Exception exception)
        {
            _exception = exception;
        }

        public void Affect()
        {
            throw _exception;
        }
    }
}