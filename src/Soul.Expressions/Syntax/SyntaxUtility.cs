using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Soul.Expressions.Tokens;

namespace Soul.Expressions
{
	internal static class SyntaxUtility
	{
		/// <summary>
		/// 是否为常量
		/// </summary>
		/// <param name="expr"></param>
		/// <returns></returns>
		public static bool TryConstantToken(string expr, out ConstantTokenValueType valueType)
		{
			if (expr=="null")
			{
				valueType = ConstantTokenValueType.Null;
				return true;
			}
			if (IsIntgerConstantToken(expr))
			{
				valueType = ConstantTokenValueType.Intger;
				return true;
			}
			if (IsBoolConstantToken(expr))
			{
				valueType = ConstantTokenValueType.Boolean;
				return true;
			}
			if (IsDoubleConstantToken(expr))
			{
				valueType = ConstantTokenValueType.Double;
				return true;
			}
			if (IsStringConstantToken(expr))
			{
				valueType = ConstantTokenValueType.String;
				return true;
			}
			if (IsCharConstantToken(expr))
			{
				valueType = ConstantTokenValueType.Char;
				return true;
			}
			valueType = ConstantTokenValueType.None;
			return false;
		}
		
		/// <summary>
		/// 是否为字符串常量
		/// </summary>
		/// <param name="expr"></param>
		/// <returns></returns>
		public static bool IsStringConstantToken(string expr)
		{
			if (expr.Length < 2)
			{
				return false;
			}
			var text = Regex.Replace(expr, @"\\.{1}", "#");
			if (!text.StartsWith("\"") || !text.EndsWith("\""))
			{
				return false;
			}
			if (text.Substring(1, text.Length - 2).Contains('"'))
			{
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// 是否为字符串
		/// </summary>
		/// <param name="expr"></param>
		/// <returns></returns>
		public static bool IsCharConstantToken(string expr)
		{
			if (expr.Length < 3)
			{
				return false;
			}
			var text = Regex.Replace(expr, @"\\.{1}", "#");
			if (!text.StartsWith("'") || !text.EndsWith("'"))
			{
				return false;
			}
			if (text.Trim('\'').Length != 1)
			{
				return false;
			}
			if (text.Substring(1, text.Length - 2).Contains('\''))
			{
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// 是否为整数
		/// </summary>
		/// <param name="expr"></param>
		/// <returns></returns>
		public static bool IsIntgerConstantToken(string expr)
		{
			return Regex.IsMatch(expr, @"^\d+$");
		}
		
		/// <summary>
		/// 是否为浮点数
		/// </summary>
		/// <param name="expr"></param>
		/// <returns></returns>
		public static bool IsDoubleConstantToken(string expr)
		{
			return Regex.IsMatch(expr, @"^\d+\.\d+$");
		}
		
		/// <summary>
		/// 是否为布尔值
		/// </summary>
		/// <param name="expr"></param>
		/// <returns></returns>
		public static bool IsBoolConstantToken(string expr)
		{
			if (expr == "true")
			{
				return true;
			}
			if (expr == "false")
			{
				return true;
			}
			return false;
		}
		
		/// <summary>
		/// 分割函数参数
		/// </summary>
		/// <param name="expr"></param>
		/// <returns></returns>
		public static string[] SplitTokens(string expr)
		{
			var args = new List<string>();
			var index = 0;
			var quotes = new char[]
			{
				'"', '\''
			};
			var startQuotes = false;
			for (int i = 0; i < expr.Length; i++)
			{
				var item = expr[i];
				if (quotes.Contains(item))
				{
					if (!startQuotes)
					{
						index = i;
						startQuotes = true;
					}
					else if (i > 0 && expr[i - 1] != '\\')
					{
						startQuotes = false;
					}
				}
				if ((!startQuotes && item == ','))
				{
					args.Add(expr.Substring(index, i - index));
					index = i + 1;
				}
				if (i == expr.Length - 1)
				{
					args.Add(expr.Substring(index, i - index + 1));
				}
			}
			return args.Select(s => s.Trim()).ToArray();
		}

		/// <summary>
		/// 处理括号运算
		/// </summary>
		/// <param name="expr"></param>
		/// <param name="match"></param>
		/// <returns></returns>
		public static bool TryIncludeToken(string expr, out Match match)
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
		public static bool TryUnaryToken(string expr, out Match match)
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
		public static bool TryBinaryToken(string expr, out Match math)
		{
			var args = new List<string>
			{
				 @"\*|/|%",
				 @"\+|\-",
				 @">|<|>=|<=",
				 @"==|!=",
				 @"&&",
				 @"\|\|"
			};
			foreach (var item in args)
			{
				var pattern = $@"(?<expr1>[^\s|\*|/|%|\+|\-|>|<|=||&|\|]+)\s*(?<expr2>({item}))\s*(?<expr3>[^\s|\*|/|%|\+|\-|>|<|=||&|\|]+)";
				math = Regex.Match(expr, pattern);
				if (math.Success)
				{
					return true;
				}
			}
			math = null;
			return false;
		}

		/// <summary>
		/// 匹配实列函数调用
		/// </summary>
		/// <param name="expr"></param>
		/// <param name="match"></param>
		/// <returns></returns>
		public static bool TryInstanceMethodCallToken(string expr, out Match match)
		{
			match = Regex.Match(expr, @"(?<instance>\w+)\.(?<name>\w+)\((?<args>[^\(|\)]+)\)");
			return match.Success;
		}

		/// <summary>
		/// 匹配实列函数调用
		/// </summary>
		/// <param name="expr"></param>
		/// <param name="match"></param>
		/// <returns></returns>
		public static bool TryStaticMethodCallToken(string expr, out Match match)
		{
			match = Regex.Match(expr, @"(?<name>\w+)\((?<args>[^\(|\)]+)\)");
			return match.Success;
		}
		
		/// <summary>
		/// 匹配成员访问
		/// </summary>
		/// <param name="expr"></param>
		/// <param name="math"></param>
		/// <returns></returns>
		public static bool TryMemberAccessToken(string expr,out Match math)
		{
			math = Regex.Match(expr, @"(?<expr1>([_a-zA-Z]\w*)|(#\{\d+\}))\.(?<expr2>[_a-zA-Z]\w*)");
			return math.Success;
		}
	
		/// <summary>
		/// 获取表达式类型
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException"></exception>
		public static ExpressionType GetExpressionType(string token)
		{
			switch (token)
			{
				case "==":
					return ExpressionType.Equal;
				case "!=":
					return ExpressionType.NotEqual;
				case ">":
					return ExpressionType.GreaterThan;
				case "<":
					return ExpressionType.LessThan;
				case ">=":
					return ExpressionType.GreaterThanOrEqual;
				case "<=":
					return ExpressionType.LessThanOrEqual;
				case "+":
					return ExpressionType.Add;
				case "-":
					return ExpressionType.Subtract;
				case "*":
					return ExpressionType.Multiply;
				case "/":
					return ExpressionType.Divide;
				case "%":
					return ExpressionType.Modulo;
				case "&&":
					return ExpressionType.AndAlso;
				case "||":
					return ExpressionType.OrElse;
				case "!":
					return ExpressionType.Not;
			}
			throw new InvalidOperationException();
		}
	}
}
