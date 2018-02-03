using System;
using System.Threading.Tasks;
using RuhRoh.Core.Affectors.Internal;

namespace RuhRoh.Core.Affectors
{
    public class Delayer : AffectorBase
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
