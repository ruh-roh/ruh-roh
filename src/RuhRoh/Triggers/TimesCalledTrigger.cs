using RuhRoh.Triggers.Internal;

namespace RuhRoh.Triggers
{
    internal class TimesCalledTrigger : IUpdateableTrigger
    {
        private readonly TimesCalledOperation _operation;
        private readonly int _trigger;
        private bool _stopUpdating = false;

        internal TimesCalledTrigger(TimesCalledOperation operation, int trigger)
        {
            _operation = operation;
            _trigger = trigger;
        }

        public int ActualTimesCalled { get; internal set; }

        bool ITrigger.WillAffect()
        {
            if (ActualTimesCalled <= 0)
            {
                return false;
            }

            switch (_operation)
            {
                case TimesCalledOperation.After:
                    if (!_stopUpdating && ActualTimesCalled > _trigger)
                    {
                        _stopUpdating = true;
                    }
                    return _stopUpdating;
                case TimesCalledOperation.Until:
                    if (!_stopUpdating && ActualTimesCalled >= _trigger)
                    {
                        _stopUpdating = true;
                    }
                    return !_stopUpdating;
                case TimesCalledOperation.At:
                    if (!_stopUpdating && ActualTimesCalled > _trigger)
                    {
                        _stopUpdating = true;
                    }
                    return ActualTimesCalled == _trigger;
                case TimesCalledOperation.EveryXCalls:
                    if (ActualTimesCalled % _trigger == 0)
                    {
                        ActualTimesCalled = 0;
                        return true;
                    }
                    return false;
            }

            return false;
        }

        void IUpdateableTrigger.Update()
        {
            if (_stopUpdating)
            {
                return;
            }

            ActualTimesCalled++;
        }
    }
}