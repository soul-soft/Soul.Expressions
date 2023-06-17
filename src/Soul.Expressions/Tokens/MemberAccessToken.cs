namespace Soul.Expressions.Tokens
{
	public class MemberAccessToken : SyntaxToken
	{
		public string Expression { get; }
		public string Member { get; }

		public MemberAccessToken(string expression, string member)
		{
			Raw = $"{expression}.{member}";
			Expression = expression;
			Member = member;
		}
	}
}
