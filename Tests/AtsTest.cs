using System;
using System.Formats.Asn1;
using NUnit.Framework;
using QuetzalCompiler;
using Char = QuetzalCompiler.Char;
using String = QuetzalCompiler.String;

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
    public void TestUnaryPlusOperation()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("+").GetEnumerator());
        var nodes = parser.OpUnary();
        Assert.IsInstanceOf<UnaryPlus>(nodes);
        Assert.AreEqual(TokenCategory.PLUS, nodes.AnchorToken.Category);
    }

    [Test]
    public void TestUnaryMinusOperation()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("-").GetEnumerator());
        var nodes = parser.OpUnary();
        Assert.IsInstanceOf<UnaryMinus>(nodes);
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

    [Test] 
    public void TestOperationadd()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("+").GetEnumerator());
        var nodes = parser.OpAdd();
        Assert.IsInstanceOf<Plus>(nodes);
        Assert.AreEqual(TokenCategory.PLUS, nodes.AnchorToken.Category);  
    }

    [Test]
    public void TestOperationMinus()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("-").GetEnumerator());
        var nodes = parser.OpAdd();
        Assert.IsInstanceOf<Minus>(nodes);
        Assert.AreEqual(TokenCategory.MINUS, nodes.AnchorToken.Category);  
    }

    [Test]
    public void TestOperatorGreaterThan()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable(">").GetEnumerator());
        var nodes = parser.OpRel();
        Assert.IsInstanceOf<GreaterThan>(nodes);
        Assert.AreEqual(TokenCategory.GREATER_THAN, nodes.AnchorToken.Category);
    }

    [Test]
    public void TestOperatorGreaterEqual()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable(">=").GetEnumerator());
        var nodes = parser.OpRel();
        Assert.IsInstanceOf<GreaterEqual>(nodes);
        Assert.AreEqual(TokenCategory.G_EQUAL, nodes.AnchorToken.Category);
    }
    
    
    [Test]
    public void TestOperatorLowerThan()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("<").GetEnumerator());
        var nodes = parser.OpRel();
        Assert.IsInstanceOf<LowerThan>(nodes);
        Assert.AreEqual(TokenCategory.LOWER_THAN, nodes.AnchorToken.Category);
    }
    
    
    [Test]
    public void TestOperatorLowerEqual()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("<=").GetEnumerator());
        var nodes = parser.OpRel();
        Assert.IsInstanceOf<LowerEqual>(nodes);
        Assert.AreEqual(TokenCategory.L_EQUAL, nodes.AnchorToken.Category);
    }

    [Test]
    public void TestStmtIncrease()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("inc id;").GetEnumerator());
        var nodes = parser.StmtInc();
        Assert.IsInstanceOf<StmtInc>(nodes);
        Assert.AreEqual(TokenCategory.IDENTIFIER, nodes[0].AnchorToken.Category);
    }
    
    [Test]
    public void TestStmtDecrease()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("dec id;").GetEnumerator());
        var nodes = parser.StmtDec();
        Assert.IsInstanceOf<StmtDec>(nodes);
        Assert.AreEqual(TokenCategory.IDENTIFIER, nodes[0].AnchorToken.Category);
    }


   

    [Test]
    public void TestExprUnary()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("+id").GetEnumerator());
        var nodes = parser.ExprUnary();
        Assert.IsInstanceOf<UnaryPlus>(nodes);
        Assert.AreEqual(TokenCategory.IDENTIFIER, nodes[0].AnchorToken.Category);
    }
    
    [Test]
    public void TestExprUnaryWithNoUnaryReturnsExprPrimary()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("id").GetEnumerator());
        var nodes = parser.ExprUnary();
        Assert.IsInstanceOf<Id>(nodes);
    }

    [Test]
    public void TestExprMul()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("id*id").GetEnumerator());
        var nodes = parser.ExprMul();
        Assert.IsInstanceOf<Multiplication>(nodes);
        Assert.AreEqual(TokenCategory.IDENTIFIER, nodes[0].AnchorToken.Category);
        Assert.AreEqual(TokenCategory.IDENTIFIER, nodes[1].AnchorToken.Category);
    }

    [Test]
    public void TestExprMulWithNoMulReturnsOnlyIdentifier()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("id").GetEnumerator());
        var nodes = parser.ExprMul();
        Assert.IsInstanceOf<Id>(nodes);
    }

    [Test]
    public void TestDifferentExprMulInSameExpression()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("id*id/id%id").GetEnumerator());
        var nodes = parser.ExprMul();
        Assert.IsInstanceOf<ModuleOp>(nodes);
        
        Assert.IsInstanceOf<Division>(nodes[0]);
        Assert.IsInstanceOf<Id>(nodes[1]);
        
        Assert.IsInstanceOf<Multiplication>(nodes[0][0]);
        Assert.IsInstanceOf<Id>(nodes[0][0][0]);
        Assert.IsInstanceOf<Id>(nodes[0][0][1]);

        
        
    }
    [Test]
    public void TestExprMulWithUnaryExpr()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("+id*-id").GetEnumerator());
        var nodes = parser.ExprMul();
        Assert.IsInstanceOf<Multiplication>(nodes);
        Assert.IsInstanceOf<UnaryPlus>(nodes[0]);
        Assert.IsInstanceOf<UnaryMinus>(nodes[1]);
    }
    
    [Test]
    public void TestExprAdd()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("id+id").GetEnumerator());
        var nodes = parser.ExprAdd();
        Assert.IsInstanceOf<Plus>(nodes);
        Assert.AreEqual(TokenCategory.IDENTIFIER, nodes[0].AnchorToken.Category);
        Assert.AreEqual(TokenCategory.IDENTIFIER, nodes[1].AnchorToken.Category);
    }
    
    [Test]
    public void TestExprAddWithNoAddReturnsOnlyId()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("id").GetEnumerator());
        var nodes = parser.ExprAdd();
        Assert.IsInstanceOf<Id>(nodes);
        Assert.AreEqual(0, nodes.CountChildren());
    }

    [Test]
    public void TestExprAddWithUnaryAndMultiplication()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("id*id2+id*id3").GetEnumerator());
        var nodes = parser.ExprAdd();
        Assert.IsInstanceOf<Plus>(nodes);
        Assert.IsInstanceOf<Multiplication>(nodes[0]);
        Assert.IsInstanceOf<Multiplication>(nodes[1]);
    }

    [Test]
    public void TestMultipleExprAddInSame()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("id + id - id - id").GetEnumerator());
        var nodes = parser.ExprAdd();
        Assert.IsInstanceOf<Minus>(nodes);
        
        Assert.IsInstanceOf<Minus>(nodes[0]);
        Assert.IsInstanceOf<Id>(nodes[1]);
        
        Assert.IsInstanceOf<Plus>(nodes[0][0]);
        Assert.IsInstanceOf<Id>(nodes[0][0][0]);
        Assert.IsInstanceOf<Id>(nodes[0][0][1]);
    }
    [Test]
    public void TestExprRel()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("id > id2").GetEnumerator());
        var nodes = parser.ExprRel();
        Assert.IsInstanceOf<GreaterThan>(nodes);
        Assert.IsInstanceOf<Id>(nodes[0]);
        Assert.IsInstanceOf<Id>(nodes[1]);
    }

    [Test]
    public void TestDifferentExprRelInSameExpression()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("id > id2 > id3").GetEnumerator());
        var nodes = parser.ExprRel();
        Assert.IsInstanceOf<GreaterThan>(nodes);
        Assert.IsInstanceOf<GreaterThan>(nodes[0]);
        Assert.IsInstanceOf<Id>(nodes[1]);
        Assert.IsInstanceOf<Id>(nodes[0][0]);
        Assert.IsInstanceOf<Id>(nodes[0][1]);
    }

    [Test]
    public void TestDifferentExprRelInSameExpression2()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("id < id2 >= id3").GetEnumerator());
        var nodes = parser.ExprRel();
        Assert.IsInstanceOf<GreaterEqual>(nodes);
        Assert.IsInstanceOf<LowerThan>(nodes[0]);
        Assert.IsInstanceOf<Id>(nodes[1]);
        Assert.IsInstanceOf<Id>(nodes[0][0]);
        Assert.IsInstanceOf<Id>(nodes[0][1]);
    }


    [Test]
    public void TestExprComparison()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("id == id").GetEnumerator());
        var nodes = parser.ExprComp();
        Assert.IsInstanceOf<EqualComparison>(nodes);
        Assert.IsInstanceOf<Id>(nodes[0]);
        Assert.IsInstanceOf<Id>(nodes[1]);

    }
    [Test]
    public void TestDifferentExprComparison()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("id == id != id").GetEnumerator());
        var nodes = parser.ExprComp();
        Assert.IsInstanceOf<NotEqualComparison>(nodes);
        Assert.IsInstanceOf<EqualComparison>(nodes[0]);
        Assert.IsInstanceOf<Id>(nodes[1]);
        Assert.IsInstanceOf<Id>(nodes[0][0]);
        Assert.IsInstanceOf<Id>(nodes[0][1]);

    }

    [Test]
    public void TestExprAnd()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("id and id2").GetEnumerator());
        var nodes = parser.ExprAnd();
        Assert.IsInstanceOf<And>(nodes);
        Assert.IsInstanceOf<Id>(nodes[0]);
        Assert.IsInstanceOf<Id>(nodes[1]);
    }
    
    [Test]
    public void TestExprAndMultiple()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("id and id2 and id3 and id4").GetEnumerator());
        var nodes = parser.ExprAnd();
        Assert.IsInstanceOf<And>(nodes);
        Assert.IsInstanceOf<Id>(nodes[0]);
        Assert.IsInstanceOf<Id>(nodes[1]);
        Assert.IsInstanceOf<Id>(nodes[2]);
        Assert.IsInstanceOf<Id>(nodes[3]);
    }

    [Test]
    public void TestExpr()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("id or id2").GetEnumerator());
        var nodes = parser.Expr();
        Assert.IsInstanceOf<Or>(nodes);
        Assert.IsInstanceOf<Id>(nodes[0]);
        Assert.IsInstanceOf<Id>(nodes[1]);
    }

    [Test]
    public void TestExprList()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("id, id*id, id - id, id / id, +id").GetEnumerator());
        var nodes = parser.ExprList();
        Assert.IsInstanceOf<ExprList>(nodes);
        Assert.IsInstanceOf<Id>(nodes[0]);
        Assert.IsInstanceOf<Multiplication>(nodes[1]);
        Assert.IsInstanceOf<Minus>(nodes[2]);
        Assert.IsInstanceOf<Division>(nodes[3]);
        Assert.IsInstanceOf<UnaryPlus>(nodes[4]);
        

    }

    [Test]
    public void TestFunctionCall()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("print(id, id*id, id - id, id / id, +id)").GetEnumerator());
        var nodes = parser.FunCall();
        Assert.IsInstanceOf<FunCall>(nodes);
        Assert.IsInstanceOf<Id>(nodes[0]);
        Assert.IsInstanceOf<ExprList>(nodes[1]);
    }

    [Test]
    public void TestVarDef()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("var id, id, id;").GetEnumerator());
        var nodes = parser.VarDef();
        Assert.IsInstanceOf<VarDef>(nodes);
        Assert.IsInstanceOf<Id>(nodes[0]);
        Assert.IsInstanceOf<Id>(nodes[1]);
        Assert.IsInstanceOf<Id>(nodes[2]);

    }

    [Test]
    public void TestParamList()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("id, id , id, id").GetEnumerator());
        var nodes = parser.ParamList();
        Assert.IsInstanceOf<ParamList>(nodes);
        Assert.IsInstanceOf<Id>(nodes[0]);
        Assert.IsInstanceOf<Id>(nodes[1]);
        Assert.IsInstanceOf<Id>(nodes[2]);
        Assert.IsInstanceOf<Id>(nodes[3]);
   
    }

    [Test]
    public void TestStmtReturnStmtInc()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("inc id;").GetEnumerator());
        var nodes = parser.Stmt();
        Assert.IsInstanceOf<StmtInc>(nodes);
    }
    
    [Test]
    public void TestStmtReturnStmtDec()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("dec id;").GetEnumerator());
        var nodes = parser.Stmt();
        Assert.IsInstanceOf<StmtDec>(nodes);
    }

    [Test]
    public void TestStmtBreak()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("break;").GetEnumerator());
        var nodes = parser.Stmt();
        Assert.IsInstanceOf<StmtBreak>(nodes);
    }    
    [Test]
    public void TestStmtReturn()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("return id;").GetEnumerator());
        var nodes = parser.Stmt();
        Assert.IsInstanceOf<StmtReturn>(nodes);
    }

    [Test]
    public void TestStmtEmptyDoesNotReturnNode()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable(";").GetEnumerator());
        var nodes = parser.Stmt();  
        Assert.IsInstanceOf<StmtEmpty>(nodes);

    }


    [Test]
    public void TestFunctionCallInStmt()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("println(id, id);").GetEnumerator());
        var nodes = parser.Stmt();
        Assert.IsInstanceOf<FunCall>(nodes);

    }

    [Test]
    public void TestAssignStmt()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("id = id2;").GetEnumerator());
        var nodes = parser.Stmt();
        Assert.IsInstanceOf<Assign>(nodes); 
    }


    [Test]
    public void TestStmtIf()
    {
        var program = @"if (value1 != value2) {
                inc fails;
                prints(id);
                prints(message);
                println();
            } elif (val3 > val2) {
                println(val4);
            } else {
                println(val1);
            }";
        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        var nodes = parser.StmtIf();
        Assert.IsInstanceOf<If>(nodes);
        Assert.IsInstanceOf<NotEqualComparison>(nodes[0]);
        Assert.IsInstanceOf<StmtList>(nodes[1]);
        Assert.IsInstanceOf<ElifList>(nodes[2]);
        Assert.IsInstanceOf<Else>(nodes[3]);
        

    }

    
    public void TestStmtList() // TODO: Ignore
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable(@"dec id;").GetEnumerator());
        var nodes = parser.StmtList();
        Assert.IsInstanceOf<StmtList>(nodes);
        Assert.IsInstanceOf<StmtDec>(nodes[0]);
        Assert.IsInstanceOf<StmtInc>(nodes[1]);

    }
    [Test]
    public void TestElse()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("else { dec id; inc id;}").GetEnumerator());
        var nodes = parser.Else();
        Assert.IsInstanceOf<Else>(nodes);
        Assert.IsInstanceOf<StmtList>(nodes[0]);
    }

    [Test]
    public void TestElseIf()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("elif (id == id2) { dec id; inc id;} elif (id != id2) { dec id; inc id;}").GetEnumerator());
        var nodes = parser.ElseIfList();
        Assert.IsInstanceOf<ElifList>(nodes);
        Assert.IsInstanceOf<Elif>(nodes[0]);
        Assert.IsInstanceOf<EqualComparison>(nodes[0][0]);
        Assert.IsInstanceOf<StmtList>(nodes[0][1]);
        
        Assert.IsInstanceOf<Elif>(nodes[1]);
        Assert.IsInstanceOf<NotEqualComparison>(nodes[1][0]);
        Assert.IsInstanceOf<StmtList>(nodes[1][1]);
    }


    [Test]
    public void TestOnlyIdArray()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("[id, id, id, id]").GetEnumerator());
        var nodes = parser.Array();
        Assert.IsInstanceOf<ExprList>(nodes);
        Assert.IsInstanceOf<Id>(nodes[0]);
        Assert.IsInstanceOf<Id>(nodes[1]); 
        Assert.IsInstanceOf<Id>(nodes[2]); 
        Assert.IsInstanceOf<Id>(nodes[3]); 
    }
    
    [Test]
    public void TestOnlyIdArrayButWithExprPrimary()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("[id, id, id, id]").GetEnumerator());
        var nodes = parser.ExprPrimary();
        Assert.IsInstanceOf<ExprList>(nodes);
        Assert.IsInstanceOf<Id>(nodes[0]);
        Assert.IsInstanceOf<Id>(nodes[1]); 
        Assert.IsInstanceOf<Id>(nodes[2]); 
        Assert.IsInstanceOf<Id>(nodes[3]); 
    }

    [Test]
    public void TestLiteralButWithExprPrimary()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("4").GetEnumerator());
        var nodes = parser.ExprPrimary();
        Assert.IsInstanceOf<Int>(nodes);
    }
    [Test]
    public void TestStringButWithExprPrimary()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("\"Hello world\"").GetEnumerator());
        var nodes = parser.ExprPrimary();
        Assert.IsInstanceOf<String>(nodes);
    }

    [Test]
    public void TestCharButWithExprPrimary()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("\'c\'").GetEnumerator());
        var nodes = parser.ExprPrimary();
        Assert.IsInstanceOf<Char>(nodes); 
    }
    
    [Test]
    public void TestFalseButWithExprPrimary()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("false").GetEnumerator());
        var nodes = parser.ExprPrimary();
        Assert.IsInstanceOf<False>(nodes); 
    }
    
    [Test]
    public void TestTrueButWithExprPrimary()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("true").GetEnumerator());
        var nodes = parser.ExprPrimary();
        Assert.IsInstanceOf<True>(nodes); 
    }

    [Test]
    public void TestExprPrimaryFunctionCall()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("print(id1)").GetEnumerator());
        var nodes = parser.ExprPrimary();
        Assert.IsInstanceOf<FunCall>(nodes); 
    }

    [Test]
    public void TestExprListFunctionCall()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("(false, 1, (false))").GetEnumerator());
        var nodes = parser.ExprPrimary();
        Assert.IsInstanceOf<ExprList>(nodes); 
        Assert.IsInstanceOf<False>(nodes[0]); 
        Assert.IsInstanceOf<Int>(nodes[1]); 
        Assert.IsInstanceOf<ExprList>(nodes[2]); 
        Assert.IsInstanceOf<False>(nodes[2][0]); 
    }

    [Test]
    public void TestStmtLoop()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("loop  {println(); println(); }").GetEnumerator());
        var nodes = parser.StmtLoop();
        Assert.IsInstanceOf<Loop>(nodes); 
        Assert.IsInstanceOf<StmtList>(nodes[0]);

    }

    [Test]
    public void TestLoopAsStmt()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("loop  {println(); println(); }").GetEnumerator());
        var nodes = parser.Stmt();
        Assert.IsInstanceOf<Loop>(nodes); 
        Assert.IsInstanceOf<StmtList>(nodes[0]);   
    }

    [Test]
    public void TestIfAsStmt()
    {
        var program = @"if (value1 != value2) {
                inc fails;
                prints(id);
                prints(message);
                println();
            } elif (val3 > val2) {
                println(val4);
            } else {
                println(val1);
            }";
        var parser = new Parser(_classifier.ClassifyAsEnumerable(program).GetEnumerator());
        var nodes = parser.Stmt();
        Assert.IsInstanceOf<If>(nodes);
        Assert.IsInstanceOf<NotEqualComparison>(nodes[0]);
        Assert.IsInstanceOf<StmtList>(nodes[1]);
        Assert.IsInstanceOf<ElifList>(nodes[2]);
        Assert.IsInstanceOf<Else>(nodes[3]);
    }

    [Test]
    public void TestVarDefList()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("var id; var id2; var id3;").GetEnumerator());
        var nodes = parser.VarDefList();
        Assert.IsInstanceOf<VarDefList>(nodes);
        Assert.IsInstanceOf<VarDef>(nodes[0]);
        Assert.IsInstanceOf<VarDef>(nodes[1]);
        Assert.IsInstanceOf<VarDef>(nodes[2]);

    }

    [Test]
    public void TestFunctionDefinition()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("main(){funCall(functionCall2);}").GetEnumerator());
        var nodes = parser.FunctionDefinition();
        Assert.IsInstanceOf<Function>(nodes);
        Assert.IsInstanceOf<Id>(nodes[0]);
        Assert.IsInstanceOf<ParamList>(nodes[1]);
        Assert.IsInstanceOf<VarDefList>(nodes[2]);
        Assert.IsInstanceOf<StmtList>(nodes[3]);
        
    }
    [Test]
    public void TestFunctionDefinitionButFromDev()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("main(){funCall(functionCall2);}").GetEnumerator());
        var nodes = parser.Def();
        Assert.IsInstanceOf<Function>(nodes);
        Assert.IsInstanceOf<Id>(nodes[0]);
        Assert.IsInstanceOf<ParamList>(nodes[1]);
        Assert.IsInstanceOf<VarDefList>(nodes[2]);
        Assert.IsInstanceOf<StmtList>(nodes[3]);
        
    }

    [Test]
    public void TestVarDefButFromDef()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("var id, id, id;").GetEnumerator());
        var nodes = parser.Def();
        Assert.IsInstanceOf<VarDef>(nodes);
        Assert.IsInstanceOf<Id>(nodes[0]);
        Assert.IsInstanceOf<Id>(nodes[1]);
        Assert.IsInstanceOf<Id>(nodes[2]);

    }

    [Test]
    public void TestDefList()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("var id, id, id; main(){funCall(functionCall2);}").GetEnumerator());
        var nodes = parser.DefList();
        Assert.IsInstanceOf<DefList>(nodes);
        Assert.IsInstanceOf<VarDef>(nodes[0]);
        Assert.IsInstanceOf<Function>(nodes[1]);
    }
    
    
    [Test]
    public void TestDefListButAsProgram()
    {
        var parser = new Parser(_classifier.ClassifyAsEnumerable("var id, id, id; main(){funCall(functionCall2);}").GetEnumerator());
        var nodes = parser.Program();
        Assert.IsInstanceOf<DefList>(nodes);
        Assert.IsInstanceOf<VarDef>(nodes[0]);
        Assert.IsInstanceOf<Function>(nodes[1]);
    }
    
    //TODO Distinguir entre FunctionCall de stmt y de expr
}

