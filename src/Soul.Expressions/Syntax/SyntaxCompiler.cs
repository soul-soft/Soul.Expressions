using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Soul.Expressions.Tokens;

namespace Soul.Expressions
{
	public static class SyntaxCompiler
	{
		public static Dictionary<string, MethodInfo> Methods = new Dictionary<string, MethodInfo>();

		public static void RegisterMethod(string name, MethodInfo method)
		{
			Methods.Add(name, method);
		}

		public static void RegisterMethod(string name, Delegate method)
		{
			Methods.Add(name, method.Method);
		}

		public static LambdaExpression Lambda(string expr, params SyntaxParameter[] parameters)
		{
			var tree = SyntaxEngine.Run(expr, parameters);
			return Lambda(tree);
		}

		public static LambdaExpression Lambda(SyntaxTree tree)
		{
			var context = new SyntaxCompilerContext(tree);
			foreach (var item in tree.Tokens)
			{
				context.AddExpression(item.Key, Watch(item.Value, context));
			}
			return null;
		}

		private static Expression Watch(SyntaxToken token, SyntaxCompilerContext context)
		{
			return null;
		}

	}
}
