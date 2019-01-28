using System;

namespace RuhRoh.ArgumentMatchers
{
	internal interface IArgumentMatcher
	{
		bool Matches(object value);
	}
}