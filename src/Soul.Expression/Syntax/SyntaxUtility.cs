using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Soul.Expression
{
	public static class SyntaxUtility
	{
		/// <summary>
		/// 是否为常量
		/// </summary>
		/// <param name="expr"></param>
		/// <returns></returns>
		public static bool IsConstant(string expr)
		{
			if (IsBool(expr))
			{
				return true;
			}
			if (IsNumber(expr))
			{
				return true;
			}
			if (IsString(expr))
			{
				return true;
			}
			if (IsChar(expr))
			{
				return true;
			}
			return false;
		}
		/// <summary>
		/// 是否为字符串常量
		/// </summary>
		/// <param name="expr"></param>
		/// <returns></returns>
		public static bool IsString(string expr)
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
		public static bool IsChar(string expr)
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
		/// 是否为数字常量
		/// </summary>
		/// <param name="expr"></param>
		/// <returns></returns>
		public static bool IsNumber(string expr)
		{
			return Regex.IsMatch(expr, @"^\d+(\.\d)*$");
		}
		/// <summary>
		/// 是否为整数
		/// </summary>
		/// <param name="expr"></param>
		/// <returns></returns>
		public static bool IsIntger(string expr)
		{
			return Regex.IsMatch(expr, @"^\d+$");
		}
		/// <summary>
		/// 是否为浮点数
		/// </summary>
		/// <param name="expr"></param>
		/// <returns></returns>
		public static bool IsDouble(string expr)
		{
			return Regex.IsMatch(expr, @"^\d+\.\d+$");
		}
		/// <summary>
		/// 是否为布尔值
		/// </summary>
		/// <param name="expr"></param>
		/// <returns></returns>
		public static bool IsBool(string expr)
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
		public static string[] SplitParameters(string expr)
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
	}
}
