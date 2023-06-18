using System;
using System.Collections.Generic;
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
		public static SyntaxTree Run(string expr, params Parameter[] parameters)
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
		/// <param name="tree"></param>
		/// <returns></returns>
		private static string Watch(string expr, SyntaxTree tree)
		{
			//处理换元
			if (tree.ContainsToken(expr))
			{
				return expr;
			}
			//处理参数
			if (tree.ContainsParameter(expr))
			{
				var value = expr;
				var parameter = tree.GetParameter(expr);
				var token = new ParameterToken(parameter.Name, value, parameter.Type);
				return tree.AddToken(token);
			}
			//处理常量
			if (SyntaxUtility.TryConstantToken(expr, out ConstantTokenValueType valueType))
			{
				var token = new ConstantToken(expr, valueType);
				return tree.AddToken(token);
			}
			//处理成员访问
			if (SyntaxUtility.TryMemberAccessToken(expr, out Match memberAccessMatch))
			{
				var expr1 = memberAccessMatch.Groups["expr1"].Value;
				var expr2 = memberAccessMatch.Groups["expr2"].Value;
				var value = memberAccessMatch.Value;
				var key1 = Watch(expr1, tree);
				var token = new MemberToken(key1, expr2);
				var key = tree.AddToken(token);
				var newExpr = expr.Replace(value, key);
				return Watch(newExpr, tree);
			}
			//处理实列函数
			if (SyntaxUtility.TryInstanceMethodCallToken(expr, out Match instanceMethodCallMatch))
			{
				var instance = instanceMethodCallMatch.Groups["instance"].Value;
				var name = instanceMethodCallMatch.Groups["name"].Value;
				var argsExpr = instanceMethodCallMatch.Groups["args"].Value;
				var value = instanceMethodCallMatch.Value;
				var args = SyntaxUtility.SplitTokens(argsExpr);
				var parameters = new List<string>();
				foreach (var item in args)
				{
					var argKey = Watch(item, tree);
					parameters.Add(argKey);
				}
				var instanceKey = Watch(instance,tree);
				var token = new MethodCallToken(instanceKey, name, parameters.ToArray());
				var key = tree.AddToken(token);
				var newExpr = expr.Replace(value, key);
				return Watch(newExpr, tree);
			}
			//处理静态函数
			if (SyntaxUtility.TryStaticMethodCallToken(expr, out Match staticMethodCallMatch))
			{
				var name = staticMethodCallMatch.Groups["name"].Value;
				var argsExpr = staticMethodCallMatch.Groups["args"].Value;
				var value = staticMethodCallMatch.Value;
				var args = SyntaxUtility.SplitTokens(argsExpr);
				var parameters = new List<string>();
				foreach (var item in args)
				{
					var argKey = Watch(item, tree);
					parameters.Add(argKey);
				}
				var token = new MethodCallToken(name, parameters.ToArray());
				var key = tree.AddToken(token);
				var newExpr = expr.Replace(value, key);
				return Watch(newExpr, tree);
			}
			//处理括号
			if (SyntaxUtility.TryIncludeToken(expr, out Match includeMatch))
			{
				var expr1 = includeMatch.Groups["expr"].Value;
				var value = includeMatch.Value;
				var key = Watch(expr1, tree);
				var newExpr = expr.Replace(value, key);
				return Watch(newExpr, tree);
			}
			//处理逻辑非
			if (SyntaxUtility.TryUnaryToken(expr, out Match unaryMatch))
			{
				var expr1 = unaryMatch.Groups["expr1"].Value;
				var value = unaryMatch.Value;
				var key1 = Watch(expr1, tree);
				var key = tree.AddToken(new UnaryToken(key1, "!"));
				var newExpr = expr.Replace(value, key);
				return Watch(newExpr, tree);
			}
			//处理二元运算
			if (SyntaxUtility.TryBinaryToken(expr, out Match binaryMatch))
			{
				var expr1 = binaryMatch.Groups["expr1"].Value;
				var expr2 = binaryMatch.Groups["expr2"].Value;
				var expr3 = binaryMatch.Groups["expr3"].Value;
				var value = binaryMatch.Value;
				var key1 = Watch(expr1, tree);
				var key3 = Watch(expr3, tree);
				var key = tree.AddToken(new BinaryToken(key1, expr2, key3));
				var newExpr = expr.Replace(value, key);
				return Watch(newExpr, tree);
			}
			var message = string.Format("Unrecognized syntax token：“{0}”", expr);
			throw new NotImplementedException(message);
		}
	}
}
