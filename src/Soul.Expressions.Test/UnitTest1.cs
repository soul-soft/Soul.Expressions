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
            options.RegisterFunction(typeof(Functions));
            var expr = "p.Name == null";
            var compiler = new SyntaxCompiler(options);
            var labmda = compiler.Lambda(expr, new Parameter("p", typeof(P)));
            var result = labmda.Compile().DynamicInvoke(new P { });
            Assert.AreEqual(result, 2.2);
        }
    }
}