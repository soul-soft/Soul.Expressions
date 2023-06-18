using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Soul.Expressions.Syntax;

namespace Soul.Expressions
{
	/// <summary>
	/// 语法分析引擎 
	/// </summary>
	public class SyntaxCompiler
	{
		public SyntaxOptions Options { get; }

		public SyntaxCompiler()
			: this(new SyntaxOptions())
		{

		}

		public SyntaxCompiler(SyntaxOptions options)
		{
			Options = options;
		}

		public LambdaExpression Lambda(SyntaxContext context)
		{
			var body = Watch(context.Expression, context);
			return Expression.Lambda(body, context.Parameters);
		}

		private Expression Watch(string token, SyntaxContext context)
		{
			if (context.TryGetToken(token, out Expression tokenExpression))
			{
				return tokenExpression;
			}
			//处理参数
			if (context.TryGetParameter(token, out ParameterExpression parameterExpression))
			{
				context.AddToken(parameterExpression);
				return parameterExpression;
			}
			//处理常量
			if (SyntaxUtility.TryConstantToken(token, out ConstantExpression constantExpression))
			{
				context.AddToken(constantExpression);
				return constantExpression;
			}
			//处理成员访问
			if (SyntaxUtility.TryMemberAccessToken(token, out Match memberAccessMatch))
			{
				var owner = memberAccessMatch.Groups["owner"].Value;
				var memberName = memberAccessMatch.Groups["member"].Value;
				var ownerExpression = Watch(owner, context);
				var member = ownerExpression.Type.GetProperty(memberName);
				if (member == null)
				{
					throw new MemberAccessException(token);
				}
				var key = context.AddToken(Expression.MakeMemberAccess(ownerExpression, member));
				var value = memberAccessMatch.Value;
				var newToken = token.Replace(value, key);
				return Watch(newToken, context);
			}
			//处理静态函数
			if (SyntaxUtility.TryStaticMethodCallToken(token, out Match staticMethodCallMatch))
			{
				var name = staticMethodCallMatch.Groups["name"].Value;
				var argsExpr = staticMethodCallMatch.Groups["args"].Value;
				var value = staticMethodCallMatch.Value;
				var arguments = new List<Expression>();
				var argumentTokens = SyntaxUtility.SplitTokens(argsExpr);
				foreach (var item in argumentTokens)
				{
					var argument = Watch(item, context);
					arguments.Add(argument);
				}
				var argumentTypes = arguments.Select(s => s.Type).ToArray();
				var method = SyntaxUtility.MatchMethod(name, argumentTypes, Options.GlobalFunctions.ToArray());
				if (method == null)
				{
					throw new MissingMethodException(token);
				}
				var key = context.AddToken(Expression.Call(null, method, arguments));
				var newExpr = token.Replace(value, key);
				return Watch(newExpr, context);
			}
			//处理括号
			if (SyntaxUtility.TryIncludeToken(token, out Match includeMatch))
			{
				var value = includeMatch.Value;
				var expr = includeMatch.Groups["expr"].Value;
				var expression = Watch(expr, context);
				var key = context.AddToken(expression);
				var newToken = token.Replace(value, key);
				return Watch(newToken, context);
			}
			//处理逻辑非
			if (SyntaxUtility.TryNotUnaryToken(token, out Match unaryMatch))
			{
				var expr = unaryMatch.Groups["expr"].Value;
				var operand = Watch(expr, context);
				var key = context.AddToken(Expression.MakeUnary(ExpressionType.Not, operand, null));
				var value = unaryMatch.Value;
				var newToken = token.Replace(value, key);
				return Watch(newToken, context);
			}
			//处理二元运算
			if (SyntaxUtility.TryBinaryToken(token, out Match binaryMatch))
			{
				var expr1 = binaryMatch.Groups["expr1"].Value;
				var expr2 = binaryMatch.Groups["expr2"].Value;
				var expr3 = binaryMatch.Groups["expr3"].Value;
				var left = Watch(expr1, context);
				var right = Watch(expr3, context);
				var type = SyntaxUtility.GetExpressionType(expr2);
				var key = context.AddToken(Expression.MakeBinary(type, left, right));
				var value = binaryMatch.Value;
				var newToken = token.Replace(value, key);
				return Watch(newToken, context);
			}
			var message = string.Format("Unrecognized syntax token：“{0}”", token);
			throw new NotImplementedException(message);
		}
	}
}
