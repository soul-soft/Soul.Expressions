using System.Linq.Expressions;

namespace Soul.Expressions.Test
{
	internal class Program
	{
		static void Main(string[] args)
		{
			SyntaxEngineSettings.RegisterMethod("pow", new Func<int, bool>((age) => age > 0));
			var context = new SyntaxContext();
			context.AddParameter("age", 20);
			var tree0 = SyntaxEngine.Run("pow(2,age)", context);
			Console.WriteLine(tree0.Raw);
		}

		public void Test()
		{
			var context = new SyntaxContext();
			var tree0 = SyntaxEngine.Run("!flag && 1 > 2", context);
			Console.WriteLine(tree0.Raw);
			var tree1 = SyntaxEngine.Run("(1 + 2) * 4 / 5", context);
			Console.WriteLine(tree1.Raw);
			var tree2 = SyntaxEngine.Run("Pow(2, 2)", context);
			Console.WriteLine(tree2.Raw);
			var tree3 = SyntaxEngine.Run("a >= 2 && 1 > 0", context);
			Console.WriteLine(tree3.Raw);
			var tree4 = SyntaxEngine.Run("a > 2 && true", context);
			Console.WriteLine(tree4.Raw);
		}
	}
}