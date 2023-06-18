using System;

namespace Soul.Expressions.Tokens
{
	public class ConstantToken : SyntaxToken
	{
		public string Value { get; }
		public ConstantTokenValueType ValueType { get; }

		public ConstantToken(string value, ConstantTokenValueType valueType)
		{
			Raw = value;
			Value = value;
			ValueType = valueType;
		}

		public object ParsedValue()
		{
			if (ConstantTokenValueType.String == ValueType)
			{
				if (Value == "\"\"")
				{
					return string.Empty;
				}
				return Value.Substring(1, Value.Length - 2);
			}
			if (ConstantTokenValueType.Char == ValueType)
			{
				var ch = Value.Substring(1, Value.Length - 2);
				return Convert.ToChar(ch);
			}
			if (ConstantTokenValueType.Intger == ValueType)
			{
				return Convert.ToInt32(Value);
			}
			if (ConstantTokenValueType.Double == ValueType)
			{
				return Convert.ToDouble(Value);
			}
			if (ConstantTokenValueType.Boolean == ValueType)
			{
				return Convert.ToBoolean(Value);
			}
			return null;
		}

		public Type ParsedType()
		{
			switch (ValueType)
			{
				case ConstantTokenValueType.String:
					return typeof(string);
				case ConstantTokenValueType.Boolean:
					return typeof(bool);
				case ConstantTokenValueType.Char:
					return typeof(char);
				case ConstantTokenValueType.Intger:
					return typeof(int);
				case ConstantTokenValueType.Double:
					return typeof(double);
				case ConstantTokenValueType.None:
					return typeof(string);
				default:
					return null;
			}
		}
	}

	public enum ConstantTokenValueType
	{ 
		String,
		Boolean,
		Char,
		Intger,
		Null,
		Double,
		None
	}
}
