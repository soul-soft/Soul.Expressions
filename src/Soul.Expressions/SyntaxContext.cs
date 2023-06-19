using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Soul.Expressions
{
	public class SyntaxContext
	{
		public string Expression { get; }

		private readonly Dictionary<string, SyntaxToken> _tokens = new Dictionary<string, SyntaxToken>();

		private readonly List<ParameterExpression> _parameters = new List<ParameterExpression>();

		public IEnumerable<ParameterExpression> Parameters => _parameters;

		public SyntaxContext(string expression)
		{
			Expression = expression;
		}

		public SyntaxContext(string expression, params Parameter[] parameters)
			: this(expression)
		{
			foreach (var item in parameters)
			{
				AddParameter(item.Name, item.Type);
			}
		}

		public void AddParameter(string name, Type type)
		{
			_parameters.Add(System.Linq.Expressions.Expression.Parameter(type, name));
		}

		public bool TryGetParameter(string token, out ParameterExpression parameterExpression)
		{
			parameterExpression = _parameters.Where(a => a.Name == token).FirstOrDefault();
			return parameterExpression != null;
		}

		internal string AddToken(string token,Expression expression)
		{
			var key = "${" + _tokens.Count + "}";
			_tokens.Add(key, new SyntaxToken(token,expression));
			return key;
		}

		internal bool TryGetToken(string key, out SyntaxToken token)
		{
			return _tokens.TryGetValue(key, out token);
		}

		public string DebugView
		{
			get
			{
				var tokens = _tokens.Select(s=>new 
				{
					s.Value.Token,
					s.Value.ExpressionType
				});

				var width = tokens.Max(s => s.Token.Length);
				
				return tokens.Select(s => $"{s.Token.PadRight(width)} | {s.ExpressionType}").Aggregate((x, y) => $"{x}\r\n{y}");
			}
		}
	}
}
