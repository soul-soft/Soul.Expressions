using System.Collections.Generic;
using System.Text;
using Soul.Expression.Tokens;

namespace Soul.Expression
{
	public class SyntaxTree
	{
		public string Text { get; }
		
		public Dictionary<string, SyntaxToken> _tokens = new Dictionary<string, SyntaxToken>();

		public SyntaxTree(string text)
		{
			Text = text;
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
