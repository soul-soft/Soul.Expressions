using System;
using System.Collections.Generic;
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
	}
}
