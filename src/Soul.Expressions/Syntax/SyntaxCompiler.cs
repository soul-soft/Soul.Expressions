using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Soul.Expressions.Tokens;

namespace Soul.Expressions
{
	public static class SyntaxCompiler
	{
		public static Dictionary<string, MethodInfo> Methods = new Dictionary<string, MethodInfo>();

		public static void RegisterStaticMethods(Type type)
		{
			var methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public);
			foreach (var item in methods)
			{
				Methods.Add(item.Name,item);
			}
		}

		public static LambdaExpression Lambda(string expr, params SyntaxParameter[] parameters)
		{
			var tree = SyntaxEngine.Run(expr, parameters);
			return Lambda(tree);
		}

		public static LambdaExpression Lambda(SyntaxTree tree)
		{
			var context = new SyntaxCompilerContext(tree);
			foreach (var item in tree.Tokens)
			{
				var expression = Watch(item.Value, context);
				context.AddExpression(item.Key, expression);
			}
			var body = context.GetBody();
			var parameters = context.GetParameters();
			return Expression.Lambda(body, parameters);
		}

		private static Expression Watch(SyntaxToken token, SyntaxCompilerContext context)
		{
			if (token is ParameterToken parameterToken)
			{
				return Expression.Parameter(parameterToken.Type, parameterToken.Name);
			}

			if (token is MemberToken memberToken)
			{
				var expression = context.GetExpression(memberToken.Expression);
				var member = expression.Type.GetMember(memberToken.Member).First();
				return Expression.MakeMemberAccess(expression, member);
			}

			if (token is ConstantToken constantToken)
			{
				var value = constantToken.ParsedValue();
				return Expression.Constant(value, constantToken.Type);
			}

			if (token is UnaryToken unaryToken)
			{
				if (unaryToken.Type == "!")
				{
					var expression = context.GetExpression(unaryToken.Operand);
					return Expression.MakeUnary(ExpressionType.Not, expression, null);
				}
				else
				{
					var message = string.Format("Unrecognized syntax token：“{0}”", unaryToken.Raw);
					throw new NotImplementedException(message);
				}
			}

			if (token is BinaryToken binaryExpression)
			{
				var type = SyntaxUtility.GetExpressionType(binaryExpression.BinaryType);
				var left = context.GetExpression(binaryExpression.Left);
				var right = context.GetExpression(binaryExpression.Right);
				if (left.Type == typeof(double) || right.Type == typeof(double))
				{
					if (left.Type != typeof(double))
					{
						left = Expression.Convert(left, typeof(double));
					}
					else
					{
						right = Expression.Convert(right, typeof(double));
					}
				}
				return Expression.MakeBinary(type, left, right);
			}

			if (token is StaticMethodCallToken methodCallToken)
			{
				Methods.TryGetValue(methodCallToken.Method,out MethodInfo method);
				var parameters = context.GetExpressions(methodCallToken.Arguments);
				return Expression.Call(method, parameters);
			}
			return null;
		}
	}
}
