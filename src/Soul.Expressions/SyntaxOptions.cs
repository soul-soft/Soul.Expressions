using System;
using System.Collections.Generic;
using System.Reflection;

namespace Soul.Expressions
{
    public class SyntaxOptions
    {
        public List<MethodInfo> Functions { get; } = new List<MethodInfo>();

        public SyntaxOptions()
        {

        }

        public void RegisterFunction(MethodInfo method)
        {
            Functions.Add(method);
        }

        public void RegisterFunction(Type type)
        {
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
            foreach (var item in methods)
            {
                RegisterFunction(item);
            }
        }
    }
}
