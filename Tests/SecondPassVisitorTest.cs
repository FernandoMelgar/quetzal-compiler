using System;
using System.Linq;
using NUnit.Framework;
using QuetzalCompiler;
using QuetzalCompiler.Visitor;
/*
 * Authors:
 *   - A01748354: Fernando Manuel Melgar Fuentes
 *   - A01376364: Alex Serrano Durán
 */
namespace Tests;

public class SecondPassVisitorTest
{
    private readonly TokenClassifier _classifier = new();

    [Test]
    public void TestDoesNotAddGlobalVarsSecondTime()
    {
        var program = @"var id; sqrt(k, n) {} main() {}";
        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        var programNode = parser.Program();
        var fpv = new FirstPassVisitor();
        var spv = new SecondPassVisitor(fpv.FGST, fpv.VGST);
        fpv.Visit((dynamic)programNode);
        spv.Visit((dynamic)programNode);
    }

    [Test]
    public void TestWrongNumberOfParamsOnFunCall()
    {
        var program = @"sqrt() {}
        main() {
            var x;
            x = 10;
            sqrt(x);
        }";

        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        var programNode = parser.Program();
        var fpv = new FirstPassVisitor();
        var spv = new SecondPassVisitor(fpv.FGST, fpv.VGST);
        fpv.Visit((dynamic)programNode);
        try
        {
            spv.Visit((dynamic)programNode);
        }
        catch (SemanticError e)
        {
            Assert.IsTrue(e.Message.Contains(" Incorrect number of params for function: 'sqrt', should be 0 not 1"));
        }
    }

