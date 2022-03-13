using NUnit.Framework;
using QuetzalCompiler;
namespace Tests;

public class TokenClassifierTest
{
    private TokenClassifier _classifier = new TokenClassifier();


    [Test]
    public void TestClassifyReturn()
    {
        var tokenizedInput = _classifier.classify("return");
        
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("return", token.Value.lexeme);
        Assert.AreEqual(TokenCategory.RETURN, token.Value.Category);
    }

    [Test]
    public void TestIgnoreWhiteSpace()
    {
        var tokenizedInput = _classifier.classify("   ");
        Assert.AreEqual(0, tokenizedInput.Count);
    }

    [Test]
    public void TestIgnoreNewLine()
    {
        var tokenizedInput = _classifier.classify("\n");
        Assert.AreEqual(0, tokenizedInput.Count);
    }

    [Test]
    public void TestClassifyTrue()
    {
        var tokenizedInput = _classifier.classify("true");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("true", token.Value.lexeme);
        Assert.AreEqual(TokenCategory.TRUE, token.Value.Category);

    }

    [Test]
    public void ClassifyFalse()
    {
        var tokenizedInput = _classifier.classify("false");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("false", token.Value.lexeme);
        Assert.AreEqual(TokenCategory.FALSE, token.Value.Category);
    }

    [Test]
    public void ClassifyAnd()
    {
        var tokenizedInput = _classifier.classify("and");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("and", token.Value.lexeme);
        Assert.AreEqual(TokenCategory.AND, token.Value.Category);
        
    }

    [Test]
    public void DontClassifyAND()
    {
        var tokenizedInput = _classifier.classify("AND");
        Assert.AreEqual(0, tokenizedInput.Count);
    }

    [Test]
    public void ClassifyOr()
    {
        var tokenizedInput = _classifier.classify("or");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("or", token.Value.lexeme);
        Assert.AreEqual(TokenCategory.OR, token.Value.Category);
    }

    [Test]
    public void ClassifyBreak()
    {
        var tokenizedInput = _classifier.classify("break");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("break", token.Value.lexeme);
        Assert.AreEqual(TokenCategory.BREAK, token.Value.Category);
    }

    [Test]
    public void ClassifyIf()
    {
        var tokenizedInput = _classifier.classify("if");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("if", token.Value.lexeme);
        Assert.AreEqual(TokenCategory.IF, token.Value.Category);   
    }

    [Test]
    public void ClassifyDec()
    {
        var tokenizedInput = _classifier.classify("dec");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("dec", token.Value.lexeme);
        Assert.AreEqual(TokenCategory.DEC, token.Value.Category);     
    }

    [Test]
    public void ClassifyInc()
    {
        var tokenizedInput = _classifier.classify("inc");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("inc", token.Value.lexeme);
        Assert.AreEqual(TokenCategory.INC, token.Value.Category);
    }

    [Test]
    public void ClassifyElif()
    {
        var tokenizedInput = _classifier.classify("elif");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("elif", token.Value.lexeme);
        Assert.AreEqual(TokenCategory.ELIF, token.Value.Category);
    }

    [Test]
    public void ClassifyLoop()
    {
        var tokenizedInput = _classifier.classify("loop");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("loop", token.Value.lexeme);
        Assert.AreEqual(TokenCategory.LOOP, token.Value.Category);   
    }

    [Test]
    public void ClassifyVar()
    {
        var tokenizedInput = _classifier.classify("var");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("var", token.Value.lexeme);
        Assert.AreEqual(TokenCategory.VAR, token.Value.Category);   
    }

    [Test]
    public void ClassifyElse()
    {
        var tokenizedInput = _classifier.classify("else");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("else", token.Value.lexeme);
        Assert.AreEqual(TokenCategory.ELSE, token.Value.Category);   
    }

    [Test]
    public void ClassifyNot()
    {
        var tokenizedInput = _classifier.classify("not");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("not", token.Value.lexeme);
        Assert.AreEqual(TokenCategory.NOT, token.Value.Category);   
    }

    [Test]
    public void ClassifyAssignSign()
    {
        var tokenizedInput = _classifier.classify("=");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("=", token.Value.lexeme);
        Assert.AreEqual(TokenCategory.ASSIGN, token.Value.Category);   
    }

    [Test]
    public void ClassifyEqualsComparison()
    {
        var tokenizedInput = _classifier.classify("==");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("==", token.Value.lexeme);
        Assert.AreEqual(TokenCategory.EQUAL_COMPARISON, token.Value.Category);    
    }

    [Test]
    public void ClassifyNotEqualComparison()
    {
        var tokenizedInput = _classifier.classify("!=");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("!=", token.Value.lexeme);
        Assert.AreEqual(TokenCategory.NOT_EQUAL, token.Value.Category);   
    }

    [Test]
    public void ClassifyLowerEqual()
    {
        var tokenizedInput = _classifier.classify("<=");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("<=", token.Value.lexeme);
        Assert.AreEqual(TokenCategory.L_EQUAL, token.Value.Category);  
    }
    
    [Test]
    public void ClassifyUpperEqual()
    {
        var tokenizedInput = _classifier.classify(">=");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual(">=", token.Value.lexeme);
        Assert.AreEqual(TokenCategory.G_EQUAL, token.Value.Category);  
    }
    
    
    [Test]
    public void ClassifyGreaterThan()
    {
        var tokenizedInput = _classifier.classify(">");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual(">", token.Value.lexeme);
        Assert.AreEqual(TokenCategory.GREATER_THAN, token.Value.Category);  
    }
    
    
    
}