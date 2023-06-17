using System.Linq.Expressions;

namespace Soul.Expression.Test
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var tree0 = SyntaxEngine.Run("!(1 < age) && 1 > 2");
			Console.WriteLine(tree0.Raw);
			var tree1 = SyntaxEngine.Run("(1 + 2) * 4 / 5");
			Console.WriteLine(tree1.Raw);
			var tree2 = SyntaxEngine.Run("Pow(2, 2)");
			Console.WriteLine(tree2.Raw);
			var tree3 = SyntaxEngine.Run("a >= 2 && 1 > 0");
			Console.WriteLine(tree3.Raw);
			var tree4 = SyntaxEngine.Run("a > 2 && true");
			Console.WriteLine(tree4.Raw);
		}

		public void Test()
		{
			var tree0 = SyntaxEngine.Run("!flag && 1 > 2");
			Console.WriteLine(tree0.Raw);
			var tree1 = SyntaxEngine.Run("(1 + 2) * 4 / 5");
			Console.WriteLine(tree1.Raw);
			var tree2 = SyntaxEngine.Run("Pow(2, 2)");
			Console.WriteLine(tree2.Raw);
			var tree3 = SyntaxEngine.Run("a >= 2 && 1 > 0");
			Console.WriteLine(tree3.Raw);
			var tree4 = SyntaxEngine.Run("a > 2 && true");
			Console.WriteLine(tree4.Raw);
		}
	}
}