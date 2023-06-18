using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Soul.Expressions
{
	public class SyntaxContext
	{
		public string Expression { get; }

		private Dictionary<string, Expression> _tokens = new Dictionary<string, Expression>();

		private List<ParameterExpression> _parameters = new List<ParameterExpression>();

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

		internal string AddToken(Expression expression)
		{
			var key = "${" + _tokens.Count + "}";
			_tokens.Add(key, expression);
			return key;
		}

		internal bool TryGetToken(string key, out Expression expression)
		{
			return _tokens.TryGetValue(key, out expression);
		}

		public string Debug
		{
			get
			{
				var list = new List<SyntaxToken>();
				foreach (var item in _tokens)
				{
					var key = item.Key;
					var token = item.Value;
					list.Add(new SyntaxToken($"{key} = {token}", token.NodeType));
				}
				var width = list.Max(s => s.Text.Length);
				return list.Select(s => $"{s.Text.PadRight(width)} | {s.Type}").Aggregate((x, y) => $"{x}\r\n{y}");
			}
		}

		class SyntaxToken
		{
			public string Text { get; }
			public ExpressionType Type { get; }

			public SyntaxToken(string text, ExpressionType type)
			{
				Text = text;
				Type = type;
			}
		}
	}
}
