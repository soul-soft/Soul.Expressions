using System.Collections.Generic;
using System.Text;
using System.Linq;
using Soul.Expressions.Tokens;

namespace Soul.Expressions
{
	public class SyntaxTree
	{
		public string Text { get; }

		private HashSet<SyntaxParameter> _parameters = new HashSet<SyntaxParameter>();

		private Dictionary<string, SyntaxToken> _tokens = new Dictionary<string, SyntaxToken>();

		internal SyntaxTree(string text, SyntaxParameter[] parameters)
		{
			Text = text;
			_parameters = new HashSet<SyntaxParameter>(parameters);
		}

		public bool ContainsKey(string key)
		{
			return _tokens.ContainsKey(key);
		}

		public bool ContainsParameter(string name)
		{
			return _parameters.Any(a => a.Name == name);
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
