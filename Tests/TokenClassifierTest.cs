using NUnit.Framework;
using QuetzalCompiler;
namespace Tests;
/*
 * Authors:
 *   - A01748354: Fernando Manuel Melgar Fuentes
 *   - A01376364: Alex Serrano Durán
 */
public class TokenClassifierTest
{
    private TokenClassifier _classifier = new TokenClassifier();


    [Test]
    public void TestClassifyReturn()
    {
        var tokenizedInput = _classifier.classify("return");
        
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("return", token.Value.Lexeme);
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
        Assert.AreEqual("true", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.TRUE, token.Value.Category);

    }

    [Test]
    public void ClassifyFalse()
    {
        var tokenizedInput = _classifier.classify("false");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("false", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.FALSE, token.Value.Category);
    }

    [Test]
    public void ClassifyAnd()
    {
        var tokenizedInput = _classifier.classify("and");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("and", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.AND, token.Value.Category);
        
    }

    [Test]
    public void ClassifyOr()
    {
        var tokenizedInput = _classifier.classify("or");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("or", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.OR, token.Value.Category);
    }

    [Test]
    public void ClassifyBreak()
    {
        var tokenizedInput = _classifier.classify("break");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("break", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.BREAK, token.Value.Category);
    }

    [Test]
    public void ClassifyIf()
    {
        var tokenizedInput = _classifier.classify("if");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("if", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.IF, token.Value.Category);   
    }

    [Test]
    public void ClassifyDec()
    {
        var tokenizedInput = _classifier.classify("dec");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("dec", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.DEC, token.Value.Category);     
    }

    [Test]
    public void ClassifyInc()
    {
        var tokenizedInput = _classifier.classify("inc");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("inc", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.INC, token.Value.Category);
    }

    [Test]
    public void ClassifyElif()
    {
        var tokenizedInput = _classifier.classify("elif");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("elif", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.ELIF, token.Value.Category);
    }

    [Test]
    public void ClassifyLoop()
    {
        var tokenizedInput = _classifier.classify("loop");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("loop", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.LOOP, token.Value.Category);   
    }

    [Test]
    public void ClassifyVar()
    {
        var tokenizedInput = _classifier.classify("var");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("var", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.VAR, token.Value.Category);   
    }

    [Test]
    public void ClassifyElse()
    {
        var tokenizedInput = _classifier.classify("else");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("else", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.ELSE, token.Value.Category);   
    }

    [Test]
    public void ClassifyNot()
    {
        var tokenizedInput = _classifier.classify("not");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("not", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.NOT, token.Value.Category);   
    }

    [Test]
    public void ClassifyAssignSign()
    {
        var tokenizedInput = _classifier.classify("=");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("=", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.ASSIGN, token.Value.Category);   
    }

    [Test]
    public void ClassifyEqualsComparison()
    {
        var tokenizedInput = _classifier.classify("==");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("==", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.EQUAL_COMPARISON, token.Value.Category);    
    }

    [Test]
    public void ClassifyNotEqualComparison()
    {
        var tokenizedInput = _classifier.classify("!=");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("!=", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.NOT_EQUAL, token.Value.Category);   
    }

    [Test]
    public void ClassifyLowerEqual()
    {
        var tokenizedInput = _classifier.classify("<=");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("<=", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.L_EQUAL, token.Value.Category);  
    }
    
    [Test]
    public void ClassifyUpperEqual()
    {
        var tokenizedInput = _classifier.classify(">=");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual(">=", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.G_EQUAL, token.Value.Category);  
    }
    
    
    [Test]
    public void ClassifyGreaterThan()
    {
        var tokenizedInput = _classifier.classify(">");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual(">", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.GREATER_THAN, token.Value.Category);  
    }

    [Test]
    public void ClassifyLowerThan()
    {
        var tokenizedInput = _classifier.classify("<");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("<", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.LOWER_THAN, token.Value.Category);  
    }

    [Test]
    public void ClassifyPlus()
    {
        var tokenizedInput = _classifier.classify("+");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("+", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.PLUS, token.Value.Category);  
    }

    [Test]
    public void ClassifyMinus()
    {
        var tokenizedInput = _classifier.classify("-");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("-", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.MINUS, token.Value.Category);  
    }

    [Test]
    public void ClassifyMultiplication()
    {
        var tokenizedInput = _classifier.classify("*");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("*", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.MULTIPLY, token.Value.Category);  
    }

    [Test]
    public void ClassifyDivide()
    {
        var tokenizedInput = _classifier.classify("/");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("/", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.DIVIDE, token.Value.Category);    
    }

    [Test]
    public void ClassifyModule()
    {
        var tokenizedInput = _classifier.classify("%");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("%", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.MODULE, token.Value.Category);    
    }
    [Test]
    public void ClassifySemicolon()
    {
        var tokenizedInput = _classifier.classify(";");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual(";", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.SEMICOLON, token.Value.Category);    
    }
    
