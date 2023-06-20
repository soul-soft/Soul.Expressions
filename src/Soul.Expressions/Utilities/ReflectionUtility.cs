using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Soul.Expressions.Utilities
{
    public static class ReflectionUtility
    {
        /// <summary>
        /// 判断type2类型的指针是否可以指向type1的实列
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instanceType"></param>
        /// <returns></returns>
        public static bool IsAssignableFrom(Type type, Type instanceType)
        {
            if (type == instanceType)
            {
                return true;
            }
            else if (type.IsValueType && IsValueType(type))
            {
                var code1 = GetValueTypeCode(type);
                var code2 = GetValueTypeCode(instanceType);
                return code1 > code2;
            }
            return type.IsAssignableFrom(instanceType);
        }

        public static bool IsValueType(Type type)
        {
            var numabers = new Type[]
            {
                typeof(byte),
                typeof(short),
                typeof(int),
                typeof(long),
                typeof(float),
                typeof(double),
                typeof(decimal),
            };
            var underType = GetUnderlyingType(type);
            return numabers.Contains(underType);
        }

        public static int GetValueTypeCode(Type type)
        {
            var underType = GetUnderlyingType(type);
            if (underType == typeof(byte))
            {
                return 0;
            }
            if (underType == typeof(short))
            {
                return 1;
            }
            if (underType == typeof(int))
            {
                return 2;
            }
            if (underType == typeof(long))
            {
                return 3;
            }
            if (underType == typeof(float))
            {
                return 4;
            }
            if (underType == typeof(double))
            {
                return 5;
            }
            if (underType == typeof(double))
            {
                return 6;
            }
            return -1;
        }

        public static Type GetUnderlyingType(Type type)
        {
            return Nullable.GetUnderlyingType(type) ?? type;
        }

        public static MethodInfo FindMethod(IEnumerable<MethodInfo> methods, IEnumerable<Expression> expressions)
        {
            var arguments = expressions.Select(s => s.Type).ToArray();
            foreach (var item in methods)
            {
                var methodArguments = item.GetParameters().Select(a => a.ParameterType).ToArray();
                var flag = true;
                for (int i = 0; i < arguments.Length; i++)
                {
                    if (methodArguments[i] == arguments[i])
                    {
                        continue;
                    }
                    if (IsAssignableFrom(methodArguments[i], arguments[i]))
                    {
                        continue;
                    }
                    flag = false;
                }
                if (flag)
                {
                    return item;
                }
            }
            return null;
        }
    }
}
