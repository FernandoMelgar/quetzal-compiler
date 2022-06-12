using System;
using System.Collections.Generic;
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
        var watVisitor = new WatVisitor(spv.FGST, fpv.VGST);
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
        var watVisitor = new WatVisitor(spv.FGST, fpv.VGST);
        string result = watVisitor.Visit((dynamic) ast); // Function, Var declaration
        Assert.True(result.Contains(@"  (func $sqr"));
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
        var watVisitor = new WatVisitor(spv.FGST, fpv.VGST);
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
        var watVisitor = new WatVisitor(spv.FGST, fpv.VGST);
        string result = watVisitor.Visit((dynamic) ast); // Function, Var declaration
        Assert.True(result.Contains(@"  (func $sqr"));
        Assert.True(result.Contains(@"    (param $x i32)"));
        Assert.True(result.Contains(@"    (export ""main"""));
        Assert.True(result.Contains(@"    (result i32)"));
        Assert.True(result.Contains(@"    i32.const 0"));
    }

    [Test]
    public void TestGlobalVariables()
    {
        var program = @"
        var global1;
        var global2;
        sqr(x){}
        main() {
        }";
        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        var ast = parser.Program();
        var fpv = new FirstPassVisitor();
        fpv.Visit((dynamic) ast);
        var spv = new SecondPassVisitor(fpv.FGST, fpv.VGST);
        spv.Visit((dynamic) ast);
        var watVisitor = new WatVisitor(spv.FGST, fpv.VGST);
        string result = watVisitor.Visit((dynamic) ast); // Function, Var declaration
        Assert.True(result.Contains(@"  (global $global1 (mut i32) (i32.const 0))"));
        Assert.True(result.Contains(@"  (global $global2 (mut i32) (i32.const 0))"));
    }


    [Test]
    public void TestAssign()
    {
        var program = @"
        main() {
            var x;
            var y;
            x = 1;
            y = 2;
        }";
        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        var ast = parser.Program();
        var fpv = new FirstPassVisitor();
        fpv.Visit((dynamic) ast);
        var spv = new SecondPassVisitor(new Dictionary<string, ParamsFGST>(fpv.FGST), new HashSet<string>(fpv.VGST));
        spv.Visit((dynamic) ast);
        var watVisitor = new WatVisitor(spv.FGST, fpv.VGST);
        string result = watVisitor.Visit((dynamic) ast);
        Assert.True(result.Contains(@"    (local $x i32)"));
        Assert.True(result.Contains(@"    (local $y i32)"));
        Console.WriteLine(result);
        Assert.True(result.Contains("    i32.const 1\n    local.set $x\n"));
        Assert.True(result.Contains("    i32.const 2\n    local.set $y\n"));
    }

    [Test]
    public void TestAssignGlobal()
    {
        var program = @"
        var global1;
        main() {
            global1 = 10;
        }";
        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        var ast = parser.Program();
        var fpv = new FirstPassVisitor();
        fpv.Visit((dynamic) ast);
        var spv = new SecondPassVisitor(new Dictionary<string, ParamsFGST>(fpv.FGST), new HashSet<string>(fpv.VGST));
        spv.Visit((dynamic) ast);
        var watVisitor = new WatVisitor(spv.FGST, fpv.VGST);
        string result = watVisitor.Visit((dynamic) ast);
        Assert.True(result.Contains("    i32.const 10\n    global.set $global1\n"));
    }

    [Test]
    public void TestLowerThan()
    {
        var program = @"
        main() {
            if (1 < 2) {
            }
        }";
        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        var ast = parser.Program();
        var fpv = new FirstPassVisitor();
        fpv.Visit((dynamic) ast);
        var spv = new SecondPassVisitor(new Dictionary<string, ParamsFGST>(fpv.FGST), new HashSet<string>(fpv.VGST));
        spv.Visit((dynamic) ast);
        var watVisitor = new WatVisitor(spv.FGST, fpv.VGST);
        string result = watVisitor.Visit((dynamic) ast);
        Assert.True(result.Contains("    i32.const 1\n    i32.const 2\n    i32.lt_s"));
    }
}