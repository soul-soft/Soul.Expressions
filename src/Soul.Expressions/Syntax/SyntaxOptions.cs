using System;
using System.Collections.Generic;
using System.Linq;

namespace Soul.Expressions
{
	public class SyntaxOptions
	{
		private HashSet<SyntaxParameter> _parameters = new HashSet<SyntaxParameter>();

		public IReadOnlyCollection<SyntaxParameter> Parameters => _parameters;

		public void AddParameter(string name, object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}
			AddParameter(name, value, value.GetType());
		}

		public void AddParameter(string name, object value, Type type)
		{
			_parameters.Add(new SyntaxParameter(name, value, type));
		}

		internal bool ContainsParameter(string name)
		{
			return _parameters.Any(a => a.Name == name);
		}
	}
}
