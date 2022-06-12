using System.Linq;
using NUnit.Framework;
using QuetzalCompiler;
using QuetzalCompiler.Visitor;
/*
 * Authors:
 *   - A01748354: Fernando Manuel Melgar Fuentes
 *   - A01376364: Alex Serrano Durán
 */
namespace Tests;

public class FirstPassVisitorTest
{
    private readonly TokenClassifier _classifier = new();

    [Test]
    public void CreateFirstPassVisitorUsesParser()
    {
        var program = @"var id; main() {}";
        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        var programNode = parser.Program();
        var fpv = new FirstPassVisitor();
        fpv.Visit((dynamic) programNode);
        Assert.Contains("id", fpv.VGST.ToList());
        Assert.Contains("main", fpv.FGST.Keys.ToList());
    }
    
    [Test]
    public void TestThrowErrorIfVariableIsTwice()
    {
        var program = @"var id; var id; main() {}";
        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        var node = parser.Program();
        var fpv = new FirstPassVisitor();
        Assert.Throws<SemanticError>(() => fpv.Visit((dynamic) node));
    }
    
    [Test]
    public void TestThrowErrorIfFunctionDeclarationIsTwice()
    {
        var program = @"var id; var id; main() {} main() {}";
        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        var node = parser.Program();
        var fpv = new FirstPassVisitor();
        Assert.Throws<SemanticError>(() => fpv.Visit((dynamic) node));
    }
    
    [Test]
    public void TestThrowErrorIfMainHasParams()
    {
        var program = @"var id; main(id){}";
        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        var node = parser.Program();
        var fpv = new FirstPassVisitor();
        Assert.Throws<SemanticError>(() => fpv.Visit((dynamic) node));
    }
    
    [Test]
    public void TestThrowErrorIfFunctionDeclarationIsTwiceWithDifferentArities() 
    {
        var program = @"var id; sum(x, y){} sum (x){} main() {}";
        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        var node = parser.Program();
        var fpv = new FirstPassVisitor();
        Assert.Throws<SemanticError>(() => fpv.Visit((dynamic) node));
    }

    [Test] public void TestThrowErrorIfMainIsNotDeclared()
    {
        var program = @"var id;";
        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        var node = parser.Program();
        var fpv = new FirstPassVisitor();
        Assert.Throws<SemanticError>(() => fpv.Visit((dynamic) node));
    }


}