using RuhRoh.ArgumentMatchers;
using Xunit;

namespace RuhRoh.Tests
{
    public class WithTests
    {
        private WithMatcher GetAnyWatcher<T>()
        {
            using (var context = new MatchingContext())
            {
                With.Any<T>();
                return context.LastMatcher;
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void Any_Should_Match_Int32_Values_For_Any_Int32(int value)
        {
            var watcher = GetAnyWatcher<int>();

            Assert.True(watcher.Matches(value));
        }

        [Fact]
        public void Any_Should_Not_Match_Null_For_Any_Int32()
        {
            var watcher = GetAnyWatcher<int>();

            Assert.False(watcher.Matches(null));
        }

        [Theory]
        [InlineData(0L)]
        [InlineData(long.MinValue)]
        [InlineData(long.MaxValue)]
        public void Any_Should_Not_Match_Int64_Values_For_Any_Int32(long value)
        {
            var watcher = GetAnyWatcher<int>();

            Assert.False(watcher.Matches(value));
        }

        [Theory]
        [InlineData(0L)]
        [InlineData(long.MinValue)]
        [InlineData(long.MaxValue)]
        public void Any_Should_Match_Int64_Values_For_Any_Int64(long value)
        {
            var watcher = GetAnyWatcher<long>();

            Assert.True(watcher.Matches(value));
        }

        [Theory]
        [InlineData(0L)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void Any_Should_Not_Match_Int32_Values_For_Any_Int64(int value)
        {
            var watcher = GetAnyWatcher<long>();

            Assert.False(watcher.Matches(value));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void Any_Should_Match_Int32_Values_For_Any_Nullable_Int32(int value)
        {
            var watcher = GetAnyWatcher<int?>();

            Assert.True(watcher.Matches(value));
        }

        [Fact]
        public void Any_Should_Match_Null_Value_For_Any_Nullable_Int32()
        {
            var watcher = GetAnyWatcher<int?>();

            Assert.True(watcher.Matches(null));
        }

        [Theory]
        [InlineData("abc")]
        [InlineData("def")]
        [InlineData("")]
        [InlineData(null)]
        public void Any_Should_Match_String_Values_For_Any_String(string value)
        {
            var watcher = GetAnyWatcher<string>();

            Assert.True(watcher.Matches(value));
        }
    }
}