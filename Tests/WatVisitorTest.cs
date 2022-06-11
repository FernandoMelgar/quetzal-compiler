using NUnit.Framework;
using QuetzalCompiler;
using QuetzalCompiler.Visitor;

namespace Tests;

public class WatVisitorTest
{
    private readonly TokenClassifier _classifier = new();

    [Test]
    public void TestCreateInstanceWatVisitor()
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
        var watVisitor = new WatVisitor(spv.VGST);
        string result = watVisitor.Visit(ast); // Function, Var declaration
        Assert.True(result.Contains("(local $x i32)"));
    }
}