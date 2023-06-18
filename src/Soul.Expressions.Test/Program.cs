namespace Soul.Expressions.Test
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var expr1 = "1+1.2";
			var expr2 = "1 + 4 / 2 * 5";
			var expr3 = "1 + 2 > 5 && 2 / 3 > 5";
			var options = new SyntaxOptions();//编译选项
			options.RegisterFunction(typeof(GlobalMethods));//注册全局函数
			var compiler = new SyntaxCompiler(options);//创建编译器
			var context = new SyntaxContext(expr1);
			var labmda1 = compiler.Lambda(context);
			var context2 = new SyntaxContext(expr2);
			var labmda2 = compiler.Lambda(context2);
			var context3 = new SyntaxContext(expr3);
			var labmda3= compiler.Lambda(context3);
			Console.WriteLine($"{expr1} = " + labmda1.Compile().DynamicInvoke());
			Console.WriteLine($"{expr2} = " + labmda2.Compile().DynamicInvoke());
			Console.WriteLine($"{expr3} = " + labmda3.Compile().DynamicInvoke());
			Test();
		}

		public static void Test()
		{
			var options = new SyntaxOptions();
			options.RegisterFunction(typeof(GlobalMethods));
			var compiler = new SyntaxCompiler(options);
			var tree0 = new SyntaxContext("!flag && 1 > 2", new Parameter("flag", typeof(bool)));
			var expression0 = compiler.Lambda(tree0);
			Console.WriteLine(tree0.Debug);
			var tree1 = new SyntaxContext("(1 + 2) * 4 / 5");
			var expression1 = compiler.Lambda(tree1);
			Console.WriteLine(tree1.Debug);
			var tree2 = new SyntaxContext("Pow(2, 2) + 2");
			var expression2 = compiler.Lambda(tree2);
			Console.WriteLine(tree2.Debug);
			var tree3 = new SyntaxContext("a >= 2 && 1 > 0", new Parameter("a", typeof(int)));
			var expression3 = compiler.Lambda(tree3);
			Console.WriteLine(tree3.Debug);
			var tree4 = new SyntaxContext("a > 2 && true", new Parameter("a", typeof(int)));
			var expression4 = compiler.Lambda(tree4);
			Console.WriteLine(tree4.Debug);
		}
	}
}