namespace Soul.Expressions.Tokens
{
	public class MethodCallToken : SyntaxToken
	{
		public string Type { get; }
		public string Method { get; }
		public string[] Arguments { get; }

		public MethodCallToken(string type, string method, string[] arguments)
		{
			Type = type;
			Method = method;
			Arguments = arguments;
			if (string.IsNullOrEmpty(type))
			{
				Raw = $"{method}({string.Join(",", arguments)})";
			}
			else
			{
				Raw = $"{type}{method}({string.Join(",", arguments)})";
			}
		}

	}
}
