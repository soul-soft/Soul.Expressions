namespace Soul.Expressions.Tokens
{
	public class BinarySyntaxToken : SyntaxToken
	{
		public string Left { get; }
		public string Operator { get; }
		public string Right { get; }
		public override SyntaxTokenType TokenType => SyntaxTokenType.Binary;

		public BinarySyntaxToken(string left, string @operator, string right)
		{
			Debug = $"{left} {@operator} {right}";
			Left = left;
			Operator = @operator;
			Right = right;
		}
	}
}
