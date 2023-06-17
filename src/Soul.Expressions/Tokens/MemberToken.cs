namespace Soul.Expressions.Tokens
{
	public class MemberToken : SyntaxToken
	{
		public string Expression { get; }
		public string Member { get; }

		public MemberToken(string expression, string member)
		{
			Raw = $"{expression}.{member}";
			Expression = expression;
			Member = member;
		}
	}
}
