using System;
using System.Linq.Expressions;

namespace RuhRoh.ArgumentMatchers
{
    internal static class ArgumentMatcher
    {
        public static IArgumentMatcher Create(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Call:
                    var call = (MethodCallExpression) expression;
                    
					// Check if the MethodCallExpression is a With-expression
	                using (var context = new MatchingContext())
	                {
		                Expression.Lambda<Action>(call).Compile()();

		                if (context.LastMatcher != null)
		                {
							return new WithExpressionMatcher(context.LastMatcher);
		                }
	                }

	                break;

				case ExpressionType.Constant:
					return new ConstantMatcher(((ConstantExpression) expression).Value);
            }

            // TODO Move to resx
            throw new NotSupportedException($"Unsupported expression: {expression}");
        }
    }
}