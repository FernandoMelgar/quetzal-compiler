using System;
using System.Linq;
using NUnit.Framework;
using QuetzalCompiler;
using QuetzalCompiler.Visitor;

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
        fpv.Visit((dynamic) programNode);
        spv.Visit((dynamic) programNode);
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
        fpv.Visit((dynamic) programNode);
        try
        {
            spv.Visit((dynamic) programNode);
        }
        catch (SemanticError e)
        {
            Assert.IsTrue(e.Message.Contains(" Incorrect number of params for function: 'sqrt', should be 0 not 1"));
        }
    }
}