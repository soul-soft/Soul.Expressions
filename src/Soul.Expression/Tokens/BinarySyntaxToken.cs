namespace Soul.Expression.Tokens
{
	public class BinarySyntaxToken : SyntaxToken
	{
		public string Left { get; }
		public string Option { get; }
		public string Right { get; }
		public override SyntaxTokenType TokenType => SyntaxTokenType.Binary;

		public BinarySyntaxToken(string left, string option, string right)
		{
			Left = left;
			Option = option;
			Right = right;
		}
	}
}
