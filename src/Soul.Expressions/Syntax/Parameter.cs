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

		public override bool Equals(object obj)
		{
			if (!(obj is Parameter))
			{
				return false;
			}
			var other = (Parameter)obj;
			return other.Name == this.Name;
		}

		public override int GetHashCode()
		{
			return Name.GetHashCode();
		}
	}
}
