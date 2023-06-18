using System.Linq.Expressions;

namespace Soul.Expressions.Test
{
	public class C
	{
		public string Name { get; set; }
	}

	internal class Program
	{
		static void Main(string[] args)
		{
			var context = new ExpressionSyntaxContext("!flag && 1 > 2");
			context.AddParameter("flag",typeof(bool));
			var labmda = ExpressionSyntax.Lambda(context);
		}

		//public static void Test()
		//{
		//	var tree0 = ExpressionEngine.Run("!flag && 1 > 2", new Parameter("flag", typeof(bool)));
		//	var expression0 = SyntaxCompiler.Lambda(tree0);
		//	Console.WriteLine(tree0.Debug);
		//	var tree1 = ExpressionEngine.Run("(1 + 2) * 4 / 5");
		//	var expression1 = SyntaxCompiler.Lambda(tree1);
		//	Console.WriteLine(tree1.Debug);
		//	var tree2 = ExpressionEngine.Run("Pow(2, 2) + 2");
		//	var expression2 = SyntaxCompiler.Lambda(tree2);
		//	Console.WriteLine(tree2.Debug);
		//	var tree3 = ExpressionEngine.Run("a >= 2 && 1 > 0", new Parameter("a", typeof(int)));
		//	var expression3 = SyntaxCompiler.Lambda(tree3);
		//	Console.WriteLine(tree3.Debug);
		//	var tree4 = ExpressionEngine.Run("a > 2 && true", new Parameter("a", typeof(int)));
		//	var expression4 = SyntaxCompiler.Lambda(tree4);
		//	Console.WriteLine(tree4.Debug);
		//}
	}
}