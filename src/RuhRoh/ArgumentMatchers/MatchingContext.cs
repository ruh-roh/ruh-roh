using System;

namespace RuhRoh.ArgumentMatchers
{
    internal class MatchingContext : IDisposable
    {
        // Every threads needs its own version of _current
        [ThreadStatic]
        private static MatchingContext _current;

        public MatchingContext()
        {
            _current = this;
        }

        public static MatchingContext Current => _current;
        public static bool IsActive => _current != null;

        public WithMatcher LastMatcher { get; set; }

        public void Dispose()
        {
            _current=null;
        }
    }
}