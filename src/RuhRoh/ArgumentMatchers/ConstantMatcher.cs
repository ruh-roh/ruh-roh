namespace RuhRoh.ArgumentMatchers
{
	internal class ConstantMatcher : IArgumentMatcher
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
	}
}