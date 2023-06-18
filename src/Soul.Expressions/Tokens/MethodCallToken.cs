namespace Soul.Expressions.Tokens
{
	public class MethodCallToken : SyntaxToken
	{
		public string Instance { get; }
		public string Method { get; }
		public string[] Arguments { get; }

		public MethodCallToken(string method, string[] arguments)
		{
			Raw = $"{method}({string.Join(",", arguments)})";
			Method = method;
			Arguments = arguments;
		}

		public MethodCallToken(string instance, string method, string[] arguments)
		{
			Instance = instance;
			Raw = $"{instance}.{method}({string.Join(",", arguments)})";
			Method = method;
			Arguments = arguments;
		}
	}
}
