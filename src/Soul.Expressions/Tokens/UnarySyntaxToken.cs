namespace Soul.Expressions.Tokens
{
	public class UnarySyntaxToken : SyntaxToken
	{
		public string Expr { get; }
		public string Operator { get; }
		public override SyntaxTokenType TokenType => SyntaxTokenType.Unary;

		public UnarySyntaxToken(string text, string expr, string @operator)
		{
			Text = text;
			Expr = expr;
			Operator = @operator;
		}
	}
}
