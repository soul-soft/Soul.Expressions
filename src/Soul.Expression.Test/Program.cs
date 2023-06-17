namespace Soul.Expression.Test
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var tree = SyntaxEngine.Run("1+2*4/5");

			Console.WriteLine(tree.Raw);
		}
	}
}