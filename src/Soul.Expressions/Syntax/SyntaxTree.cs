using System.Collections.Generic;
using System.Text;
using Soul.Expressions.Tokens;

namespace Soul.Expressions
{
	public class SyntaxTree
	{
		public string Text { get; }

		public SyntaxContext Context { get; }

		public Dictionary<string, SyntaxToken> _tokens = new Dictionary<string, SyntaxToken>();

		internal SyntaxTree(string text, SyntaxContext context)
		{
			Text = text;
			Context = context;
		}

		public bool ContainsKey(string key)
		{
			return _tokens.ContainsKey(key);
		}

		public string Raw
		{
			get
			{
				var sb = new StringBuilder();
				foreach (var token in _tokens)
				{
					sb.AppendLine($"{token.Key} = {token.Value.Text}");
				}
				return sb.ToString();
			}
		}

		internal string AddToken(SyntaxToken token)
		{
			var key = "#{" + _tokens.Count + "}";
			_tokens.Add(key, token);
			return key;
		}
	}
}
