using Microsoft.VisualBasic.CompilerServices;

namespace QuetzalCompiler;
public class Parser
{
    private readonly IEnumerator<Token> _tokenStream;
    
    static readonly ISet<TokenCategory> firstOfStatement =
        new HashSet<TokenCategory>() {
            TokenCategory.IDENTIFIER,
            TokenCategory.INC,
            TokenCategory.DEC,
            TokenCategory.IF,
            TokenCategory.LOOP,
            TokenCategory.BREAK,
            TokenCategory.RETURN,
            TokenCategory.SEMICOLON,
        };
    
        
    static readonly ISet<TokenCategory> firstOfComparisons =
        new HashSet<TokenCategory>() {
            TokenCategory.EQUAL_COMPARISON,
            TokenCategory.NOT_EQUAL,
        };
    static readonly ISet<TokenCategory> firstOfOperationRelative =
        new HashSet<TokenCategory>() {
            TokenCategory.GREATER_THAN,
            TokenCategory.LOWER_THAN,
            TokenCategory.L_EQUAL,
            TokenCategory.G_EQUAL,
        };


    public Parser(IEnumerator<Token> tokenStream) {
        _tokenStream = tokenStream;
        _tokenStream.MoveNext();
    }

    private TokenCategory CurrentTokenCategory => _tokenStream.Current.Category;

    public Token Expect(TokenCategory category)
    {
        if (CurrentTokenCategory != category) throw new SyntaxError(category, _tokenStream.Current);
        
        var current = _tokenStream.Current;
        _tokenStream.MoveNext();
        return current;

    }

    public void Program()
    {
        
    }

    public void Definition()
    {
        switch (CurrentTokenCategory == TokenCategory.VAR)
        {
            
        }
    }

    public void VarDef()
    {
        Expect(TokenCategory.VAR);
        Id();
        while (CurrentTokenCategory == TokenCategory.COMMA) // TODO: Preguntar cómo hacer esta doble verificación
        {
            Expect(TokenCategory.COMMA);
            Id();
        }
        Expect(TokenCategory.SEMICOLON);
    }

    public void FunctionDefinition()
    {
        Id();
        Expect(TokenCategory.PAR_LEFT);
        ParamList();
        Expect(TokenCategory.PAR_RIGHT);
        Expect(TokenCategory.CURLY_LEFT);
        VarDefList();
        StmtList();
        Expect(TokenCategory.CURLY_RIGHT);
    }

    public void StmtList()
    {
        while (firstOfStatement.Contains(CurrentTokenCategory))
        {
            Stmt();
        }
    }

    public void VarDefList()
    {
        while (CurrentTokenCategory == TokenCategory.VAR) // TODO
        {
            VarDef();
        }
    }
    
    public void ParamList()
    {
        throw new NotImplementedException();
    }

    public void Id()
    {
        Expect(TokenCategory.IDENTIFIER);
    }

    public void Stmt()
    {
        switch (CurrentTokenCategory)
        {
            case TokenCategory.IDENTIFIER:
                Id();
                if (CurrentTokenCategory == TokenCategory.PAR_LEFT) // Te vas por function-call
                {
                    Expect(TokenCategory.PAR_LEFT);
                    ExprList();
                    Expect(TokenCategory.PAR_RIGHT);
                    Expect(TokenCategory.SEMICOLON);

                }
                else
                {
                    Expect(TokenCategory.ASSIGN);
                    Expr();
                    Expect(TokenCategory.SEMICOLON);

                }
                break;
            case TokenCategory.INC:
                StmtInc();
                break;
            case TokenCategory.DEC:
                StmtDec();
                break;
            case TokenCategory.IF:
                StmtIf();
                break;
            case TokenCategory.LOOP:
                StmtLoop();
                break;
            case TokenCategory.BREAK:
                StmtBreak();
                break;
            case TokenCategory.RETURN:
                StmtReturn();
                break;
            case TokenCategory.SEMICOLON:
                StmtEmpty();
                break;
            default:
                throw new SyntaxError(firstOfStatement, _tokenStream.Current);
        }
    }

