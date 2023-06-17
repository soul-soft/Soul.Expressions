using static System.Net.Mime.MediaTypeNames;

namespace Soul.Expressions.Tokens
{
	public class MethodSyntaxToken : SyntaxToken
	{
		public string Type { get; }
		public string Name { get; }
		public string[] Args { get; }
		public string Expr { get; }

		public override SyntaxTokenType TokenType => SyntaxTokenType.Method;

		public MethodSyntaxToken(string text, string type, string name, string[] args, string expr)
		{
			Text = text;
			Type = type;
			Name = name;
			Args = args;
			Expr = expr;
		}
	}
}
