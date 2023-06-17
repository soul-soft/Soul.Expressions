namespace Soul.Expression.Tokens
{
	public class MethodSyntaxToken : SyntaxToken
	{
		public string Name { get; }
		public string[] Args { get; }
		public string Expr { get; }

		public override SyntaxTokenType TokenType => SyntaxTokenType.Method;

		public MethodSyntaxToken(string name, string[] args, string expr)
		{
			Name = name;
			Args = args;
			Expr = expr;
		}
	}
}
