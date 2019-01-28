using System;

namespace RuhRoh.Triggers
{
    internal class AgendaTrigger : ITrigger
    {
        private readonly DateTimeOffset _moment;
        private readonly TimeSpan _duration;

        public AgendaTrigger(DateTimeOffset moment, TimeSpan duration)
        {
            if (duration <= TimeSpan.Zero)
            {
                // TODO Move to resx
                throw new ArgumentOutOfRangeException(nameof(duration), "Duration should contain a strictly positive amount of time.");
            }

            _moment = moment;
            _duration = duration;
        }

        public bool WillAffect()
        {
            var now = DateTimeOffset.Now;
            return _moment <= now && now <= _moment.Add(_duration);
        }
    }
}
