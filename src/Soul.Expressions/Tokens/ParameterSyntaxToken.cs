using System;

namespace Soul.Expressions.Tokens
{
	public class ParameterSyntaxToken : SyntaxToken
	{
		public string Name { get; }
		public object Value { get; }
		public Type Type { get; }
		public override SyntaxTokenType TokenType => SyntaxTokenType.Parameter;

		public ParameterSyntaxToken(string name, object value, Type type)
		{
			Debug = name;
			Name = name;
			Value = value;
			Type = type;
		}
	}
}
