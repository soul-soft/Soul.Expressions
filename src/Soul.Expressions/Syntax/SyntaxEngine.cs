using System;
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
			if (tree.ContainsKey(expr))
			{
				//处理换元
				return expr;
			}
			if (tree.ContainsParameter(expr))
			{
				//处理参数
				return expr;
			}
			if (SyntaxUtility.IsConstantToken(expr))
			{
				//处理常量
				return expr;
			}
			if (MatchObjectMethodSyntax(expr, out Match objectMethodMatch))
			{
				//处理函数
				var type = objectMethodMatch.Groups["type"].Value;
				var name = objectMethodMatch.Groups["name"].Value;
				var args = objectMethodMatch.Groups["args"].Value;
				var value = objectMethodMatch.Value;
				var parameters = SyntaxUtility.MatchParameterTokens(args)
					.Select(arg => Watch(arg, tree)).ToArray();
				var token = new MethodSyntaxToken(value, type, name, parameters, expr);
				var funcKey = tree.AddToken(token);
				return funcKey;
			}
			if (MatchStaticMethodSyntax(expr, out Match globalMethodMatch))
			{
				//处理函数
				var name = globalMethodMatch.Groups["name"].Value;
				var args = globalMethodMatch.Groups["args"].Value;
				var value = globalMethodMatch.Value;
				var parameters = SyntaxUtility.MatchParameterTokens(args);
				var parametersArray = parameters.Select(arg => Watch(arg, tree)).ToArray();
				var token = new MethodSyntaxToken(value, null, name, parametersArray, expr);
				var funcKey = tree.AddToken(token);
				return funcKey;
			}
			if (MatchIncludeSyntax(expr, out Match includeMatch))
			{
				//处理括号
				var expr1 = includeMatch.Groups["expr"].Value;
				var value = includeMatch.Value;
				var key = Watch(expr1, tree);
				var text = expr.Replace(value, key);
				return Watch(text, tree);
			}
			if (MatchUnarySyntax(expr, out Match unaryMatch))
			{
				//处理逻辑非
				var expr1 = unaryMatch.Groups["expr1"].Value;
				var value = unaryMatch.Value;
				var key = tree.AddToken(new UnarySyntaxToken(value, Watch(expr1, tree), "!"));
				var text = expr.Replace(value, key);
				return Watch(text, tree);
			}
			if (MatchBinarySyntax(expr, out Match multiplyMtch, @"\*|/|%"))
			{
				//处理乘法
				var expr1 = multiplyMtch.Groups["expr1"].Value;
				var expr2 = multiplyMtch.Groups["expr2"].Value;
				var expr3 = multiplyMtch.Groups["expr3"].Value;
				var value = multiplyMtch.Value;
				var key = tree.AddToken(new BinarySyntaxToken(value, expr1, expr2, expr3));
				var text = expr.Replace(value, key);
				Watch(text, tree);
				return key;
			}
			if (MatchBinarySyntax(expr, out Match addMatch, @"\+|\-"))
			{
				//处理加减
				var expr1 = addMatch.Groups["expr1"].Value;
				var expr2 = addMatch.Groups["expr2"].Value;
				var expr3 = addMatch.Groups["expr3"].Value;
				var value = addMatch.Value;
				var key = tree.AddToken(new BinarySyntaxToken(value, expr1, expr2, expr3));
				var text = expr.Replace(value, key);
				Watch(text, tree);
				return key;
			}
			if (MatchBinarySyntax(expr, out Match relatMatch, @">|<|>=|<="))
			{
				//处理关系
				var expr1 = relatMatch.Groups["expr1"].Value;
				var expr2 = relatMatch.Groups["expr2"].Value;
				var expr3 = relatMatch.Groups["expr3"].Value;
				var value = relatMatch.Value;
				var key = tree.AddToken(new BinarySyntaxToken(value, expr1, expr2, expr3));
				var text = expr.Replace(value, key);
				Watch(text, tree);
				return key;
			}
			else if (MatchBinarySyntax(expr, out Match equalsMatch, @"==|!="))
			{
				//处理关系
				var expr1 = equalsMatch.Groups["expr1"].Value;
				var expr2 = equalsMatch.Groups["expr2"].Value;
				var expr3 = equalsMatch.Groups["expr3"].Value;
				var value = equalsMatch.Value;
				var key = tree.AddToken(new BinarySyntaxToken(value, expr1, expr2, expr3));
				var text = expr.Replace(value, key);
				Watch(text, tree);
				return key;
			}
			else if (MatchBinarySyntax(expr, out Match andMatch, "&&"))
			{
				//处理逻辑与
				var expr1 = andMatch.Groups["expr1"].Value;
				var expr2 = andMatch.Groups["expr2"].Value;
				var expr3 = andMatch.Groups["expr3"].Value;
				var value = andMatch.Value;
				var key = tree.AddToken(new BinarySyntaxToken(value, expr1, expr2, expr3));
				var text = expr.Replace(value, key);
				Watch(text, tree);
				return key;
			}
			if (MatchBinarySyntax(expr, out Match orMatch, @"\|\|"))
			{
				//处理逻辑或
				var expr1 = orMatch.Groups["expr1"].Value;
				var expr2 = orMatch.Groups["expr2"].Value;
				var expr3 = orMatch.Groups["expr3"].Value;
				var value = orMatch.Value;
				var key = tree.AddToken(new BinarySyntaxToken(value, expr1, expr2, expr3));
				var text = expr.Replace(value, key);
				Watch(text, tree);
				return key;
			}
			var message = string.Format("Unrecognized syntax token：“{0}”", expr);
			throw new NotImplementedException(message);
		}

		/// <summary>
		/// 处理括号运算
		/// </summary>
		/// <param name="expr"></param>
		/// <param name="match"></param>
		/// <returns></returns>
		private static bool MatchIncludeSyntax(string expr, out Match match)
		{
			match = Regex.Match(expr, @"\((?<expr>.+)\)");
			return match.Success;
		}

		/// <summary>
		/// 处理逻辑非
		/// </summary>
		/// <param name="expr"></param>
		/// <param name="match"></param>
		/// <returns></returns>
		private static bool MatchUnarySyntax(string expr, out Match match)
		{
			match = Regex.Match(expr, @"\!(?<expr1>\w+|\w+\.\w+|#\{\d+\})");
			return match.Success;
		}

		/// <summary>
		/// 二元运算
		/// </summary>
		/// <param name="expr"></param>
		/// <param name="math"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private static bool MatchBinarySyntax(string expr, out Match math, string args)
		{
			var pattern = $@"(?<expr1>(\w+|\w+\.\w+|#\{{\d+\}}))\s*(?<expr2>({args}))\s*(?<expr3>(\w+|\w+\.\w+|#\{{\d+\}}))";
			math = Regex.Match(expr, pattern);
			return math.Success;
		}

		/// <summary>
		/// 是否全局函数
		/// </summary>
		/// <param name="expr"></param>
		/// <param name="match"></param>
		/// <returns></returns>
		private static bool MatchStaticMethodSyntax(string expr, out Match match)
		{
			match = Regex.Match(expr, @"(?<name>\w+)\((?<args>[^\(|\)]+)\)");
			return match.Success;
		}

		/// <summary>
		/// 匹配成员函数
		/// </summary>
		/// <param name="expr"></param>
		/// <param name="match"></param>
		/// <returns></returns>
		private static bool MatchObjectMethodSyntax(string expr, out Match match)
		{
			match = Regex.Match(expr, @"(?<type>\w+)\.(?<name>\w+)\((?<args>[^\(|\)]+)\)");
			return match.Success;
		}
	}
}
