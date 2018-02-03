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

        public static IAffector SlowItDownBy(this IAffectedMethod affectedMethod, TimeSpan time)
        {
            if (time.Ticks <= 0)
            {
                // TODO Needs explanation
                throw new ArgumentOutOfRangeException(); // we can't speed up things
            }

            return affectedMethod.AddAffector(new Delayer(time));
        }

        public static IAffector Throw<TException>(this IAffectedMethod affectedMethod)
            where TException : Exception
        {
            return Throw(affectedMethod, typeof(TException));
        }

        public static IAffector Throw(this IAffectedMethod affectedMethod, Type exceptionType)
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

        public static IAffector Throw(this IAffectedMethod affectedMethod, Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            return affectedMethod.AddAffector(new ExceptionThrower(exception));
        }

        // Triggers

        public static IAffector AtRandom(this IAffector affector)
        {
            affector.AddTrigger(new Random());
            return affector;
        }

        public static IAffector After(this IAffector affector, DateTime moment)
        {
            affector.AddTrigger(new Timed(moment, TimedOperation.After));
            return affector;
        }

        public static IAffector Before(this IAffector affector, DateTime moment)
        {
            affector.AddTrigger(new Timed(moment, TimedOperation.Before));
            return affector;
        }

        public static IAffector Between(this IAffector affector, DateTime from, DateTime until)
        {
            affector.AddTrigger(new Timed(from, until));
            return affector;
        }

        public static IAffector AfterNCalls(this IAffector affector, int calls)
        {
            affector.AddTrigger(new TimesCalled(TimesCalledOperation.After, calls));
            return affector;
        }

        public static IAffector UntilNCalls(this IAffector affector, int calls)
        {
            affector.AddTrigger(new TimesCalled(TimesCalledOperation.Until, calls));
            return affector;
        }

        public static IAffector WhenCalledNTimes(this IAffector affector, int calls)
        {
            affector.AddTrigger(new TimesCalled(TimesCalledOperation.At, calls));
            return affector;
        }

        public static IAffector EveryNCalls(this IAffector affector, int calls)
        {
            affector.AddTrigger(new TimesCalled(TimesCalledOperation.EveryXCalls, calls));
            return affector;
        }
    }
}