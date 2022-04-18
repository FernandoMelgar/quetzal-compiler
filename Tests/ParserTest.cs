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

    
    
}