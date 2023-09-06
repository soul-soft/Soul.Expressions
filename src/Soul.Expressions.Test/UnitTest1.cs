namespace Soul.Expressions.Test
{
    public class P
    {
        public string Name { get; set; }
    }
    [TestClass]
    public class UnitTest1
    {
        [TestMethod("测试自动类型转换")]
        public void AutoTypeCast()
        {
            var options = new SyntaxOptions();
            var expr = "1 + 0.9";
            var compiler = new SyntaxCompiler(options);
            var context = new SyntaxContext(expr, new Parameter("a", typeof(int)));
            var labmda = compiler.Lambda(context);
            Console.WriteLine(context.DebugView);
        }
    }
}