using System;

namespace Soul.Expressions.Tokens
{
	public class ConstantToken : SyntaxToken
	{
		public string Value { get; }
		public Type Type { get; }
		public override TokenType TokenType => TokenType.Constant;

		public ConstantToken(string value, Type type)
		{
			Debug = value;
			Value = value;
			Type = type;
		}
	}
}