    [Test]
    public void FunctionIsNotCreated()
    {
        var program = @"main() {
            var x;
            x = 10;
            sqrt(x);
        }";

        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        var programNode = parser.Program();
        var fpv = new FirstPassVisitor();
        var spv = new SecondPassVisitor(fpv.FGST, fpv.VGST);
        fpv.Visit((dynamic)programNode);
        try
        {
            spv.Visit((dynamic)programNode);
        }
        catch (SemanticError e)
        {
            Assert.IsTrue(e.Message.Contains(@"Function 'sqrt' Is not Created"));
        }
    }

    [Test]
    public void TestVarDeclarationFoundTwice()
    {
        var program = @"main() {
            var x, x;
            x = 10;
        }";

        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        var programNode = parser.Program();
        var fpv = new FirstPassVisitor();
        var spv = new SecondPassVisitor(fpv.FGST, fpv.VGST);
        fpv.Visit((dynamic)programNode);
        try
        {
            spv.Visit((dynamic)programNode);
        }
        catch (SemanticError e)
        {
            Assert.IsTrue(e.Message.Contains("Var declaration found twice"));
        }
    }

    [Test]
    public void TestVarDeclarationFoundTwiceSeparated()
    {
        var program = @"main() {
            var x;
            var x;
            x = 10;
        }";

        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        var programNode = parser.Program();
        var fpv = new FirstPassVisitor();
        var spv = new SecondPassVisitor(fpv.FGST, fpv.VGST);
        fpv.Visit((dynamic)programNode);
        try
        {
            spv.Visit((dynamic)programNode);
        }
        catch (SemanticError e)
        {
            Assert.IsTrue(e.Message.Contains("Var declaration found twice: x"));
        }
    }

    [Test]
    public void TestIncorrectBreakUsage()
    {
        var program = @"main() {
            var x;
            x = 10;
            break;
        }";

        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        var programNode = parser.Program();
        var fpv = new FirstPassVisitor();
        var spv = new SecondPassVisitor(fpv.FGST, fpv.VGST);
        fpv.Visit((dynamic)programNode);
        try
        {
            spv.Visit((dynamic)programNode);
        }
        catch (SemanticError e)
        {
            Assert.IsTrue(e.Message.Contains("Incorrect usage of break statement"));
        }
    }

    [Test]
    public void TestAssignsToNonExistentVar()
    {
        var program = @"main() {
            x = 10;
        }";

        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        var programNode = parser.Program();
        var fpv = new FirstPassVisitor();
        var spv = new SecondPassVisitor(fpv.FGST, fpv.VGST);
        fpv.Visit((dynamic)programNode);
        try
        {
            spv.Visit((dynamic)programNode);
        }
        catch (SemanticError e)
        {
            Assert.IsTrue(e.Message.Contains("Undeclared variable: x"));
        }
    }

    [Test]
    public void TestVarIsNotInScope()
    {
        var program = @"sqrt(x) {
            var y;
        }
        main() {
            y = 3;
        }";

        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        var programNode = parser.Program();
        var fpv = new FirstPassVisitor();
        var spv = new SecondPassVisitor(fpv.FGST, fpv.VGST);
        fpv.Visit((dynamic)programNode);
        try
        {
            spv.Visit((dynamic)programNode);
        }
        catch (SemanticError e)
        {
            Assert.IsTrue(e.Message.Contains("Semantic Error: Undeclared variable: y "));
        }
    }
    
    [Test]
    public void TestEqualComparison()
    {
        var program = @"main() {
            var x;
            x = 1;
            if(x == 6 > 3){
                x = 1;
            }
        }";

        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        var programNode = parser.Program();
        var fpv = new FirstPassVisitor();
        var spv = new SecondPassVisitor(fpv.FGST, fpv.VGST);
        fpv.Visit((dynamic) programNode);
        spv.Visit((dynamic) programNode);
    }
    
    [Test]
    public void TestNotComparison()
    {
        var program = @"main() {
            var x;
            if(not x){
                x = 1;
            }
        }";

        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        var programNode = parser.Program();
        var fpv = new FirstPassVisitor();
        var spv = new SecondPassVisitor(fpv.FGST, fpv.VGST);
        fpv.Visit((dynamic) programNode);
        spv.Visit((dynamic) programNode);
    }
    
    [Test]
    public void TestIntTooLarge()
    {
        var program = @"main() {
            var x;
            x = 99999999999999999999999999999999999999999999999999999999999999;
        }";

        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        var programNode = parser.Program();
        var fpv = new FirstPassVisitor();
        var spv = new SecondPassVisitor(fpv.FGST, fpv.VGST);
        fpv.Visit((dynamic) programNode);
        try
        {
            spv.Visit((dynamic)programNode);
        }
        catch (SemanticError e)
        {
            Assert.IsTrue(e.Message.Contains("Integer literal too large:"));
        }
    }

    [Test]
    public void TestMultilineVariableDeclaration()
    {
        var program = @"main() {
            var x;
            var i;
            var j;
            var k;
        }";
        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        var programNode = parser.Program();
        var fpv = new FirstPassVisitor();
        fpv.Visit((dynamic) programNode);
        var spv = new SecondPassVisitor(fpv.FGST, fpv.VGST);
        spv.Visit((dynamic) programNode);
        var refLst = spv.FGST["main"].refLST;
        Assert.AreEqual(4, refLst.Count);
        Assert.AreEqual("x", refLst.ToList()[0]);
        Assert.AreEqual("i", refLst.ToList()[1]);
        Assert.AreEqual("j", refLst.ToList()[2]);
        Assert.AreEqual("k", refLst.ToList()[3]);
        
    }
    
    
    [Test]
    public void TestMultilineVariableDeclaration2()
    {
        var program = @"main() {
            var x;
            var i, j, k;
            var l;
        }";
        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        var programNode = parser.Program();
        var fpv = new FirstPassVisitor();
        fpv.Visit((dynamic) programNode);
        var spv = new SecondPassVisitor(fpv.FGST, fpv.VGST);
        spv.Visit((dynamic) programNode);
        var refLst = spv.FGST["main"].refLST;
        Assert.AreEqual(5, refLst.Count);
        Assert.AreEqual("x", refLst.ToList()[0]);
        Assert.AreEqual("i", refLst.ToList()[1]);
        Assert.AreEqual("j", refLst.ToList()[2]);
        Assert.AreEqual("k", refLst.ToList()[3]);
        Assert.AreEqual("l", refLst.ToList()[4]);
    }

    [Test]
    public void TestDeclareSameLineVgst()
    {
        var program = @"
        var global1, global2, global3;
        main() {
        }";
        
        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        var programNode = parser.Program();
        var fpv = new FirstPassVisitor();
        fpv.Visit((dynamic) programNode);
        Assert.AreEqual(3, fpv.VGST.Count);
    }
}