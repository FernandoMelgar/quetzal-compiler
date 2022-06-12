using NUnit.Framework;
using QuetzalCompiler;
using QuetzalCompiler.Visitor;

namespace Tests;

public class WatVisitorTest
{
    private readonly TokenClassifier _classifier = new();

    [Test]
    public void TestFunctionWithVarDef()
    {
        var program = @"main() {
            var x;
        }";
        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        var ast = parser.Program();
        var fpv = new FirstPassVisitor();
        fpv.Visit((dynamic) ast);
        var spv = new SecondPassVisitor(fpv.FGST, fpv.VGST);
        spv.Visit((dynamic) ast);
        var watVisitor = new WatVisitor(spv.FGST);
        string result = watVisitor.Visit((dynamic) ast); // Function, Var declaration
        Assert.True(result.Contains(@"  (func"));
        Assert.True(result.Contains(@"    (export ""main"""));
        Assert.True(result.Contains(@"    (result i32)"));
        Assert.True(result.Contains(@"    (local $x i32)"));
        Assert.True(result.Contains(@"    i32.const 0"));
    }
    
    [Test]
    public void TestFunctionParams()
    {
        var program = @"sqr(x){}
            main() {
            var x;
        }";
        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        var ast = parser.Program();
        var fpv = new FirstPassVisitor();
        fpv.Visit((dynamic) ast);
        var spv = new SecondPassVisitor(fpv.FGST, fpv.VGST);
        spv.Visit((dynamic) ast);
        var watVisitor = new WatVisitor(spv.FGST);
        string result = watVisitor.Visit((dynamic) ast); // Function, Var declaration
        Assert.True(result.Contains(@"  (func"));
        Assert.True(result.Contains(@"    (export ""sqr"""));
        Assert.True(result.Contains(@"    (result i32)"));
        Assert.True(result.Contains(@"    (param $x i32)"));
        Assert.True(result.Contains(@"    i32.const 0"));
    }
    
    [Test]
    public void TestFunctionMainWithExportTag()
    {
        var program = @"main() {
        }";
        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        var ast = parser.Program();
        var fpv = new FirstPassVisitor();
        fpv.Visit((dynamic) ast);
        var spv = new SecondPassVisitor(fpv.FGST, fpv.VGST);
        spv.Visit((dynamic) ast);
        var watVisitor = new WatVisitor(spv.FGST);
        string result = watVisitor.Visit((dynamic) ast); // Function, Var declaration
        Assert.True(result.Contains(@"  (func"));
        Assert.True(result.Contains(@"    (export ""main"""));
        Assert.True(result.Contains(@"    (result i32)"));
        Assert.True(result.Contains(@"    i32.const 0"));
    }
    
    [Test]
    public void TestLocalFunctionSignature()
    {
        var program = @"sqr(x){}
        main() {
        }";
        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        var ast = parser.Program();
        var fpv = new FirstPassVisitor();
        fpv.Visit((dynamic) ast);
        var spv = new SecondPassVisitor(fpv.FGST, fpv.VGST);
        spv.Visit((dynamic) ast);
        var watVisitor = new WatVisitor(spv.FGST);
        string result = watVisitor.Visit((dynamic) ast); // Function, Var declaration
        Assert.True(result.Contains(@"  (func $sqr"));
        Assert.True(result.Contains(@"    (param $x i32)"));
        Assert.True(result.Contains(@"    (export ""main"""));
        Assert.True(result.Contains(@"    (result i32)"));
        Assert.True(result.Contains(@"    i32.const 0"));
    }
    
}