using System;

namespace Soul.Expressions
{
	public class SyntaxParameter
	{
		public string Name { get; }
		public object Value { get; }
		public Type Type { get; }

		public SyntaxParameter(string name, Type type)
		{
			Name = name;
			Type = type;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is SyntaxParameter))
			{
				return false;
			}
			var other = (SyntaxParameter)obj;
			return other.Name == this.Name;
		}

		public override int GetHashCode()
		{
			return Name.GetHashCode();
		}
	}
}
