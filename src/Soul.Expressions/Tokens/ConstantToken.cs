using System;

namespace Soul.Expressions.Tokens
{
	public class ConstantToken : SyntaxToken
	{
		public string Value { get; }
		public Type Type { get; }

		public ConstantToken(string value, Type type)
		{
			Raw = value;
			Value = value;
			Type = type;
		}

		public object ParsedValue()
		{
			if (typeof(string) == Type)
			{
				if (Value == "\"\"")
				{
					return string.Empty;
				}
				return Value.Substring(1, Value.Length - 2);
			}
			if (typeof(char) == Type)
			{
				var ch = Value.Substring(1, Value.Length - 2);
				return Convert.ToChar(ch);
			}
			if (typeof(int) == Type)
			{
				return Convert.ToInt32(Value);
			}
			if (typeof(double) == Type)
			{
				return Convert.ToDouble(Value);
			}
			if (typeof(bool) == Type)
			{
				return Convert.ToBoolean(Value);
			}
			return null;
		}

	}
}
