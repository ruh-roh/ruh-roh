namespace RuhRoh.ArgumentMatchers
{
	internal class WithExpressionMatcher : IArgumentMatcher
	{
		private readonly WithMatcher _matcher;

		public WithExpressionMatcher(WithMatcher matcher)
		{
			_matcher = matcher;
		}

		public bool Matches(object value)
		{
			return _matcher.Matches(value);
		}
	}
}