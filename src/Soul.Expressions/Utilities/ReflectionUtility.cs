using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Soul.Expressions.Utilities
{
    public static class ReflectionUtility
    {
        public static bool IsAssignableFrom(Type referenceType, Type instanceType)
        {
            if (referenceType == instanceType)
            {
                return true;
            }
            else if (referenceType.IsValueType && IsValueType(referenceType))
            {
                var referenceTypeCode = GetTypeCode(referenceType);
                var instanceTypeCode = GetTypeCode(instanceType);
                return referenceTypeCode > instanceTypeCode;
            }
            return referenceType.IsAssignableFrom(instanceType);
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

        public static int GetTypeCode(Type type)
        {
            var underlyingType = GetUnderlyingType(type);
            if (underlyingType == typeof(byte))
            {
                return 0;
            }
            if (underlyingType == typeof(short))
            {
                return 1;
            }
            if (underlyingType == typeof(int))
            {
                return 2;
            }
            if (underlyingType == typeof(long))
            {
                return 3;
            }
            if (underlyingType == typeof(float))
            {
                return 4;
            }
            if (underlyingType == typeof(double))
            {
                return 5;
            }
            if (underlyingType == typeof(double))
            {
                return 6;
            }
            return int.MaxValue;
        }

        public static Type GetUnderlyingType(Type type)
        {
            return Nullable.GetUnderlyingType(type) ?? type;
        }

        public static bool IsNullableType(Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }

        public static Type GetBinaryExpressionType(Type type1, Type type2)
        {
            Type resultType = GetUnderlyingType(type1);
            if (GetTypeCode(type1) < GetTypeCode(type2))
            {
                resultType = GetUnderlyingType(type2);
            }
            var integers = new Type[]
            {
                typeof(byte),
                typeof(short),
                typeof(sbyte),
                typeof(ushort)
            };
            var floats = new Type[]
            {
                typeof(float),
            };
            if (integers.Contains(resultType))
            {
                resultType = typeof(int);
            }
            if (floats.Contains(resultType))
            {
                resultType = typeof(double);
            }
            if (resultType.IsValueType && (IsNullableType(type1) || IsNullableType(type2)))
            {
                return typeof(Nullable<>).MakeGenericType(resultType);
            }
            return resultType;
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
