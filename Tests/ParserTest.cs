using System.Data;
using NUnit.Framework;
using QuetzalCompiler;

namespace Tests;

public class ParserTest
{
    private TokenClassifier _classifier = new TokenClassifier();

    public ParserTest()
    {
    }

    [Test]
    public void ExpectOperationAdd()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("+").GetEnumerator());
        var validToken = parser.Expect(TokenCategory.PLUS);
        Assert.AreEqual(TokenCategory.PLUS, validToken.Category);
    }

    [Test]
    public void TestVarDefinition()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("var a1, a2;").GetEnumerator());
        parser.VarDef();
    }
    
    [Test]
    public void TestVarDefinitionButEndsWithPlusSign()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("var a1, a2+").GetEnumerator());

        var ex = Assert.Throws<SyntaxError>(() => parser.VarDef());
        Assert.AreEqual(TokenCategory.SEMICOLON,ex!.WasExpecting());
    }

    [Test]
    
    public void TestStatement()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("v1 = 2;").GetEnumerator());
        parser.VarDef();
    }
    
    


    
    
}