namespace Soul.Expressions.Tokens
{
	public class UnaryNotSyntaxToken : SyntaxToken
	{
		public string Expr { get; }
		public string Operator { get; }
		public override SyntaxTokenType TokenType => SyntaxTokenType.Unary;

		public UnaryNotSyntaxToken(string expr, string @operator)
		{
			Debug = $"!{expr}";
			Expr = expr;
			Operator = @operator;
		}
	}
}
