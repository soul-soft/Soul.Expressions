using System;

namespace Soul.Expressions
{
	public class Parameter
	{
		public string Name { get; }
		public Type Type { get; }

		public Parameter(string name, Type type)
		{
			Name = name;
			Type = type;
		}
	}
}
