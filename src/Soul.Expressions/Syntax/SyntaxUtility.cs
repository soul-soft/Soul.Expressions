using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Soul.Expressions
{
	internal static class SyntaxUtility
	{
		/// <summary>
		/// 是否为常量
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public static bool TryConstantToken(string token, out ConstantExpression constantExpression)
		{
			if (token == "null")
			{
				constantExpression = Expression.Constant(null);
				return true;
			}
			if (IsIntgerConstantToken(token))
			{
				constantExpression = Expression.Constant(Convert.ToInt32(token));
				return true;
			}
			if (IsBoolConstantToken(token))
			{
				constantExpression = Expression.Constant(Convert.ToBoolean(token));
				return true;
			}
			if (IsDoubleConstantToken(token))
			{
				constantExpression = Expression.Constant(Convert.ToDouble(token));
				return true;
			}
			if (IsStringConstantToken(token))
			{
				constantExpression = Expression.Constant(Convert.ToString(token));
				return true;
			}
			if (IsCharConstantToken(token))
			{
				constantExpression = Expression.Constant(Convert.ToChar(token));
				return true;
			}
			constantExpression = null;
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
		public static bool TryNotUnaryToken(string expr, out Match match)
		{
			match = Regex.Match(expr, @"\!(?<expr>\w+|\w+\.\w+|#\{\d+\})");
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
			match = Regex.Match(expr, @"(?<instance>\w+)\.(?<name>\w+)\((?<args>[^\(|\)]*)\)");
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
			match = Regex.Match(expr, @"(?<name>\w+)\((?<args>[^\(|\)]*)\)");
			return match.Success;
		}

		/// <summary>
		/// 匹配成员访问
		/// </summary>
		/// <param name="expr"></param>
		/// <param name="math"></param>
		/// <returns></returns>
		public static bool TryMemberAccessToken(string expr, out Match math)
		{
			math = Regex.Match(expr, @"(?<owner>([_a-zA-Z]\w*)|(#\{\d+\}))\.(?<member>[_a-zA-Z]\w*)");
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

		/// <summary>
		/// 匹配函数
		/// </summary>
		/// <returns></returns>
		public static MethodInfo MatchMethod(string name, Type[] arguments, MethodInfo[] methods)
		{
			return methods.Where(a => a.Name == name)
				.Where(a => a.GetParameters().Length == arguments.Length)
				.Where(a => IsMatchMethod(a, arguments))
				.FirstOrDefault();
		}
		public static bool IsMatchMethod(MethodInfo method, Type[] arguments)
		{
			var parameters = method.GetParameters()
				.Select(a => a.ParameterType)
				.ToArray();
			for (int i = 0; i < arguments.Length; i++)
			{
				if (parameters[i] != arguments[i] && parameters[i].IsAssignableFrom(arguments[i]))
				{
					return false;
				}
			}
			return true;
		}
	}
}
