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
        Assert.True(
            result.Contains("    i32.const 1\n    i32.const 2\n    i32.lt_s\n    if\n    end\n    i32.const 0"));
    }

    [Test]
    public void TestLowerEqualThan()
    {
        var program = @"
        main() {
            if (1 <= 2) {
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
        Assert.True(
            result.Contains("    i32.const 1\n    i32.const 2\n    i32.le_s\n    if\n    end\n    i32.const 0"));
    }

    [Test]
    public void TestGreaterEqualThan()
    {
        var program = @"
        main() {
            if (1 >= 2) {
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
        Assert.True(
            result.Contains("    i32.const 1\n    i32.const 2\n    i32.ge_s\n    if\n    end\n    i32.const 0"));
    }

    [Test]
    public void TestGreaterThan()
    {
        var program = @"
        main() {
            if (1 > 2) {
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
        Assert.True(
            result.Contains("    i32.const 1\n    i32.const 2\n    i32.gt_s\n    if\n    end\n    i32.const 0"));
    }

    [Test]
    public void TestEqual()
    {
        var program = @"
        main() {
            if (1 == 2) {
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
        Assert.True(result.Contains("    i32.const 1\n    i32.const 2\n    i32.eq\n    if\n    end\n    i32.const 0"));
    }

    [Test]
    public void TestNotEqual()
    {
        var program = @"
        main() {
            if (1 != 2) {
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
        Assert.True(result.Contains("    i32.const 1\n    i32.const 2\n    i32.ne\n    if\n    end\n    i32.const 0"));
    }

    [Test]
    public void TestStmtInc()
    {
        var program = @"
        main() {
            var x;
            x = 0;
            inc x;
        }";
        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        var ast = parser.Program();
        var fpv = new FirstPassVisitor();
        fpv.Visit((dynamic) ast);
        var spv = new SecondPassVisitor(new Dictionary<string, ParamsFGST>(fpv.FGST), new HashSet<string>(fpv.VGST));
        spv.Visit((dynamic) ast);
        var watVisitor = new WatVisitor(spv.FGST, fpv.VGST);
        string result = watVisitor.Visit((dynamic) ast);
        Assert.True(result.Contains("    i32.const 1\n    local.get $x\n    $i32.add\n    local.set $x"));
    }

    [Test]
    public void TestStmtDec()
    {
        var program = @"
        main() {
            var x;
            x = 0;
            dec x;
        }";
        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        var ast = parser.Program();
        var fpv = new FirstPassVisitor();
        fpv.Visit((dynamic) ast);
        var spv = new SecondPassVisitor(new Dictionary<string, ParamsFGST>(fpv.FGST), new HashSet<string>(fpv.VGST));
        spv.Visit((dynamic) ast);
        var watVisitor = new WatVisitor(spv.FGST, fpv.VGST);
        string result = watVisitor.Visit((dynamic) ast);
        Assert.True(result.Contains("    local.get $x\n    i32.const 1\n    $i32.sub\n    local.set $x"));
    }

    [Test]
    public void TestAssignNegativeNumber()
    {
        var program = @"
        main() {
            var x;
            x = -99;
        }";
        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        var ast = parser.Program();
        var fpv = new FirstPassVisitor();
        fpv.Visit((dynamic) ast);
        var spv = new SecondPassVisitor(new Dictionary<string, ParamsFGST>(fpv.FGST), new HashSet<string>(fpv.VGST));
        spv.Visit((dynamic) ast);
        var watVisitor = new WatVisitor(spv.FGST, fpv.VGST);
        string result = watVisitor.Visit((dynamic) ast);
        Assert.True(result.Contains("    i32.const 0\n    i32.const 99\n    i32.sub\n    local.set $x"));
    }

    [Test]
    public void TestNot()
    {
        var program = @"
        main() {
            var x;
            x = not 2;
        }";
        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        var ast = parser.Program();
        var fpv = new FirstPassVisitor();
        fpv.Visit((dynamic) ast);
        var spv = new SecondPassVisitor(new Dictionary<string, ParamsFGST>(fpv.FGST), new HashSet<string>(fpv.VGST));
        spv.Visit((dynamic) ast);
        var watVisitor = new WatVisitor(spv.FGST, fpv.VGST);
        string result = watVisitor.Visit((dynamic) ast);
        Assert.True(result.Contains("    i32.const 2\n    i32.eqz\n    local.set $x"));
    }


    [Test]
    public void TestLoop()
    {
        var program = @"
        main() {
            var x;
            var y;
            loop {
                inc x;
                inc y;
                break;
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
        Assert.True(result.Contains(@"    (local $x i32)
    (local $y i32)
    block $00001
        loop $00001
            
            "));
    }


    [Test]
    public void TestGenerateLabel()
    {
        var watVisitor = new WatVisitor(new Dictionary<string, ParamsFGST>(), new HashSet<string>());
        Assert.AreEqual("$00000", watVisitor.GenerateLabel());
        Assert.AreEqual("$00001", watVisitor.GenerateLabel());
        Assert.AreEqual("$00002", watVisitor.GenerateLabel());
        Assert.AreEqual("$00003", watVisitor.GenerateLabel());
        Assert.AreEqual("$00004", watVisitor.GenerateLabel());
        Assert.AreEqual("$00005", watVisitor.GenerateLabel());
        Assert.AreEqual("$00006", watVisitor.GenerateLabel());
        Assert.AreEqual("$00007", watVisitor.GenerateLabel());
        Assert.AreEqual("$00008", watVisitor.GenerateLabel());
        Assert.AreEqual("$00009", watVisitor.GenerateLabel());
        Assert.AreEqual("$00010", watVisitor.GenerateLabel());
    }


    [Test]
    public void TestElse()
    {
        var program = @"
        main() {
            var x;
            if (1 < 2) {
                x = 3;
            }else{
                x = 4;
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
        Assert.True(result.Contains("    else\n    i32.const 4\n    local.set $x\n    end\n    i32.const 0"));
    }

    [Test]
    public void TestElseIf()
    {
        var program = @"
        main() {
            var x;
            if (1 < 2) {
                x = 3;
            }elif (4 < 5){
                x = 6;
            }else{
                x = 7;
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
        Assert.True(result.Contains("    else\n    i32.const 4\n    i32.const 5\n    i32.lt_s\n"));
        Assert.True(result.Contains("    if\n    i32.const 6\n    local.set $x\n"));
        Assert.True(result.Contains("    else\n    i32.const 7\n    local.set $x\n    end\n    end\n    i32.const 0"));
    }

    [Test]
    public void TestIndentation()
    {
        var watVisitor = new WatVisitor(new Dictionary<string, ParamsFGST>(), new HashSet<string>());
        Assert.AreEqual("", watVisitor._t);
        watVisitor.IncreaseIndentation();
        Assert.AreEqual("  ", watVisitor._t);
        watVisitor.IncreaseIndentation();
        watVisitor.IncreaseIndentation();
        watVisitor.IncreaseIndentation();
        Assert.AreEqual("        ", watVisitor._t);
        watVisitor.DecreaseIndentation();
        watVisitor.DecreaseIndentation();
        Assert.AreEqual("    ", watVisitor._t);
        watVisitor.DecreaseIndentation();
        watVisitor.DecreaseIndentation();
        Assert.AreEqual("", watVisitor._t);
        watVisitor.DecreaseIndentation();
        Assert.AreEqual("", watVisitor._t);
    }
}