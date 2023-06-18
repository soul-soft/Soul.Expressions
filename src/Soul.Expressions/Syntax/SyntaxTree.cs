using System.Collections.Generic;
using System.Text;
using System.Linq;
using Soul.Expressions.Tokens;

namespace Soul.Expressions
{
	public class SyntaxTree
	{
		public string Text { get; }

		private HashSet<Parameter> _parameters = new HashSet<Parameter>();

		private Dictionary<string, SyntaxToken> _tokens = new Dictionary<string, SyntaxToken>();

		public IReadOnlyDictionary<string,SyntaxToken> Tokens => _tokens;

		internal SyntaxTree(string text, Parameter[] parameters)
		{
			Text = text;
			_parameters = new HashSet<Parameter>(parameters);
		}

		public bool ContainsToken(string key)
		{
			return _tokens.ContainsKey(key);
		}

		public bool ContainsParameter(string name)
		{
			return _parameters.Any(a => a.Name == name);
		}

		public Parameter GetParameter(string name)
		{
			return _parameters.Where(a => a.Name == name).FirstOrDefault();
		}

		public string Debug
		{
			get
			{
				var sb = new StringBuilder();
				foreach (var token in _tokens)
				{
					sb.AppendLine($"{token.Key} = {token.Value.Raw}");
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
