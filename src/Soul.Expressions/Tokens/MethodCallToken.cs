namespace Soul.Expressions.Tokens
{
	public class StaticMethodCallToken : SyntaxToken
	{
		public string Type { get; }
		public string Method { get; }
		public string[] Arguments { get; }

		public StaticMethodCallToken(string method, string[] arguments)
		{
			Raw = $"{method}({string.Join(",", arguments)})";
			Method = method;
			Arguments = arguments;
		}

	}
}
