using System;
using System.Linq.Expressions;
using System.Reflection;
using RuhRoh.ArgumentMatchers;

namespace RuhRoh
{
    internal abstract class WithMatcher
    {
	    internal Expression ValueExpression { get; set; }
	    public abstract bool Matches(object value);

        public static T Create<T>(Predicate<T> condition, Expression<Func<T>> valueExpression)
        {
            return Create(new WithMatcher<T>(condition, valueExpression));
        }

        public static T Create<T>(WithMatcher<T> matcher)
        {
	        Cache(matcher);
            return default;
        }

	    private static void Cache<T>(WithMatcher<T> matcher)
	    {
		    if (MatchingContext.IsActive)
		    {
			    MatchingContext.Current.LastMatcher = matcher;
		    }
	    }
    }

    internal class WithMatcher<T> : WithMatcher
    {
        public WithMatcher(Predicate<T> condition, Expression<Func<T>> valueExpression)
        {
            Condition = condition;
            ValueExpression = valueExpression.Body;
        }

        internal Predicate<T> Condition { get; set; }

	    public override bool Matches(object value)
	    {
		    if (value != null && !(value is T))
		    {
			    return false;
		    }

		    // If value is null and T is a struct, that should result in false.
		    // Unless T is Nullable<TOther>, then we can check if the condition allows the value.
		    var typeToMatch = typeof(T);
		    if (value == null && typeToMatch.GetTypeInfo().IsValueType
		                      && (!typeToMatch.GetTypeInfo().IsGenericType ||
		                          typeToMatch.GetGenericTypeDefinition() != typeof(Nullable<>)))
		    {
			    return false;
		    }

		    return Condition((T) value);
	    }
    }
}