    [Test]
    public void ClassifyManySemicolons()
    {
        var tokenizedInput = _classifier.classify(";;;;;;;;;;");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual(";;;;;;;;;;", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.SEMICOLON, token.Value.Category);    
    }

    [Test]
    public void ClassifyParenthesisLeft()
    {
        var tokenizedInput = _classifier.classify("(");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("(", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.PAR_LEFT, token.Value.Category);    
    }

    [Test]
    public void ClassifyParenthesisRight()
    {
        var tokenizedInput = _classifier.classify(")");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual(")", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.PAR_RIGHT, token.Value.Category);    
    }

    [Test]
    public void ClassifyCurlyLeft()
    {
        var tokenizedInput = _classifier.classify("{");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("{", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.CURLY_LEFT, token.Value.Category);       
    }

    [Test]
    public void ClassifyCurlyRight()
    {
        var tokenizedInput = _classifier.classify("}");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("}", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.CURLY_RIGHT, token.Value.Category);   
    }

    [Test]
    public void ClassifyBracketLeft()
    {
        var tokenizedInput = _classifier.classify("[");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("[", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.BRACKET_LEFT, token.Value.Category);
    }

    [Test]
    public void ClassifyBracketRight()
    {
        var tokenizedInput = _classifier.classify("]");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("]", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.BRACKET_RIGHT, token.Value.Category);
    }

    [Test]
    public void ClassifyIdentifier()
    {
        var tokenizedInput = _classifier.classify("public");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("public", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.IDENTIFIER, token.Value.Category);
    }

    [Test]
    public void ClassifyVarInitialization()
    {
        var tokenizedInput = _classifier.classify("var velocity = 10");
        Assert.AreEqual(4, tokenizedInput.Count);
        
        Assert.AreEqual("var",tokenizedInput.First?.Value.Lexeme);
        Assert.AreEqual(TokenCategory.VAR, tokenizedInput.First?.Value.Category);
        
        Assert.AreEqual("velocity", tokenizedInput.First?.Next?.Value.Lexeme);
        Assert.AreEqual(TokenCategory.IDENTIFIER, tokenizedInput.First?.Next?.Value.Category);
        
        Assert.AreEqual("=", tokenizedInput.First?.Next?.Next?.Value.Lexeme);
        Assert.AreEqual(TokenCategory.ASSIGN, tokenizedInput.First?.Next?.Next?.Value.Category);

        Assert.AreEqual("10", tokenizedInput.First?.Next?.Next?.Next?.Value.Lexeme);
        Assert.AreEqual(TokenCategory.INT_LITERAL, tokenizedInput.First?.Next?.Next?.Next?.Value.Category);
    }

    [Test]
    public void ClassifyIntLiteral()
    {
        var tokenizedInput = _classifier.classify("1234");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("1234", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.INT_LITERAL, token.Value.Category);
    }

    [Test]
    public void IgnoreBlockComments()
    {
        var tokenizedInput = _classifier.classify("/* Hello darkness \n My \n old fried *//**/");
        Assert.AreEqual(0, tokenizedInput.Count);
        
    }

    [Test]
    public void IgnoreBlockCommentButInLine()
    {
        var tokenizedInput = _classifier.classify("/* Hello darkness My old fried */");
        Assert.AreEqual(0, tokenizedInput.Count);
    }
    
    
    
    [Test]
    public void IgnoreLineComment()
    {
        var tokenizedInput = _classifier.classify("// Hellou \n");
        Assert.AreEqual(0, tokenizedInput.Count);

    }

