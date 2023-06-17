namespace Soul.Expressions.Tokens
{
	public abstract class SyntaxToken
	{
		public string Text { get; protected set; }
		public virtual SyntaxTokenType TokenType { get; }
	}
}
