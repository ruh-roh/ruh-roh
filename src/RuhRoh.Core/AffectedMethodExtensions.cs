using System;
using RuhRoh.Core.Affectors;
using System.Reflection;

namespace RuhRoh.Core
{
    public static class AffectedMethodExtensions
    {
        public static void SlowItDownBy(this IAffectedMethod affectedMethod, TimeSpan time)
        {
            if (time.Ticks <= 0)
            {
                return; // we can't speed up things
            }

            affectedMethod.AddAffector(new Delayer(time));
        }

        public static void Throw<TException>(this IAffectedMethod affectedMethod)
            where TException : Exception
        {
            Throw(affectedMethod, typeof(TException));
        }

        public static void Throw(this IAffectedMethod affectedMethod, Type exceptionType)
        {
            if (exceptionType == null)
            {
                throw new ArgumentNullException(nameof(exceptionType));
            }

            var ti = exceptionType.GetTypeInfo();
            if (!ti.IsAssignableFrom(typeof(Exception).GetTypeInfo()) && !ti.IsSubclassOf(typeof(Exception)))
            {
                //TODO move to resources file
                throw new ArgumentException("The given exception type is not System.Exception nor derived from System.Exception", nameof(exceptionType));
            }

            var ex = Activator.CreateInstance(exceptionType) as Exception;
            Throw(affectedMethod, ex);
        }

        public static void Throw(this IAffectedMethod affectedMethod, Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            affectedMethod.AddAffector(new ExceptionThrower(exception));
        }
    }
}