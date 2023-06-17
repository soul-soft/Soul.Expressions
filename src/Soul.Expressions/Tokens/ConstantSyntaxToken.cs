using System;

namespace Soul.Expressions.Tokens
{
	public class ConstantSyntaxToken : SyntaxToken
	{
		public string Value { get; }
		public Type Type { get; }
		public override SyntaxTokenType TokenType => SyntaxTokenType.Constant;

		public ConstantSyntaxToken(string value, Type type)
		{
			Debug = value;
			Value = value;
			Type = type;
		}
	}
}