    public void StmtFunCall()
    {
        FunCall();
        Expect(TokenCategory.SEMICOLON);
    }


    public void StmtEmpty()
    {
        Expect(TokenCategory.SEMICOLON);
    }

    public void StmtReturn()
    {
        Expect(TokenCategory.RETURN);
        Expr();
        Expect(TokenCategory.SEMICOLON);
    }

    public void StmtBreak()
    {
        Expect(TokenCategory.BREAK);
        Expect(TokenCategory.SEMICOLON);
    }

    public void StmtLoop()
    {
        Expect(TokenCategory.LOOP);
        Expect(TokenCategory.CURLY_LEFT);
        StmtList();
        Expect(TokenCategory.CURLY_RIGHT);
    }

    public void StmtIf()
    {
        Expect(TokenCategory.IF);
        Expect(TokenCategory.PAR_LEFT);
        Expr();
        Expect(TokenCategory.PAR_RIGHT);
        Expect(TokenCategory.CURLY_LEFT);
        StmtList();
        Expect(TokenCategory.CURLY_RIGHT);
        ElseIfList();
        Else();
    }

    public void Else()
    {
        throw new NotImplementedException();
    }

    public void ElseIfList()
    {
        throw new NotImplementedException();
    }

    public void FunCall()
    {
        Id();
        Expect(TokenCategory.PAR_LEFT);
        ExprList();
        Expect(TokenCategory.PAR_RIGHT);
    }

    public void ExprList()
    {
        throw new NotImplementedException();
    }

    public void StmtDec()
    {
        Expect(TokenCategory.DEC);
        Id();
        Expect(TokenCategory.SEMICOLON);
    }

    public void StmtInc()
    {
        Expect(TokenCategory.INC);
        Id();
        Expect(TokenCategory.SEMICOLON);
    }

    

    public void StmtAssign()
    {
        Id();
        Expect(TokenCategory.ASSIGN);
        Expr();
        Expect(TokenCategory.SEMICOLON);
    }

    public void Expr()
    {
        ExprAnd();
        while (CurrentTokenCategory == TokenCategory.OR)
        {
            Expect(TokenCategory.OR);
            ExprAnd();
        }
    }

    public void ExprAnd()
    {
        ExprComp();
        while (CurrentTokenCategory == TokenCategory.AND)
        {
            Expect(TokenCategory.AND);
            ExprComp();
        }
    }

    public void ExprComp()
    {
        ExprRel();
        do
        {
            OpComp();
            ExprRel();
        } while (firstOfComparisons.Contains(CurrentTokenCategory));
    }

    public void ExprRel()
    {
        ExprAdd();
        do
        {
            OpRel();
            ExprAdd();
        } while (firstOfComparisons.Contains(CurrentTokenCategory));
    }

    private void OpRel()
    {
        switch (CurrentTokenCategory)
        {
            case TokenCategory.GREATER_THAN:
                Expect(TokenCategory.GREATER_THAN);
                break;
            case TokenCategory.G_EQUAL:
                Expect(TokenCategory.G_EQUAL);
                break;
            case TokenCategory.LOWER_THAN:
                Expect(TokenCategory.LOWER_THAN);
                break;
            case TokenCategory.L_EQUAL:
                Expect(TokenCategory.L_EQUAL);
                break;
            default:
                throw new SyntaxError(firstOfOperationRelative, _tokenStream.Current);

        }
    }

    // ‹expr-mul› (‹op-add› ‹expr-mul›)+
    private void ExprAdd()
    {
        ExprMul();
        do
        {
            OpAdd();
            ExprMul();
        } while (CurrentTokenCategory == );
    }

    public void OpComp()
    {
        if (CurrentTokenCategory == TokenCategory.EQUAL_COMPARISON)
            Expect(TokenCategory.EQUAL_COMPARISON);
        else if (CurrentTokenCategory == TokenCategory.NOT_EQUAL)
            Expect(TokenCategory.NOT_EQUAL);
        else
            throw new SyntaxError(firstOfComparisons, _tokenStream.Current);
}