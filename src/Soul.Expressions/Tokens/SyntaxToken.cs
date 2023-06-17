namespace Soul.Expressions.Tokens
{
	public abstract class SyntaxToken
	{
		public string Debug { get; protected set; }
		public virtual SyntaxTokenType TokenType { get; }

		public bool IsMethodSyntax()
		{
			return TokenType == SyntaxTokenType.MethodCall;
		}

		public bool IsBinarySyntax()
		{
			return TokenType == SyntaxTokenType.Binary;
		}

		public bool IsUnarySyntax()
		{
			return TokenType == SyntaxTokenType.Unary;
		}

		public BinarySyntaxToken CastBinarySyntaxToken()
		{
			return (BinarySyntaxToken)this;
		}

		public MethodCallSyntaxToken CastMethodCallSyntaxToken()
		{
			return (MethodCallSyntaxToken)this;
		}

		public UnaryNotSyntaxToken CastUnarySyntaxToken()
		{
			return (UnaryNotSyntaxToken)this;
		}
	}
}
