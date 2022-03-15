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

    [Test]
    public void ClassifyLowerThan()
    {
        var tokenizedInput = _classifier.classify("<");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("<", token.Value.lexeme);
        Assert.AreEqual(TokenCategory.LOWER_THAN, token.Value.Category);  
    }

    [Test]
    public void ClassifyPlus()
    {
        var tokenizedInput = _classifier.classify("+");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("+", token.Value.lexeme);
        Assert.AreEqual(TokenCategory.PLUS, token.Value.Category);  
    }

    [Test]
    public void ClassifyMinus()
    {
        var tokenizedInput = _classifier.classify("-");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("-", token.Value.lexeme);
        Assert.AreEqual(TokenCategory.MINUS, token.Value.Category);  
    }

    [Test]
    public void ClassifyMultiplication()
    {
        var tokenizedInput = _classifier.classify("*");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("*", token.Value.lexeme);
        Assert.AreEqual(TokenCategory.MULTIPLY, token.Value.Category);  
    }

    [Test]
    public void ClassifyDivide()
    {
        var tokenizedInput = _classifier.classify("/");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("/", token.Value.lexeme);
        Assert.AreEqual(TokenCategory.DIVIDE, token.Value.Category);    
    }

    [Test]
    public void ClassifyModule()
    {
        var tokenizedInput = _classifier.classify("%");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("%", token.Value.lexeme);
        Assert.AreEqual(TokenCategory.MODULE, token.Value.Category);    
    }
    [Test]
    public void ClassifySemicolon()
    {
        var tokenizedInput = _classifier.classify(";");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual(";", token.Value.lexeme);
        Assert.AreEqual(TokenCategory.SEMICOLON, token.Value.Category);    
    }

    [Test]
    public void ClassifyParenthesisLeft()
    {
        var tokenizedInput = _classifier.classify("(");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("(", token.Value.lexeme);
        Assert.AreEqual(TokenCategory.PAR_LEFT, token.Value.Category);    
    }

    [Test]
    public void ClassifyParenthesisRight()
    {
        var tokenizedInput = _classifier.classify(")");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual(")", token.Value.lexeme);
        Assert.AreEqual(TokenCategory.PAR_RIGHT, token.Value.Category);    
    }

    [Test]
    public void ClassifyCurlyLeft()
    {
        var tokenizedInput = _classifier.classify("{");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("{", token.Value.lexeme);
        Assert.AreEqual(TokenCategory.CURLY_LEFT, token.Value.Category);       
    }

    [Test]
    public void ClassifyCurlyRight()
    {
        var tokenizedInput = _classifier.classify("}");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("}", token.Value.lexeme);
        Assert.AreEqual(TokenCategory.CURLY_RIGHT, token.Value.Category);   
    }

    [Test]
    public void ClassifyBracketLeft()
    {
        var tokenizedInput = _classifier.classify("[");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("[", token.Value.lexeme);
        Assert.AreEqual(TokenCategory.BRACKET_LEFT, token.Value.Category);
    }

    [Test]
    public void ClassifyBracketRight()
    {
        var tokenizedInput = _classifier.classify("]");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("]", token.Value.lexeme);
        Assert.AreEqual(TokenCategory.BRACKET_RIGHT, token.Value.Category);
    }

    [Test]
    public void ClassifyIdentifier()
    {
        var tokenizedInput = _classifier.classify("public");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("public", token.Value.lexeme);
        Assert.AreEqual(TokenCategory.IDENTIFIER, token.Value.Category);
    }

    [Test]
    public void ClassifyVarInitialization()
    {
        var tokenizedInput = _classifier.classify("var velocity = 10");
        Assert.AreEqual(4, tokenizedInput.Count);
        
        Assert.AreEqual("var",tokenizedInput.First?.Value.lexeme);
        Assert.AreEqual(TokenCategory.VAR, tokenizedInput.First?.Value.Category);
        
        Assert.AreEqual("velocity", tokenizedInput.First?.Next?.Value.lexeme);
        Assert.AreEqual(TokenCategory.IDENTIFIER, tokenizedInput.First?.Next?.Value.Category);
        
        Assert.AreEqual("=", tokenizedInput.First?.Next?.Next?.Value.lexeme);
        Assert.AreEqual(TokenCategory.ASSIGN, tokenizedInput.First?.Next?.Next?.Value.Category);

        Assert.AreEqual("10", tokenizedInput.First?.Next?.Next?.Next?.Value.lexeme);
        Assert.AreEqual(TokenCategory.INT_LITERAL, tokenizedInput.First?.Next?.Next?.Next?.Value.Category);
    }

    [Test]
    public void ClassifyIntLiteral()
    {
        var tokenizedInput = _classifier.classify("1234");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("1234", token.Value.lexeme);
        Assert.AreEqual(TokenCategory.INT_LITERAL, token.Value.Category);
    }

    [Test]
    public void IgnoreBlockComments()
    {
        var tokenizedInput = _classifier.classify("/* Hello darkness \n My old fried */ /**/");
        Assert.AreEqual(0, tokenizedInput.Count);
        
    }

    [Test]
    public void IgnoreLineComment()
    {
        var tokenizedInput = _classifier.classify("// Hellou \n");
        Assert.AreEqual(0, tokenizedInput.Count);

    }

    [Test]
    public void ClassifyString()
    {
        var tokenizedInput = _classifier.classify("\"Hello, world\"");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("\"Hello, world\"", token.Value.lexeme);
        Assert.AreEqual(TokenCategory.STRING, token.Value.Category);
    }

    [Test]
    public void ClassifyComma()
    {
        var tokenizedInput = _classifier.classify(",");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual(",", token.Value.lexeme);
        Assert.AreEqual(TokenCategory.COMMA, token.Value.Category);
    }

    [Test]
    public void ClassifySingleQuoteAsChar()
    {
        var tokenizedInput = _classifier.classify("\'N\'");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("\'N\'", token.Value.lexeme);
        Assert.AreEqual(TokenCategory.CHAR, token.Value.Category);
    }

    [Test]
    public void CountBlockCommentsLines()
    {
        var tokenizedInput = _classifier.classify(@"/* File: 002_binary.quetzal
            Converts decimal numbers into binary.
            (C) 2022 Ariel Ortiz, ITESM CEM
            */

            reverse");

        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("reverse", token.Value.lexeme);
        Assert.AreEqual(TokenCategory.IDENTIFIER, token.Value.Category);
        Assert.AreEqual(6, token.Value.Row);

    }

}