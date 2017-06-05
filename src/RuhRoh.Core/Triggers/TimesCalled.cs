using RuhRoh.Core.Triggers.Internal;

namespace RuhRoh.Core.Triggers
{
    public class TimesCalled : ITrigger
    {
        private readonly TimesCalledOperation _operation;
        private readonly int _trigger;

        public TimesCalled(TimesCalledOperation operation, int trigger)
        {
            _operation = operation;
            _trigger = trigger;
        }

        public int ActualTimesCalled { get; internal set; }

        public bool WillAffect()
        {
            if (ActualTimesCalled <= 0)
            {
                return false;
            }

            switch (_operation)
            {
                case TimesCalledOperation.After:
                    return ActualTimesCalled > _trigger;
                case TimesCalledOperation.Until:
                    return ActualTimesCalled < _trigger;
                case TimesCalledOperation.At:
                    return ActualTimesCalled == _trigger;
                case TimesCalledOperation.EveryXCalls:
                    return ActualTimesCalled % _trigger == 0;
            }

            return false;
        }
    }
}