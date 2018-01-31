using System;
using System.Threading.Tasks;

namespace RuhRoh.Core.Affectors
{
    public class Delayer : IAffector
    {
        private readonly TimeSpan _delay;

        public Delayer(TimeSpan delay)
        {
            _delay = delay;
        }

        public void Affect()
        {
            Task.Delay(_delay).GetAwaiter().GetResult();
        }
    }
}
