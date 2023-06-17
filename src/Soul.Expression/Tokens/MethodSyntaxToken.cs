using static System.Net.Mime.MediaTypeNames;

namespace Soul.Expression.Tokens
{
	public class MethodSyntaxToken : SyntaxToken
	{
		public string Name { get; }
		public string[] Args { get; }
		public string Expr { get; }

		public override SyntaxTokenType TokenType => SyntaxTokenType.Method;

		public MethodSyntaxToken(string text, string name, string[] args, string expr)
		{
			Text = text;
			Name = name;
			Args = args;
			Expr = expr;
		}
	}
}
