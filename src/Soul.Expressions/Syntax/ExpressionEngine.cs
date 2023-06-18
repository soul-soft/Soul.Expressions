using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Soul.Expressions
{
	/// <summary>
	/// 语法分析引擎 
	/// </summary>
	public static class ExpressionSyntax
	{
		public static LambdaExpression Lambda(ExpressionSyntaxContext context)
		{
			var body = Watch(context.Expression, context);
			return Expression.Lambda(body, context.Parameters);
		}

		private static Expression Watch(string token, ExpressionSyntaxContext context)
		{
			if (context.TryGetToken(token, out Expression tokenExpression))
			{
				return tokenExpression;
			}
			//处理参数
			if (context.TryGetParameter(token, out ParameterExpression parameterExpression))
			{
				return parameterExpression;
			}
			//处理常量
			if (SyntaxUtility.TryConstantToken(token, out ConstantExpression constantExpression))
			{
				return constantExpression;
			}
			//处理成员访问
			if (SyntaxUtility.TryMemberAccessToken(token, out Match memberAccessMatch))
			{
				var value = memberAccessMatch.Value;
				var owner = memberAccessMatch.Groups["owner"].Value;
				var member = memberAccessMatch.Groups["member"].Value;
				var ownerExpression = Watch(owner, context);
				var key = context.AddToken(ownerExpression);
				var newToken = token.Replace(value, key);
				return Watch(newToken, context);
			}
			////处理实列函数
			//if (SyntaxUtility.TryInstanceMethodCallToken(token, out Match instanceMethodCallMatch))
			//{
			//	var instance = instanceMethodCallMatch.Groups["instance"].Value;
			//	var name = instanceMethodCallMatch.Groups["name"].Value;
			//	var argsExpr = instanceMethodCallMatch.Groups["args"].Value;
			//	var value = instanceMethodCallMatch.Value;
			//	var args = SyntaxUtility.SplitTokens(argsExpr);
			//	var parameters = new List<string>();
			//	foreach (var item in args)
			//	{
			//		var argKey = Watch(item, tree);
			//		parameters.Add(argKey);
			//	}
			//	var instanceKey = Watch(instance, tree);
			//	var token = new MethodCallToken(instanceKey, name, parameters.ToArray());
			//	var key = tree.AddToken(token);
			//	var newExpr = token.Replace(value, key);
			//	return Watch(newExpr, tree);
			//}
			////处理静态函数
			//if (SyntaxUtility.TryStaticMethodCallToken(token, out Match staticMethodCallMatch))
			//{
			//	var name = staticMethodCallMatch.Groups["name"].Value;
			//	var argsExpr = staticMethodCallMatch.Groups["args"].Value;
			//	var value = staticMethodCallMatch.Value;
			//	var args = SyntaxUtility.SplitTokens(argsExpr);
			//	var parameters = new List<string>();
			//	foreach (var item in args)
			//	{
			//		var argKey = Watch(item, tree);
			//		parameters.Add(argKey);
			//	}
			//	var token = new MethodCallToken(name, parameters.ToArray());
			//	var key = tree.AddToken(token);
			//	var newExpr = token.Replace(value, key);
			//	return Watch(newExpr, tree);
			//}
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
				var value = unaryMatch.Value;
				var expr = unaryMatch.Groups["expr"].Value;
				var operand = Watch(expr, context);
				var key = context.AddToken(Expression.MakeUnary(ExpressionType.Not, operand, null));
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
