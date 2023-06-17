using System.Linq.Expressions;

namespace Soul.Expressions.Test
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var syntax = SyntaxEngine.Run("pow(2,age) + 2", new SyntaxParameter("age", 20));
			Console.WriteLine(syntax.Raw);
			//var expression = SyntaxCompiler.Lambda(syntax);
			Test();
		}

		public static void Test()
		{
			var tree0 = SyntaxEngine.Run("!flag && 1 > 2", new SyntaxParameter("flag", false));
			Console.WriteLine(tree0.Raw);
			var tree1 = SyntaxEngine.Run("(1 + 2) * 4 / 5");
			Console.WriteLine(tree1.Raw);
			var tree2 = SyntaxEngine.Run("Pow(2, 2) + 2");
			Console.WriteLine(tree2.Raw);
			var tree3 = SyntaxEngine.Run("a >= 2 && 1 > 0", new SyntaxParameter("a", 1));
			Console.WriteLine(tree3.Raw);
			var tree4 = SyntaxEngine.Run("a > 2 && true", new SyntaxParameter("a", 1));
			Console.WriteLine(tree4.Raw);
		}
	}
}