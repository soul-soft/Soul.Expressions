using Soul.Expressions.Utilities;

namespace Soul.Expressions.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod("测试自动类型转换")]
        public void AutoTypeCast()
        {
            //var f1 = ReflectionUtility.IsNumableType(typeof(UnitTest1));
            //var f2 = ReflectionUtility.IsNumableType(typeof(int));
            //var f3 = ReflectionUtility.IsNumableType(typeof(int?));
            var flag1 = ReflectionUtility.IsAssignableFrom(typeof(object), typeof(int?));
            var options = new SyntaxOptions();
            options.RegisterFunction(typeof(Functions));
            var flag = typeof(long).IsAssignableFrom(typeof(int));
            var expr = "Pow(1.0, 2)";
            var compiler = new SyntaxCompiler(options);
            var labmda = compiler.Lambda(expr);
            var result = labmda.Compile().DynamicInvoke();
            Assert.AreEqual(result, 2.2);
        }
    }
}