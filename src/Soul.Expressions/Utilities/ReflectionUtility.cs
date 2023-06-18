using System;
using System.Collections.Generic;

namespace Soul.Expressions.Utilities
{
	public static class ReflectionUtility
	{
		/// <summary>
		/// 判断type2类型的指针是否可以指向type1的实列
		/// </summary>
		/// <param name="type1"></param>
		/// <param name="type2"></param>
		/// <returns></returns>
		public static bool IsIsAssignableFrom(Type type1, Type type2)
		{
			if (type1 == type2)
			{
				return true;
			}
			else if (type1.IsValueType)
			{
				var pows = new Dictionary<Type, int>
				{
					{ typeof(double), 5 },
					{ typeof(float), 4 },
					{ typeof(long), 3 },
					{ typeof(int), 2 },
					{ typeof(short), 1 },
					{ typeof(byte), 0 }
				};
				var pow1 = pows[type1];
				var pow2 = pows[type2];
				return pow1 > pow2;
			}
			return type1.IsAssignableFrom(type2);
		}
	}
}
