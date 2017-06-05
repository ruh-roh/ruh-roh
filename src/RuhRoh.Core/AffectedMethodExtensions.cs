using System;
using RuhRoh.Core.Affectors;
using System.Reflection;
using RuhRoh.Core.Triggers;
using RuhRoh.Core.Triggers.Internal;
using Random = RuhRoh.Core.Triggers.Random;

namespace RuhRoh.Core
{
    public static class AffectedMethodExtensions
    {
        // Affectors

        public static IAffectedMethod SlowItDownBy(this IAffectedMethod affectedMethod, TimeSpan time)
        {
            if (time.Ticks <= 0)
            {
                return affectedMethod; // we can't speed up things
            }

            affectedMethod.AddAffector(new Delayer(time));
            return affectedMethod;
        }

        public static IAffectedMethod Throw<TException>(this IAffectedMethod affectedMethod)
            where TException : Exception
        {
            return Throw(affectedMethod, typeof(TException));
        }

        public static IAffectedMethod Throw(this IAffectedMethod affectedMethod, Type exceptionType)
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
            return Throw(affectedMethod, ex);
        }

        public static IAffectedMethod Throw(this IAffectedMethod affectedMethod, Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            affectedMethod.AddAffector(new ExceptionThrower(exception));
            return affectedMethod;
        }

        // Triggers

        public static IAffectedMethod AtRandom(this IAffectedMethod affectedMethod)
        {
            affectedMethod.AddTrigger(new Random());
            return affectedMethod;
        }

        public static IAffectedMethod After(this IAffectedMethod affectedMethod, DateTime moment)
        {
            affectedMethod.AddTrigger(new Timed(moment, TimedOperation.After));
            return affectedMethod;
        }

        public static IAffectedMethod Before(this IAffectedMethod affectedMethod, DateTime moment)
        {
            affectedMethod.AddTrigger(new Timed(moment, TimedOperation.Before));
            return affectedMethod;
        }

        public static IAffectedMethod Between(this IAffectedMethod affectedMethod, DateTime from, DateTime until)
        {
            affectedMethod.AddTrigger(new Timed(from, until));
            return affectedMethod;
        }

        public static IAffectedMethod AfterNCalls(this IAffectedMethod affectedMethod, int calls)
        {
            affectedMethod.AddTrigger(new TimesCalled(TimesCalledOperation.After, calls));
            return affectedMethod;
        }

        public static IAffectedMethod UntilNCalls(this IAffectedMethod affectedMethod, int calls)
        {
            affectedMethod.AddTrigger(new TimesCalled(TimesCalledOperation.Until, calls));
            return affectedMethod;
        }

        public static IAffectedMethod WhenCalledNTimes(this IAffectedMethod affectedMethod, int calls)
        {
            affectedMethod.AddTrigger(new TimesCalled(TimesCalledOperation.At, calls));
            return affectedMethod;
        }

        public static IAffectedMethod EveryNCalls(this IAffectedMethod affectedMethod, int calls)
        {
            affectedMethod.AddTrigger(new TimesCalled(TimesCalledOperation.EveryXCalls, calls));
            return affectedMethod;
        }
    }
}