namespace Soul.Expressions.Tokens
{
	public abstract class SyntaxToken
	{
		public string Debug { get; protected set; }
		
		public virtual TokenType TokenType { get; }
	
	}
}
