using System.Collections.Generic;
using System.Reflection;

namespace Soul.Expressions.Syntax
{
	public class SyntaxEngineBuilder
	{
		private List<Assembly> _assembiles = new List<Assembly>();

		public SyntaxEngineBuilder()
		{

		}

		public SyntaxEngineBuilder ReferenceAssembly(Assembly assembly)
		{
			_assembiles.Add(assembly);
			return this;
		}

		public SyntaxEngineBuilder ReferenceAssemblies(params Assembly[] assemblies)
		{
			_assembiles.AddRange(assemblies);
			return this;
		}
	
	}
}
