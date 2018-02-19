using System;
using System.Linq.Expressions;
using System.Reflection;
using RuhRoh.ArgumentMatchers;

namespace RuhRoh
{
    internal abstract class WithMatcher : IEquatable<WithMatcher>
    {
        internal WithMatcher(Expression valueExpression)
        {
            ValueExpression = valueExpression;
        }

	    internal Expression ValueExpression { get; }
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

        public bool Equals(WithMatcher other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(ValueExpression, other.ValueExpression);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((WithMatcher) obj);
        }

        public override int GetHashCode()
        {
            return (ValueExpression != null ? ValueExpression.GetHashCode() : 0);
        }
    }

    internal class WithMatcher<T> : WithMatcher, IEquatable<WithMatcher<T>>
    {
        public WithMatcher(Predicate<T> condition, Expression<Func<T>> valueExpression)
            : base(valueExpression.Body)
        {
            Condition = condition;
        }

        internal Predicate<T> Condition { get; }

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

        public bool Equals(WithMatcher<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Equals(Condition, other.Condition);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((WithMatcher<T>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ (Condition != null ? Condition.GetHashCode() : 0);
            }
        }
    }
}