    [Test]
    public void IgnoreLineCommentButWithNoLineBreak()
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
        Assert.AreEqual("\"Hello, world\"", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.STRING, token.Value.Category);
    }

    [Test]
    public void ClassifyComma()
    {
        var tokenizedInput = _classifier.classify(",");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual(",", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.COMMA, token.Value.Category);
    }

    [Test]
    public void ClassifySingleQuoteAsChar()
    {
        var tokenizedInput = _classifier.classify("\'N\'");
        Assert.AreEqual(1, tokenizedInput.Count);

        var token = tokenizedInput.First;
        Assert.AreEqual("\'N\'", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.CHAR, token.Value.Category);
    }

    [Test]
    public void ClassifyScapeChar()
    {
        var tokenizedInput = _classifier.classify("\'\\n\'"); 
        Assert.AreEqual(1, tokenizedInput.Count);

        var token = tokenizedInput.First;
        Assert.AreEqual("\'\\n\'", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.CHAR, token.Value.Category);
    }

    [Test]
    public void DontClassifyCharWithLenghtOverOneIfAreNotSpecialChars()
    {
        var tokenizedInput = _classifier.classify("\'NN\'");
        Assert.AreEqual(3, tokenizedInput.Count); 
        
        Assert.AreEqual("'",tokenizedInput.First?.Value.Lexeme);
        Assert.AreEqual(TokenCategory.ILLEGAL_CHAR, tokenizedInput.First?.Value.Category);
        
        Assert.AreEqual("NN", tokenizedInput.First?.Next?.Value.Lexeme);
        Assert.AreEqual(TokenCategory.IDENTIFIER, tokenizedInput.First?.Next?.Value.Category);
        
        Assert.AreEqual("'", tokenizedInput.First?.Next?.Next?.Value.Lexeme);
        Assert.AreEqual(TokenCategory.ILLEGAL_CHAR, tokenizedInput.First?.Next?.Next?.Value.Category);
    }

    [Test]
    public void ClassifyUnicodeHEX()
    {
        var tokenizedInput = _classifier.classify("'\\uFFFFFF'");
        Assert.AreEqual(1, tokenizedInput.Count);

        Assert.AreEqual("'\\uFFFFFF'",tokenizedInput.First?.Value.Lexeme);
        Assert.AreEqual(TokenCategory.CHAR, tokenizedInput.First?.Value.Category);
    }

    [Test]
    public void ClassifyUnicodeDigits()
    {
        var tokenizedInput = _classifier.classify("'\\u111222'");
        Assert.AreEqual(1, tokenizedInput.Count);

        Assert.AreEqual("'\\u111222'",tokenizedInput.First?.Value.Lexeme);
        Assert.AreEqual(TokenCategory.CHAR, tokenizedInput.First?.Value.Category);
    }

    [Test]
    public void ClassifyUnicodeMix()
    {
        var tokenizedInput = _classifier.classify("'\\u111FFF'");
        Assert.AreEqual(1, tokenizedInput.Count);

        Assert.AreEqual("'\\u111FFF'",tokenizedInput.First?.Value.Lexeme);
        Assert.AreEqual(TokenCategory.CHAR, tokenizedInput.First?.Value.Category);
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
        Assert.AreEqual("reverse", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.IDENTIFIER, token.Value.Category);
        Assert.AreEqual(6, token.Value.Row);

    }

    [Test]
    public void ClassifyCharNewLine()
    {
        var tokenizedInput = _classifier.classify("\'N\'");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("\'N\'", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.CHAR, token.Value.Category);
    }

    [Test]
    public void ClassifyIdentifierWithUnderscoreAndNumbers()
    {
        var tokenizedInput = _classifier.classify("identy_fier300");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("identy_fier300", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.IDENTIFIER, token.Value.Category);
    }

    [Test]
    public void DontClassifyIdentifierWithSpecialCharacters()
    {
        var tokenizedInput1 = _classifier.classify("iden'ty_fier300");
        Assert.AreNotEqual(1, tokenizedInput1.Count);
    }

    [Test]
    public void DontClassifyIdentifersThatStartWithUnderscore()
    {
        var tokenizedInput1 = _classifier.classify("_identy_fier300");
        Assert.AreNotEqual(1, tokenizedInput1.Count);

    }

    [Test]
    public void ClassifyNegativeInteger()
    {
        var tokenizedInput = _classifier.classify("-1000");
        Assert.AreEqual(1, tokenizedInput.Count);
        
        var token = tokenizedInput.First;
        Assert.AreEqual("-1000", token.Value.Lexeme);
        Assert.AreEqual(TokenCategory.INT_LITERAL, token.Value.Category);
    }

    [Test]
    public void DontClassifyWithHardCodedNewLine()
    {
        var tokenizedInput = _classifier.classify(@"
");
        Assert.AreEqual(0, tokenizedInput.Count);
    }

    [Test]
    public void ClassifySingleQuoteWithScapeAsChar()
    {
        var tokenizedInput = _classifier.classify("'\''");
        Assert.AreEqual(1, tokenizedInput.Count);

        Assert.AreEqual("'\''",tokenizedInput.First?.Value.Lexeme);
        Assert.AreEqual(TokenCategory.CHAR, tokenizedInput.First?.Value.Category);
        
    }

    [Test]
    public void ClassifyStringWithScape()
    {
        var tokenizedInput = _classifier.classify("\"Hola\\nmundo\"");
        Assert.AreEqual(1, tokenizedInput.Count);

        Assert.AreEqual("\"Hola\\nmundo\"",tokenizedInput.First?.Value.Lexeme);
        Assert.AreEqual(TokenCategory.STRING, tokenizedInput.First?.Value.Category);
    }

    [Test]
    public void DontClassifyStringWithIllegalScapeSequence()
    {
        var tokenizedInput = _classifier.classify("\"Hola\\Rmundo\"");
        Assert.AreEqual(5, tokenizedInput.Count);
    }
}