using System;
using System.Collections.Generic;
using System.Reflection;

namespace Soul.Expressions.Syntax
{
	public class SyntaxOptions
	{
		public List<MethodInfo> GlobalFunctions { get; } = new List<MethodInfo>();
		
		public SyntaxOptions() 
		{

		}

		public void RegisterFunction(MethodInfo method)
		{
			GlobalFunctions.Add(method);
		}

		public void RegisterFunction(Type type)
		{
			var methods = type.GetMethods(BindingFlags.Public|BindingFlags.Static);
			foreach (var item in methods)
			{
				RegisterFunction(item);
			}
		}
	}
}
