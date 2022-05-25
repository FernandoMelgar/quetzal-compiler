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
}