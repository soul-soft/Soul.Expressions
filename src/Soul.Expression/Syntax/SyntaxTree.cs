using System.Collections.Generic;
using Soul.Expression.Tokens;

namespace Soul.Expression
{
	public class SyntaxTree
	{
		public Dictionary<string, SyntaxToken> _tokens = new Dictionary<string, SyntaxToken>();

		public SyntaxTree()
		{
		}

		internal string AddToken(SyntaxToken token)
		{
			var key = "#{" + _tokens.Count + "}";
			_tokens.Add(key, token);
			return key;
		}

		internal bool ContainsKey(string key)
		{
			return _tokens.ContainsKey(key);
		}
	}
}
