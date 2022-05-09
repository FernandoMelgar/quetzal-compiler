using NUnit.Framework;
using QuetzalCompiler;

namespace Tests;

public class AtsTest
{
    
    private TokenClassifier _classifier = new TokenClassifier();
    
    [Test]
    public void TestOperationComparison()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("==").GetEnumerator());
        var nodes = parser.OpComp();
        Assert.IsInstanceOf<EqualComparison>(nodes);
        Assert.AreEqual(TokenCategory.EQUAL_COMPARISON, nodes.AnchorToken.Category);
    }

    [Test]
    public void TestNotEqualComparison()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("!=").GetEnumerator());
        var nodes = parser.OpComp();
        Assert.IsInstanceOf<NotEqualComparison>(nodes);
        Assert.AreEqual(TokenCategory.NOT_EQUAL, nodes.AnchorToken.Category);
    }

    [Test]
    public void TestPlusOperation()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("+").GetEnumerator());
        var nodes = parser.OpUnary();
        Assert.IsInstanceOf<Plus>(nodes);
        Assert.AreEqual(TokenCategory.PLUS, nodes.AnchorToken.Category);
    }

    [Test]
    public void TestMinusOperation()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("-").GetEnumerator());
        var nodes = parser.OpUnary();
        Assert.IsInstanceOf<Minus>(nodes);
        Assert.AreEqual(TokenCategory.MINUS, nodes.AnchorToken.Category);
    }

    [Test]
    public void TestNotOperator()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("not").GetEnumerator());
        var nodes = parser.OpUnary();
        Assert.IsInstanceOf<Not>(nodes);
        Assert.AreEqual(TokenCategory.NOT, nodes.AnchorToken.Category);
    }

    [Test]
    public void TestTrue()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("true").GetEnumerator());
        var nodes = parser.Lit();
        Assert.IsInstanceOf<True>(nodes);
        Assert.AreEqual(TokenCategory.TRUE, nodes.AnchorToken.Category);
    }

    [Test]
    public void TestFalse()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("false").GetEnumerator());
        var nodes = parser.Lit();
        Assert.IsInstanceOf<False>(nodes);
        Assert.AreEqual(TokenCategory.FALSE, nodes.AnchorToken.Category); 
    }

    [Test]
    public void TestIntLiteral()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("123456").GetEnumerator());
        var nodes = parser.Lit();
        Assert.IsInstanceOf<Int>(nodes);
        Assert.AreEqual(TokenCategory.INT_LITERAL, nodes.AnchorToken.Category); 
    }

    [Test]
    public void TestChar()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("\'c\'").GetEnumerator());
        var nodes = parser.Lit();
        Assert.IsInstanceOf<Char>(nodes);
        Assert.AreEqual(TokenCategory.CHAR, nodes.AnchorToken.Category); 
    }

    [Test]
    public void TestString()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("\"Hello world\"").GetEnumerator());
        var nodes = parser.Lit();
        Assert.IsInstanceOf<String>(nodes);
        Assert.AreEqual(TokenCategory.STRING, nodes.AnchorToken.Category); 
    }

    [Test]
    public void TestMultiply()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("*").GetEnumerator());
        var nodes = parser.OpMul();
        Assert.IsInstanceOf<Multiplication>(nodes);
        Assert.AreEqual(TokenCategory.MULTIPLY, nodes.AnchorToken.Category); 
    }

    [Test]
    public void TestDivision()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("/").GetEnumerator());
        var nodes = parser.OpMul();
        Assert.IsInstanceOf<Division>(nodes);
        Assert.AreEqual(TokenCategory.DIVIDE, nodes.AnchorToken.Category); 
    }

    [Test]
    public void TesModuleOperator()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("%").GetEnumerator());
        var nodes = parser.OpMul();
        Assert.IsInstanceOf<ModuleOp>(nodes);
        Assert.AreEqual(TokenCategory.MODULE, nodes.AnchorToken.Category); 
    }
}