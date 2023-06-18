using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Soul.Expressions
{
	public class ExpressionSyntaxContext
	{
		public string Expression { get; }

		private Dictionary<string, Expression> _tokens = new Dictionary<string, Expression>();

		private List<ParameterExpression> _parameters = new List<ParameterExpression>();

		public IEnumerable<ParameterExpression> Parameters => _parameters;

		public ExpressionSyntaxContext(string expression)
		{
			Expression = expression;
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
	}
}
