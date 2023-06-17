namespace Soul.Expressions.Tokens
{
	public class BinaryToken : SyntaxToken
	{
		public string Left { get; }
		public string BinaryType { get; }
		public string Right { get; }

		public BinaryToken(string left, string binaryType, string right)
		{
			Raw = $"{left} {binaryType} {right}";
			Left = left;
			BinaryType = binaryType;
			Right = right;
		}
	}
}
