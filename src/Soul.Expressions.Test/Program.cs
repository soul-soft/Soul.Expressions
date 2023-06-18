using System.Linq.Expressions;

namespace Soul.Expressions.Test
{
	public class P
	{
		public C Age;
	}
	public class C
	{
		public string Name { get; set; }
	}

	internal class Program
	{
		static void Main(string[] args)
		{
			Expression<Func<P, string>> expr = p => p.Age.Name;

			var syntax = SyntaxEngine.Run("p.Age.Name", new SyntaxParameter("p", typeof(P)));
			SyntaxCompiler.RegisterStaticMethods(typeof(Funcs));
			var expression = SyntaxCompiler.Lambda(syntax);
			var func = expression.Compile();
			Console.WriteLine(func.DynamicInvoke(new P { Age = new C { Name = "aa"} }));
			Test();
		}

		public static void Test()
		{
			var tree0 = SyntaxEngine.Run("!flag && 1 > 2", new SyntaxParameter("flag", typeof(bool)));
			var expression0 = SyntaxCompiler.Lambda(tree0);
			Console.WriteLine(tree0.Debug);
			var tree1 = SyntaxEngine.Run("(1 + 2) * 4 / 5");
			var expression1 = SyntaxCompiler.Lambda(tree1);
			Console.WriteLine(tree1.Debug);
			var tree2 = SyntaxEngine.Run("Pow(2, 2) + 2");
			var expression2 = SyntaxCompiler.Lambda(tree2);
			Console.WriteLine(tree2.Debug);
			var tree3 = SyntaxEngine.Run("a >= 2 && 1 > 0", new SyntaxParameter("a", typeof(int)));
			var expression3 = SyntaxCompiler.Lambda(tree3);
			Console.WriteLine(tree3.Debug);
			var tree4 = SyntaxEngine.Run("a > 2 && true", new SyntaxParameter("a", typeof(int)));
			var expression4 = SyntaxCompiler.Lambda(tree4);
			Console.WriteLine(tree4.Debug);
		}
	}
}