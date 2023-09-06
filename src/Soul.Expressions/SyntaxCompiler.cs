using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Soul.Expressions.Utilities;

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

        public LambdaExpression Lambda(string expression, params Parameter[] parameters)
        {
            return Lambda(new SyntaxContext(expression, parameters));
        }

        public LambdaExpression Lambda(string expression, Type resultType, params Parameter[] parameters)
        {
            return Lambda(new SyntaxContext(expression, resultType, parameters));
        }

        public LambdaExpression Lambda(SyntaxContext context)
        {
            var body = Watch(context.Expression, context);
            if (context.ResultType != null && body.Type != context.ResultType)
            {
                body = Expression.Convert(body, context.ResultType);
            }
            return Expression.Lambda(body, context.Parameters);
        }

        private Expression Watch(string token, SyntaxContext context)
        {
            if (context.TryGetToken(token, out SyntaxToken syntaxToken))
            {
                return syntaxToken.Expression;
            }
            //处理参数
            if (context.TryGetParameter(token, out ParameterExpression parameterExpression))
            {
                context.AddToken(token, parameterExpression);
                return parameterExpression;
            }
            //处理常量
            if (SyntaxUtility.TryConstantToken(token, out ConstantExpression constantExpression))
            {
                context.AddToken(token, constantExpression);
                return constantExpression;
            }
            //处理成员访问
            if (SyntaxUtility.TryMemberAccessToken(token, out Match memberAccessMatch))
            {
                var owner = memberAccessMatch.Groups["owner"].Value;
                var memberName = memberAccessMatch.Groups["member"].Value;
                var ownerExpression = Watch(owner, context);
                var member = ownerExpression.Type.GetProperty(memberName)
                    ?? throw new MemberAccessException(token);
                var value = memberAccessMatch.Value;
                var key = context.AddToken(value, Expression.MakeMemberAccess(ownerExpression, member));
                var newToken = token.Replace(value, key);
                return Watch(newToken, context);
            }
            //处理静态函数
            if (SyntaxUtility.TryStaticMethodCallToken(token, out Match staticMethodCallMatch))
            {
                var name = staticMethodCallMatch.Groups["name"].Value;
                var argsExpression = staticMethodCallMatch.Groups["args"].Value;
                var value = staticMethodCallMatch.Value;
                var arguments = new List<Expression>();
                var argumentTokens = SyntaxUtility.SplitArgumentTokens(argsExpression);
                foreach (var item in argumentTokens)
                {
                    var argument = Watch(item, context);
                    arguments.Add(argument);
                }
                var functions = Options.Functions.Where(a => a.Name == name);
                var method = ReflectionUtility.FindMethod(functions, arguments) ?? throw new MissingMethodException(token);

                var parameters = SyntaxUtility.ConvertExpressionType(method, arguments);
                var key = context.AddToken(token, Expression.Call(null, method, parameters));
                var newToken = token.Replace(value, key);
                return Watch(newToken, context);
            }
            //处理括号
            if (SyntaxUtility.TryIncludeToken(token, out Match includeMatch))
            {
                var value = includeMatch.Value;
                var expr = includeMatch.Groups["expr"].Value;
                var expression = Watch(expr, context);
                var key = context.AddToken(value, expression);
                var newToken = token.Replace(value, key);
                return Watch(newToken, context);
            }
            //处理逻辑非
            if (SyntaxUtility.TryNotUnaryToken(token, out Match unaryMatch))
            {
                var expr = unaryMatch.Groups["expr"].Value;
                var operand = Watch(expr, context);
                var value = unaryMatch.Value;
                var key = context.AddToken(value, Expression.MakeUnary(ExpressionType.Not, operand, null));
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
                var resultType = ReflectionUtility.GetBinaryExpressionType(left.Type, right.Type);
                if (left.Type != resultType)
                {
                    left = Expression.Convert(left, resultType);
                }
                if (right.Type != resultType)
                {
                    right = Expression.Convert(right, resultType);
                }
                var value = binaryMatch.Value;
                var binaryType = SyntaxUtility.GetExpressionType(expr2);
                var key = context.AddToken(value, Expression.MakeBinary(binaryType, left, right));
                var newToken = token.Replace(value, key);
                return Watch(newToken, context);
            }
            var message = string.Format("Unrecognized syntax token：“{0}”", token);
            throw new NotImplementedException(message);
        }
    }
}
