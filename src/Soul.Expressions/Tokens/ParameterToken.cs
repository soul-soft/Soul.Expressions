using System;

namespace Soul.Expressions.Tokens
{
	public class ParameterToken : SyntaxToken
	{
		public string Name { get; }
		public object Value { get; }
		public Type Type { get; }
		public override TokenType TokenType => TokenType.Parameter;

		public ParameterToken(string name, object value, Type type)
		{
			Debug = name;
			Name = name;
			Value = value;
			Type = type;
		}
	}
}
