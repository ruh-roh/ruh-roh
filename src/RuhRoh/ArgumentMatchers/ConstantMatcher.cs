using System;

namespace RuhRoh.ArgumentMatchers
{
	internal class ConstantMatcher : IArgumentMatcher, IEquatable<ConstantMatcher>
	{
	    private readonly object _constantValue;

	    public ConstantMatcher(object constantValue)
	    {
	        _constantValue = constantValue;
	    }

	    public bool Matches(object value)
	    {
	        return Equals(_constantValue, value);
	    }

	    public bool Equals(ConstantMatcher other)
	    {
	        if (ReferenceEquals(null, other)) return false;
	        if (ReferenceEquals(this, other)) return true;
	        return Equals(_constantValue, other._constantValue);
	    }

	    public override bool Equals(object obj)
	    {
	        if (ReferenceEquals(null, obj)) return false;
	        if (ReferenceEquals(this, obj)) return true;
	        if (obj.GetType() != GetType()) return false;
	        return Equals((ConstantMatcher) obj);
	    }

	    public override int GetHashCode()
	    {
	        return (_constantValue != null ? _constantValue.GetHashCode() : 0);
	    }
	}
}