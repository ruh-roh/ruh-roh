using System;

namespace RuhRoh.ArgumentMatchers
{
	internal class WithExpressionMatcher : IArgumentMatcher, IEquatable<WithExpressionMatcher>
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

	    public bool Equals(WithExpressionMatcher other)
	    {
	        if (ReferenceEquals(null, other)) return false;
	        if (ReferenceEquals(this, other)) return true;
	        return Equals(_matcher, other._matcher);
	    }

	    public override bool Equals(object obj)
	    {
	        if (ReferenceEquals(null, obj)) return false;
	        if (ReferenceEquals(this, obj)) return true;
	        if (obj.GetType() != GetType()) return false;
	        return Equals((WithExpressionMatcher) obj);
	    }

	    public override int GetHashCode()
	    {
	        return (_matcher != null ? _matcher.GetHashCode() : 0);
	    }
	}
}