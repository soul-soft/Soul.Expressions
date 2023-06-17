using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Soul.Expressions
{
	internal class SyntaxCompilerContext
	{
		public SyntaxTree SyntaxTree { get; }

		public Dictionary<string, Expression> Expressions { get; } = new Dictionary<string, Expression>();

		public SyntaxCompilerContext(SyntaxTree syntaxTree)
		{
			SyntaxTree = syntaxTree;
		}

		public void AddExpression(string key, Expression expression)
		{
			Expressions.Add(key, expression);
		}

		public Expression GetExpression(string key)
		{
			return Expressions[key];
		}

		public IEnumerable<Expression> GetExpressions(string[] names)
		{
			foreach (var item in names)
			{
				yield return Expressions[item];
			}
		}

		public Expression GetBody()
		{
			var key = SyntaxTree.Tokens.Keys.Last();
			return Expressions[key];
		}

		public ParameterExpression[] GetParameters()
		{
			var key = SyntaxTree.Tokens.Keys.Last();
			return Expressions.Values
				.Where(a => a is ParameterExpression)
				.Cast<ParameterExpression>()
				.ToArray();
		}
	}
}
