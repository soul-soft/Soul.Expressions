namespace Soul.Expressions.Tokens
{
	public class MethodCallToken : SyntaxToken
	{
		public string Type { get; }
		public string Name { get; }
		public string[] Args { get; }

		public MethodCallToken(string type, string name, string[] args)
		{
			Type = type;
			Name = name;
			Args = args;
			if (string.IsNullOrEmpty(type))
			{
				Raw = $"{name}({string.Join(",", args)})";
			}
			else
			{
				Raw = $"{type}{name}({string.Join(",", args)})";
			}
		}

	}
}
