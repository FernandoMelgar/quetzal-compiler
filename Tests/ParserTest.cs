using NUnit.Framework;
using NUnit.Framework.Internal;
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
    public void TestSimpleFunctionStructure()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable(@"assert(value1, value2, message) { }")
            .GetEnumerator());
        parser.Program();
        

    }

    [Test]
    public void TestClassifyExpressionPrimaryId()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("id1").GetEnumerator());
        parser.ExprPrimary();
    }

    [Test]
    public void TestClassifyFunctionCall()
    {
        
    }

    [Test]
    public void TestLiteral()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("true").GetEnumerator());
        parser.Lit();

    }

    [Test]
    public void TestLiteralButWithExpressionPrimary()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("true").GetEnumerator());
        parser.ExprPrimary();
        var parser2 = new Parser(_classifier.ClassifyAsEnumerable("false").GetEnumerator());
        parser2.ExprPrimary();
    }

    [Test]
    public void TestLiteralChar()
    {
        var parser3 = new Parser(_classifier.ClassifyAsEnumerable("\'N\'").GetEnumerator());
        parser3.Lit();
    }

    [Test]
    public void TestFunctionCall()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("main(){funCall(functionCall2);}").GetEnumerator());
        parser.Program();
    }

    [Test]
    public void TestExpressionUnary()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("main(){id = true; x = not id;}").GetEnumerator());
        parser.Program();
    }

    [Test]
    public void TestAssertSyntaxErrorWithDoubleOpUnary()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("main(){id = true; x = not not id;}").GetEnumerator());
        var ex = Assert.Throws<SyntaxError>(() => parser.Program());
        Assert.AreEqual(TokenCategory.IDENTIFIER,ex!.WasExpecting());
    }

    [Test]
    public void TestIfFunction()
    {
        var program = @"assert(value1, value2, message) {
            if (value1 != value2) {
                inc fails;
                prints(""Assertion failure: "");
                prints(message);
                println();
            }";
        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        parser.Program();
    }

    [Test]
    public void TestExpresionMultiplication()
    {
        var program = @"main(){var a2; a2 = 4 * (4 * 2);}";
        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        parser.Program();
    }

    [Test]
    public void TestOperationAdd()
    {
        var program = @"main(){var a2; a2 = 4 + (4 * (2 / 4));}";
        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        parser.Program();   
    }
    [Test]
    public void TestOperationAddOnlyAdds()
    {
        var program = @"main(){var a2; a2 = 1 + 2 + 3 + 4 + 5;}";
        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        parser.Program();   
    }

    [Test]
    public void TestOperationRel()
    {
        var program = @"main(){var a2; a2 = 10 < 2 + (2 / 6 );}";
        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        parser.Program();   
    }

    [Test]
    public void assingValueOfFunctionResultToVariable()
    {
        var program = @"main(){a1 = range(1, 10);}";
        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        parser.Program();   
    }

}