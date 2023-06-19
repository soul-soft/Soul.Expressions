namespace Soul.Expressions.Test
{
    [TestClass]
    public class CallTest
    {
        [TestMethod("测试函数调用1")]
        public void Call1()
        {
            var expr = "Pow(2,3)";
            var options = new SyntaxOptions();
            options.RegisterFunction(typeof(Functions));
            var compiler = new SyntaxCompiler(options);
            var labmda = compiler.Lambda(expr);
            var result = (int?)labmda.Compile().DynamicInvoke();
            Assert.AreEqual(result, 8);
        }
    }
}
