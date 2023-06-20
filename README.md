# Soul.Expressions

这是一款开源免费的词法分析引擎，支持动态编译、执行表达式，生成拉姆达表达式树。

## 基本使用

``` C#

var compiler = new SyntaxCompiler();
compiler.Lambda("a > 20 && 1+2 < 4", new Parameter("a", typeof(int)));
var labmda = compiler.Lambda(context);
var result = labmda.Compile().DynamicInvoke(2);

```

## 词法分析

``` C#
var options = new SyntaxOptions();
var expr = "a > 20 && 1+2 < 4";
var compiler = new SyntaxCompiler(options);
var context = new SyntaxContext(expr, new Parameter("a", typeof(int)));
var labmda = compiler.Lambda(context);
var result = labmda.Compile().DynamicInvoke(2);

```

