using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Soul.Expressions.Utilities
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
            if (IsBooleanConstantToken(token))
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
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool IsStringConstantToken(string token)
        {
            if (token.Length < 2)
            {
                return false;
            }
            var text = Regex.Replace(token, @"\\.{1}", "#");
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
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool IsCharConstantToken(string token)
        {
            if (token.Length < 3)
            {
                return false;
            }
            var text = Regex.Replace(token, @"\\.{1}", "#");
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
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool IsIntgerConstantToken(string token)
        {
            return Regex.IsMatch(token, @"^\d+$");
        }

        /// <summary>
        /// 是否为浮点数
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool IsDoubleConstantToken(string token)
        {
            return Regex.IsMatch(token, @"^\d+\.\d+$");
        }

        /// <summary>
        /// 是否为布尔值
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool IsBooleanConstantToken(string token)
        {
            if (token == "true")
            {
                return true;
            }
            if (token == "false")
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 分割函数参数
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string[] SplitArgumentTokens(string token)
        {
            var args = new List<string>();
            var index = 0;
            var quotes = new char[]
            {
                '"', '\''
            };
            var startQuotes = false;
            for (int i = 0; i < token.Length; i++)
            {
                var item = token[i];
                if (quotes.Contains(item))
                {
                    if (!startQuotes)
                    {
                        index = i;
                        startQuotes = true;
                    }
                    else if (i > 0 && token[i - 1] != '\\')
                    {
                        startQuotes = false;
                    }
                }
                if (!startQuotes && item == ',')
                {
                    args.Add(token.Substring(index, i - index));
                    index = i + 1;
                }
                if (i == token.Length - 1)
                {
                    args.Add(token.Substring(index, i - index + 1));
                }
            }
            return args.Select(s => s.Trim()).ToArray();
        }

        /// <summary>
        /// 处理括号运算
        /// </summary>
        /// <param name="token"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public static bool TryIncludeToken(string token, out Match match)
        {
            match = Regex.Match(token, @"\((?<expr>.+)\)");
            return match.Success;
        }

        /// <summary>
        /// 处理逻辑非
        /// </summary>
        /// <param name="token"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public static bool TryNotUnaryToken(string token, out Match match)
        {
            match = Regex.Match(token, @"\!(?<expr>\w+|\w+\.\w+|#\{\d+\})");
            return match.Success;
        }

        /// <summary>
        /// 二元运算
        /// </summary>
        /// <param name="token"></param>
        /// <param name="math"></param>
        /// <returns></returns>
        public static bool TryBinaryToken(string token, out Match math)
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
                math = Regex.Match(token, pattern);
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
        /// <param name="token"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public static bool TryInstanceMethodCallToken(string token, out Match match)
        {
            match = Regex.Match(token, @"(?<instance>\w+)\.(?<name>\w+)\((?<args>[^\(|\)]*)\)");
            return match.Success;
        }

        /// <summary>
        /// 匹配实列函数调用
        /// </summary>
        /// <param name="token"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public static bool TryStaticMethodCallToken(string token, out Match match)
        {
            match = Regex.Match(token, @"(?<name>\w+)\((?<args>[^\(|\)]*)\)");
            return match.Success;
        }

        /// <summary>
        /// 匹配成员访问
        /// </summary>
        /// <param name="token"></param>
        /// <param name="math"></param>
        /// <returns></returns>
        public static bool TryMemberAccessToken(string token, out Match math)
        {
            math = Regex.Match(token, @"(?<owner>([_a-zA-Z]\w*)|(#\{\d+\}))\.(?<member>[_a-zA-Z]\w*)");
            return math.Success;
        }

        /// <summary>
        /// 获取表达式类型
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
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
            throw new NotImplementedException(token);
        }

        /// <summary>
        /// 类型自动转换
        /// </summary>
        /// <param name="expression1"></param>
        /// <param name="expression2"></param>
        public static void ConvertBinaryExpression(ref Expression expression1, ref Expression expression2)
        {
            if (expression1.Type != expression2.Type)
            {
                if (ReflectionUtility.IsAssignableFrom(expression1.Type, expression2.Type))
                {
                    expression1 = Expression.Convert(expression1, expression2.Type);
                }
                else
                {
                    expression2 = Expression.Convert(expression2, expression1.Type);
                }
            }
        }

        /// <summary>
        /// 参数类型转换
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<Expression> ConvertArgumentExpressions(MethodInfo method, IEnumerable<Expression> expressions)
        {
            var arguments = expressions.ToArray();
            var parameters = method.GetParameters().Select(s => s.ParameterType).ToArray();
            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                var expression = arguments[i];
                yield return ConvertExpression(expression, parameter);
            }
        }
       
        /// <summary>
        /// 自动类型转换
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Expression ConvertExpression(Expression expression, Type type)
        {
            if (expression.Type != type)
            {
                return Expression.Convert(expression, type);
            }
            return expression;
        }
    }
}
