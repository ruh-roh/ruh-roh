using System;
using System.Reflection;
using RuhRoh.Affectors;
using RuhRoh.Triggers;
using RuhRoh.Triggers.Internal;

namespace RuhRoh
{
    /// <summary>
    /// Extension methods to build chaos affectors and triggers
    /// </summary>
    public static class AffectedMethodExtensions
    {
        // Affectors

        /// <summary>
        /// Slows the <paramref name="affectedMethod"/> down by the given <paramref name="time"/>.
        /// </summary>
        /// <param name="affectedMethod">The method to affect.</param>
        /// <param name="time">A <see cref="TimeSpan"/> value indicating the amount of time used to slow down the call.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="time"/> equals zero or a negative amount of time.</exception>
        public static Affector SlowItDownBy(this IAffectedMethod affectedMethod, TimeSpan time)
        {
            if (time.Ticks <= 0)
            {
                // TODO Needs explanation
                throw new ArgumentOutOfRangeException(); // we can't speed up things
            }

            return (Affector)affectedMethod.AddAffector(new Delayer(time));
        }

        /// <summary>
        /// Throws an exception of type <typeparamref name="TException"/> when the <paramref name="affectedMethod" /> is called.
        /// </summary>
        /// <typeparam name="TException">A reference type derived from <see cref="Exception"/>.</typeparam>
        /// <param name="affectedMethod">The method to affect.</param>
        /// <exception cref="ArgumentException">Thrown when <typeparamref name="TException"/> is not referring to a reference type based on <see cref="Exception"/>.</exception>
        public static Affector Throw<TException>(this IAffectedMethod affectedMethod)
            where TException : Exception
        {
            return Throw(affectedMethod, typeof(TException));
        }

        /// <summary>
        /// Throws an exception of type <paramref name="exceptionType"/> when the <paramref name="affectedMethod" /> is called.
        /// </summary>
        /// <param name="affectedMethod">The method to affect.</param>
        /// <param name="exceptionType">A <see cref="Type"/> of <see cref="Exception"/> that should be thrown.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="exceptionType"/> is a null reference.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="exceptionType"/> is not referring to a reference type based on <see cref="Exception"/>.</exception>
        public static Affector Throw(this IAffectedMethod affectedMethod, Type exceptionType)
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

        /// <summary>
        /// Throws an <paramref name="exception"/> when the <paramref name="affectedMethod" /> is called.
        /// </summary>
        /// <param name="affectedMethod">The method to affect.</param>
        /// <param name="exception">The <see cref="Exception"/> that should be thrown.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="exception"/> is a null reference.</exception>
        public static Affector Throw(this IAffectedMethod affectedMethod, Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            return (Affector)affectedMethod.AddAffector(new ExceptionThrower(exception));
        }

        // Triggers

        /// <summary>
        /// Alters the behavior of the configured affector to trigger at random.
        /// </summary>
        /// <param name="affector">The configured affector</param>
        public static Affector AtRandom(this Affector affector)
        {
            ((IAffector)affector).AddTrigger(new RandomTrigger());
            return affector;
        }

        /// <summary>
        /// Alters the behavior of the configured affector to trigger after the given <paramref name="moment"/> in time.
        /// </summary>
        /// <param name="affector">The configured affector</param>
        /// <param name="moment">Absolute point in time after which this trigger will become active.</param>
        public static Affector After(this Affector affector, DateTime moment)
        {
            ((IAffector)affector).AddTrigger(new TimedTrigger(moment, TimedOperation.After));
            return affector;
        }

        /// <summary>
        /// Alters the behavior of the configured affector to trigger before the given <paramref name="moment"/> in time.
        /// </summary>
        /// <param name="affector">The configured affector</param>
        /// <param name="moment">Absolute point in time before which this trigger will be active.</param>
        public static Affector Before(this Affector affector, DateTime moment)
        {
            ((IAffector)affector).AddTrigger(new TimedTrigger(moment, TimedOperation.Before));
            return affector;
        }

        /// <summary>
        /// Alters the behavior of the configured affector to trigger between <paramref name="from"/> and <paramref name="until"/>.
        /// </summary>
        /// <param name="affector">The configured affector</param>
        /// <param name="from">Absolute point in time after which this trigger will become active.</param>
        /// <param name="until">Absolute point in time after which this trigger will become inactive again.</param>
        public static Affector Between(this Affector affector, DateTime from, DateTime until)
        {
            ((IAffector)affector).AddTrigger(new TimedTrigger(from, until));
            return affector;
        }

        /// <summary>
        /// Alters the behavior of the configured affector to trigger after the affected method has been called N times.
        /// </summary>
        /// <param name="affector">The configured affector</param>
        /// <param name="calls">The amount of calls after which the trigger becomes active.</param>
        public static Affector AfterNCalls(this Affector affector, int calls)
        {
            ((IAffector)affector).AddTrigger(new TimesCalledTrigger(TimesCalledOperation.After, calls));
            return affector;
        }

        /// <summary>
        /// Alters the behavior of the configured affector to trigger until the affected method has been called N times.
        /// </summary>
        /// <param name="affector">The configured affector</param>
        /// <param name="calls">The amount of calls when the trigger is active. After this amount of calls, the trigger becomes inactive.</param>
        public static Affector UntilNCalls(this Affector affector, int calls)
        {
            ((IAffector)affector).AddTrigger(new TimesCalledTrigger(TimesCalledOperation.Until, calls));
            return affector;
        }

        /// <summary>
        /// Alters the behavior of the configured affector to trigger when the affected method is being called for the N-th time.
        /// </summary>
        /// <param name="affector">The configured affector</param>
        /// <param name="calls">The amount of calls needed to activate the trigger once.</param>
        public static Affector WhenCalledNTimes(this Affector affector, int calls)
        {
            ((IAffector)affector).AddTrigger(new TimesCalledTrigger(TimesCalledOperation.At, calls));
            return affector;
        }

        /// <summary>
        /// Alters the behavior of the configured affector to trigger every time the affected method has been called N times.
        /// </summary>
        /// <param name="affector">The configured affector</param>
        /// TODO Might need better wording.
        /// <param name="calls">The amount of calls needed between calls to activate the trigger.</param>
        public static Affector EveryNCalls(this Affector affector, int calls)
        {
            ((IAffector)affector).AddTrigger(new TimesCalledTrigger(TimesCalledOperation.EveryXCalls, calls));
            return affector;
        }
    }
}