namespace Soul.Expressions.Tokens
{
	public class UnaryToken : SyntaxToken
	{
		public string Expr { get; }
		public string Operator { get; }
		public override TokenType TokenType => TokenType.Unary;

		public UnaryToken(string expr, string @operator)
		{
			Expr = expr;
			Operator = @operator;
			if (@operator == "!")
			{
				Debug = $"!{expr}";
			}
		}
	}
}
