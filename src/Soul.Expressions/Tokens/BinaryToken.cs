namespace Soul.Expressions.Tokens
{
	public class BinaryToken : SyntaxToken
	{
		public string Left { get; }
		public string Operator { get; }
		public string Right { get; }
		public override TokenType TokenType => TokenType.Binary;

		public BinaryToken(string left, string @operator, string right)
		{
			Debug = $"{left} {@operator} {right}";
			Left = left;
			Operator = @operator;
			Right = right;
		}
	}
}
