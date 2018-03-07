using System;
using RuhRoh.Triggers.Internal;

namespace RuhRoh.Triggers
{
    internal class TimedTrigger : ITrigger
    {
        private readonly TimeSpan _when;
        private readonly TimeSpan _end;
        private readonly TimedOperation _operation;

        internal TimedTrigger(TimeSpan when, TimedOperation operation)
        {
            if (operation == TimedOperation.Between)
            {
                throw new ArgumentException("The Between operation can only be used when specifying two moments in time.", nameof(operation));
            }

            _when = when;
            _operation = operation;
        }

        public TimedTrigger(TimeSpan from, TimeSpan until)
        {
            if (from > until)
            {
                throw new ArgumentException("\"Until\" should be a moment after \"from\"", nameof(until));
            }

            _when = from;
            _end = until;

            _operation = TimedOperation.Between;
        }

        bool ITrigger.WillAffect()
        {
            var now = DateTime.Now.TimeOfDay;

            switch (_operation)
            {
                case TimedOperation.After:
                    return now > _when;
                case TimedOperation.Before:
                    return now < _when;
                case TimedOperation.Between:
                    return _when <= now && now <= _end;
            }

            return false;
        }
    }
}