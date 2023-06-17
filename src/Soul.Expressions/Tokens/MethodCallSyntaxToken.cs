namespace Soul.Expressions.Tokens
{
	public class MethodCallSyntaxToken : SyntaxToken
	{
		public string Type { get; }
		public string Name { get; }
		public string[] Args { get; }

		public override SyntaxTokenType TokenType => SyntaxTokenType.MethodCall;

		public MethodCallSyntaxToken(string type, string name, string[] args)
		{
			Type = type;
			Name = name;
			Args = args;
			if (string.IsNullOrEmpty(type))
			{
				Debug = $"{name}({string.Join(",", args)})";
			}
			else
			{
				Debug = $"{type}{name}({string.Join(",", args)})";
			}
		}

	}
}
