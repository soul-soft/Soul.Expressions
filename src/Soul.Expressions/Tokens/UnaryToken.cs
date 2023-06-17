namespace Soul.Expressions.Tokens
{
	public class UnaryToken : SyntaxToken
	{
		public string Operand { get; }
		public string Type { get; }

		public UnaryToken(string operand, string type)
		{
			Operand = operand;
			Type = type;
			if (type == "!")
			{
				Raw = $"!{operand}";
			}
		}
	}
}
