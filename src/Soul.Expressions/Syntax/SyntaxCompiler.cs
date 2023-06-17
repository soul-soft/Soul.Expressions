using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Soul.Expressions
{
	public static class SyntaxCompiler
	{
		public static Dictionary<string, MethodInfo> Methods = new Dictionary<string, MethodInfo>();

		public static void RegisterMethod(string name, MethodInfo method)
		{
			Methods.Add(name, method);
		}

		public static void RegisterMethod(string name, Delegate method)
		{
			Methods.Add(name, method.Method);
		}

		public static LambdaExpression MakeLambda(SyntaxTree tree)
		{
			return null;
		}
	}
}
