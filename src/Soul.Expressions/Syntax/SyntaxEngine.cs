using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Soul.Expressions.Tokens;

namespace Soul.Expressions
{
	/// <summary>
	/// 语法分析引擎 
	/// </summary>
	public static class SyntaxEngine
	{
		/// <summary>
		/// 运行
		/// </summary>
		/// <param name="expr"></param>
		/// <returns></returns>
		public static SyntaxTree Run(string expr, params SyntaxParameter[] parameters)
		{
			var tree = new SyntaxTree(expr, parameters);
			return Watch(tree);
		}

		/// <summary>
		/// 观察
		/// </summary>
		/// <param name="tree"></param>
		/// <returns></returns>
		private static SyntaxTree Watch(SyntaxTree tree)
		{
			Watch(tree.Text, tree);
			return tree;
		}

		/// <summary>
		/// 递归观察
		/// </summary>
		/// <param name="expr"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		private static string Watch(string expr, SyntaxTree tree)
		{
			if (tree.ContainsToken(expr))
			{
				//处理换元
				return expr;
			}
			if (tree.ContainsParameter(expr))
			{
				//处理参数
				var parameter = tree.GetParameter(expr);
				return tree.AddToken(new ParameterSyntaxToken(parameter.Name, parameter.Value, parameter.Type));
			}
			if (SyntaxUtility.TryConstantSyntaxToken(expr, out ConstantSyntaxToken constantSyntaxToken))
			{
				//处理常量
				return tree.AddToken(constantSyntaxToken);
			}
			if (SyntaxUtility.TryMethodCallSyntax(expr, out Match methodCallMatch))
			{
				//处理函数
				var type = methodCallMatch.Groups["type"].Value;
				var name = methodCallMatch.Groups["name"].Value;
				var argsExpr = methodCallMatch.Groups["args"].Value;
				var value = methodCallMatch.Value;
				var args = SyntaxUtility.SplitArguments(argsExpr);
				var parameters = new List<string>();
				foreach (var item in args)
				{
					var argKey = Watch(item, tree);
					parameters.Add(argKey);
				}
				var token = new MethodCallSyntaxToken(type, name, parameters.ToArray());
				var key = tree.AddToken(token);
				var newExpr = expr.Replace(value, key);
				return Watch(newExpr, tree);
			}
			if (SyntaxUtility.TryIncludeSyntax(expr, out Match includeMatch))
			{
				//处理括号
				var expr1 = includeMatch.Groups["expr"].Value;
				var value = includeMatch.Value;
				var key = Watch(expr1, tree);
				var newExpr = expr.Replace(value, key);
				return Watch(newExpr, tree);
			}
			if (SyntaxUtility.TryUnarySyntax(expr, out Match unaryMatch))
			{
				//处理逻辑非
				var expr1 = unaryMatch.Groups["expr1"].Value;
				var value = unaryMatch.Value;
				var key1 = Watch(expr1, tree);
				var key = tree.AddToken(new UnaryNotSyntaxToken(key1, "!"));
				var newExpr = expr.Replace(value, key);
				return Watch(newExpr, tree);
			}
			if (SyntaxUtility.TryBinarySyntax(expr, out Match binaryMatch))
			{
				//处理二元运算
				var expr1 = binaryMatch.Groups["expr1"].Value;
				var expr2 = binaryMatch.Groups["expr2"].Value;
				var expr3 = binaryMatch.Groups["expr3"].Value;
				var value = binaryMatch.Value;
				var key1 = Watch(expr1, tree);
				var key3 = Watch(expr3, tree);
				var key = tree.AddToken(new BinarySyntaxToken(key1, expr2, key3));
				var newExpr = expr.Replace(value, key);
				return Watch(newExpr, tree);
			}
			var message = string.Format("Unrecognized syntax token：“{0}”", expr);
			throw new NotImplementedException(message);
		}

	}
}
