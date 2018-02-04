using System;
using System.Threading.Tasks;
using RuhRoh.Affectors.Internal;

namespace RuhRoh.Affectors
{
    internal class Delayer : AffectorBase
    {
        private readonly TimeSpan _delay;

        public Delayer(TimeSpan delay)
        {
            _delay = delay;
        }

        public sealed override void Affect()
        {
            Task.Delay(_delay).GetAwaiter().GetResult();
        }
    }
}
