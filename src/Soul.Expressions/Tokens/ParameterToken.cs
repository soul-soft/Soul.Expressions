using System;

namespace Soul.Expressions.Tokens
{
	public class ParameterToken : SyntaxToken
	{
		public string Name { get; }
		public object Value { get; }
		public Type Type { get; }

		public ParameterToken(string name, object value, Type type)
		{
			Raw = name;
			Name = name;
			Value = value;
			Type = type;
		}
	}
}
