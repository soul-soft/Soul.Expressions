using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;
using Soul.Expression.Tokens;

namespace Soul.Expression
{
	/// <summary>
	/// 语法分析引擎 
	/// </summary>
	public class SyntaxEngine
	{
		public static SyntaxTree Run(string expr)
		{
			return Watch(expr);
		}

		/// <summary>
		/// 观察
		/// </summary>
		/// <param name="expr"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		private static string Watch(string expr, SyntaxTree tree)
		{
			if (tree.ContainsKey(expr))
			{
				return expr;
			}
			else if (MatchUnarySyntax(expr, out Match match0))
			{
				var text = match0.Groups["expr"].Value;
				return Watch(text, tree);
			}
			else if (MatchBinarySyntax(expr, out Match match1, "\\*|/|%"))
			{
				var expr1 = match1.Groups["expr1"].Value;
				var expr2 = match1.Groups["expr2"].Value;
				var expr3 = match1.Groups["expr3"].Value;
				var key = tree.AddToken(new BinarySyntaxToken(expr1, expr2, expr3));
				var text = expr.Replace(match1.Value, key);
				Watch(text, tree);
				return key;
			}
			else if (MatchBinarySyntax(expr, out Match match2, "\\+|\\-"))
			{
				var expr1 = match2.Groups["expr1"].Value;
				var expr2 = match2.Groups["expr2"].Value;
				var expr3 = match2.Groups["expr3"].Value;
				var key = tree.AddToken(new BinarySyntaxToken(expr1, expr2, expr3));
				var text = expr.Replace(match2.Value, key);
				Watch(text, tree);
				return key;
			}
			else if (MatchBinarySyntax(expr, out Match match3, "&&"))
			{
				var expr1 = match3.Groups["expr1"].Value;
				var expr2 = match3.Groups["expr2"].Value;
				var expr3 = match3.Groups["expr3"].Value;
				var key = tree.AddToken(new BinarySyntaxToken(expr1, expr2, expr3));
				var text = expr.Replace(match3.Value, key);
				Watch(text, tree);
				return key;
			}
			else if (MatchBinarySyntax(expr, out Match match4, "\\|\\|"))
			{
				var expr1 = match4.Groups["expr1"].Value;
				var expr2 = match4.Groups["expr2"].Value;
				var expr3 = match4.Groups["expr3"].Value;
				var key = tree.AddToken(new BinarySyntaxToken(expr1, expr2, expr3));
				var text = expr.Replace(match3.Value, key);
				Watch(text, tree);
				return key;
			}

			else if (MatchMethodSyntax(expr, out Match match5))
			{
				var name = match5.Groups["name"].Value;
				var argExpr = match5.Groups["args"].Value;
				var args = SplitArguments(argExpr)
					.Select(arg => Watch(arg, tree))
					.ToArray();
				var token = new MethodSyntaxToken(name, args, expr);
				var funcKey = tree.AddToken(token);
				return funcKey;
			}
			else
			{
				var message = string.Format("不支持的语法：{0}", expr);
				throw new NotImplementedException(message);
			}
		}

		private static SyntaxTree Watch(string expr)
		{
			var tokens = new SyntaxTree();
			Watch(expr, tokens);
			return tokens;
		}
		/// <summary>
		/// 是否是括号
		/// </summary>
		/// <param name="expr"></param>
		/// <param name="match"></param>
		/// <returns></returns>
		private static bool MatchUnarySyntax(string expr, out Match match)
		{
			match = Regex.Match(expr, @"\((?<expr>.+)\)");
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
			var pattern = $@"(?<expr1>(\w|#\{{\d+\}})+)\s*(?<expr2>({args}))\s*(?<expr3>(\w|#\{{\d+\}})+)";
			math = Regex.Match(expr, pattern);
			return math.Success;
		}

		/// <summary>
		/// 是否函数表达式
		/// </summary>
		/// <param name="expr"></param>
		/// <param name="match"></param>
		/// <returns></returns>
		private static bool MatchMethodSyntax(string expr, out Match match)
		{
			match = Regex.Match(expr, @"(?<name>\w+)\((?<args>[^\(|\)]+)\)");
			return match.Success;
		}




		private static string[] SplitArguments(string input)
		{
			var args = new List<string>();
			var index = 0;
			var quotes = new char[]
			{
				'"', '\''
			};
			var startQuotes = false;
			for (int i = 0; i < input.Length; i++)
			{
				var item = input[i];
				if (quotes.Contains(item))
				{
					if (!startQuotes)
					{
						index = i;
						startQuotes = true;
					}
					else if (i > 0 && input[i - 1] != '\\')
					{
						startQuotes = false;
					}
				}
				if ((!startQuotes && item == ','))
				{
					args.Add(input.Substring(index, i - index));
					index = i + 1;
				}
				if (i == input.Length - 1)
				{
					args.Add(input.Substring(index, i - index + 1));
				}
			}
			return args.ToArray();
		}
	}
}
