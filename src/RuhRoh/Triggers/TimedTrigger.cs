using System;
using RuhRoh.Triggers.Internal;

namespace RuhRoh.Triggers
{
    internal class TimedTrigger : ITrigger
    {
        private readonly DateTime _when;
        private readonly DateTime _end;
        private readonly TimedOperation _operation;

        internal TimedTrigger(DateTime when, TimedOperation operation)
        {
            if (operation == TimedOperation.Between)
            {
                throw new ArgumentException("The Between operation can only be used when specifying two moments in time.", nameof(operation));
            }

            _when = when;
            _operation = operation;
        }

        public TimedTrigger(DateTime from, DateTime until)
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
            switch (_operation)
            {
                case TimedOperation.After:
                    return DateTime.Now > _when;
                case TimedOperation.Before:
                    return DateTime.Now < _when;
                case TimedOperation.Between:
                    return _when <= DateTime.Now && DateTime.Now <= _end;
            }

            return false;
        }
    }
}