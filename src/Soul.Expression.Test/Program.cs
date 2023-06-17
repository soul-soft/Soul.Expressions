namespace Soul.Expression.Test
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var tree1 = SyntaxEngine.Run("1 + 2 * 4 / 5");
			Console.WriteLine(tree1.Raw);
			var tree2 = SyntaxEngine.Run("Pow(2, 2)");
			Console.WriteLine(tree2.Raw);
			var tree3 = SyntaxEngine.Run("!falg && 1 > 0");
			Console.WriteLine(tree3.Raw);
		}
	}
}