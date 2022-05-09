namespace QuetzalCompiler;
public class Parser
{
    private readonly IEnumerator<Token> _tokenStream;
    
    static readonly ISet<TokenCategory> firstOfDefinition =
        new HashSet<TokenCategory>() {
            TokenCategory.VAR,
            TokenCategory.IDENTIFIER,
        };
    
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
        
    static readonly ISet<TokenCategory> firstOfExpression =
        new HashSet<TokenCategory>() {
            TokenCategory.IDENTIFIER,
            TokenCategory.BRACKET_LEFT,
            TokenCategory.TRUE,
            TokenCategory.FALSE,
            TokenCategory.INT_LITERAL,
            TokenCategory.CHAR,
            TokenCategory.STRING,
            TokenCategory.PAR_LEFT,
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

    static readonly ISet<TokenCategory> firstOfAdditions =
        new HashSet<TokenCategory>() {
            TokenCategory.PLUS,
            TokenCategory.MINUS,
        };

    static readonly ISet<TokenCategory> firstOfMultiplications =
        new HashSet<TokenCategory>() {
            TokenCategory.MULTIPLY,
            TokenCategory.DIVIDE,
            TokenCategory.MODULE,
        };

    static readonly ISet<TokenCategory> firstOfUnary =
        new HashSet<TokenCategory>() {
            TokenCategory.PLUS,
            TokenCategory.MINUS,
            TokenCategory.NOT,
        };

    static readonly ISet<TokenCategory> firstOfLit =
        new HashSet<TokenCategory>() {
            TokenCategory.TRUE,
            TokenCategory.FALSE,
            TokenCategory.INT_LITERAL,
            TokenCategory.CHAR,
            TokenCategory.STRING,
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
           DefList(); 
    }

    public void DefList()
    {
        while (firstOfDefinition.Contains(CurrentTokenCategory))
        {
            Def();
        }
    }
    
    public void Def()
    {
        switch (CurrentTokenCategory)
        {
            case TokenCategory.VAR:
                VarDef();
                break;
            case TokenCategory.IDENTIFIER:
                FunctionDefinition();
                break;
            default:
                throw new SyntaxError(firstOfDefinition, _tokenStream.Current);
        }
    }

    public void VarDef()
    {
        Expect(TokenCategory.VAR);
        Id();
        while (CurrentTokenCategory == TokenCategory.COMMA)
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
        while (CurrentTokenCategory == TokenCategory.VAR)
        {
            VarDef();
        }
    }
    
    public void ParamList()
    {
        if (CurrentTokenCategory == TokenCategory.IDENTIFIER)
        {
            Id();
            while (CurrentTokenCategory == TokenCategory.COMMA)
            {
                Expect(TokenCategory.COMMA);
                Id();
            }
        }
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

    public void ElseIfList()
    {
        while(CurrentTokenCategory == TokenCategory.ELIF){
            Expect(TokenCategory.ELIF);
            Expect(TokenCategory.PAR_LEFT);
            Expr();
            Expect(TokenCategory.PAR_RIGHT);
            Expect(TokenCategory.CURLY_LEFT);
            StmtList();
            Expect(TokenCategory.CURLY_RIGHT);
        }
    }

    public void Else()
    {
        if (CurrentTokenCategory == TokenCategory.ELSE)
        {
            Expect(TokenCategory.ELSE);
            Expect(TokenCategory.CURLY_LEFT);
            StmtList();
            Expect(TokenCategory.CURLY_RIGHT);
        }
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
        if (firstOfExpression.Contains(CurrentTokenCategory))
        {
            Expr();
            while (CurrentTokenCategory == TokenCategory.COMMA)
            {
                Expect(TokenCategory.COMMA);
                Expr();
            }
        }
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
        while (firstOfComparisons.Contains(CurrentTokenCategory))
        {
            OpComp();
            ExprRel();
        }
    }

    public void ExprRel()
    {
        ExprAdd();
        while (firstOfOperationRelative.Contains(CurrentTokenCategory))
        {
            OpRel();
            ExprAdd();
        }
    }

    public void OpRel()
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
    public void ExprAdd()
    {
        ExprMul();
        while (firstOfAdditions.Contains(CurrentTokenCategory))
        {
            OpAdd();
            ExprMul();
        }
    }

    public void OpAdd()
    {
        switch(CurrentTokenCategory)
        {
            case TokenCategory.PLUS:
                Expect(TokenCategory.PLUS);
                break;
            case TokenCategory.MINUS:
                Expect(TokenCategory.MINUS);
                break;
            default:
                throw new SyntaxError(firstOfAdditions, _tokenStream.Current);
        }
    }

    public void ExprMul()
    {
        ExprUnary();
        while (firstOfMultiplications.Contains(CurrentTokenCategory))
        {
            OpMul();
            ExprUnary();
        } 
    }

    public Node OpMul()
    {
        switch(CurrentTokenCategory)
        {
            case TokenCategory.MULTIPLY:
                return new Multiplication()
                {
                    AnchorToken = Expect(TokenCategory.MULTIPLY)
                };
            case TokenCategory.DIVIDE:
                return new Division()
                {
                    AnchorToken = Expect(TokenCategory.DIVIDE)
                };
            case TokenCategory.MODULE:
                return new ModuleOp()
                {
                    AnchorToken = Expect(TokenCategory.MODULE)
                };
            default:
                throw new SyntaxError(firstOfMultiplications, _tokenStream.Current);
        }
    }

    public void ExprUnary()
    {
        if (firstOfUnary.Contains(CurrentTokenCategory))
            OpUnary();
        ExprPrimary();
    }

    public void ExprPrimary()
    {
        if (CurrentTokenCategory == TokenCategory.PAR_LEFT) {
            Expect(TokenCategory.PAR_LEFT);
            ExprList();
            Expect(TokenCategory.PAR_RIGHT);
        } else if (CurrentTokenCategory == TokenCategory.IDENTIFIER)
        {
            Id();
            if (CurrentTokenCategory == TokenCategory.PAR_LEFT)
            {
                Expect(TokenCategory.PAR_LEFT);
                ExprList();
                Expect(TokenCategory.PAR_RIGHT);
            }
        }
        else if (CurrentTokenCategory == TokenCategory.BRACKET_LEFT)
            Array();
        else if (firstOfLit.Contains(CurrentTokenCategory))
            Lit();
        else
            throw new SyntaxError(TokenCategory.IDENTIFIER, _tokenStream.Current);
    }

    public Node Lit()
    {
        switch(CurrentTokenCategory)
        {
            case TokenCategory.TRUE:
                return new True()
                {
                    AnchorToken = Expect(TokenCategory.TRUE)
                };
            case TokenCategory.FALSE:
                return new False()
                {
                    AnchorToken = Expect(TokenCategory.FALSE)
                };
            case TokenCategory.INT_LITERAL:
                return new Int()
                {
                    AnchorToken = Expect(TokenCategory.INT_LITERAL)
                };
            case TokenCategory.CHAR:
                return new Char()
                {
                    AnchorToken = Expect(TokenCategory.CHAR)
                };
            case TokenCategory.STRING:
                return new String()
                {
                    AnchorToken = Expect(TokenCategory.STRING)
                };
            default:
                throw new SyntaxError(firstOfLit, _tokenStream.Current);
        }
    }

    public void Array()
    {
        Expect(TokenCategory.BRACKET_LEFT);
        ExprList();
        Expect(TokenCategory.BRACKET_RIGHT);
    }

    public Node OpUnary()
    {
        switch(CurrentTokenCategory)
        {
            case TokenCategory.PLUS:
                return new Plus()
                {
                    AnchorToken = Expect(TokenCategory.PLUS)
                };
            case TokenCategory.MINUS:
                return new Minus()
                {
                    AnchorToken = Expect(TokenCategory.MINUS)
                };
            case TokenCategory.NOT:
                return new Not()
                {
                    AnchorToken = Expect(TokenCategory.NOT)
                };
            default:
                throw new SyntaxError(firstOfUnary, _tokenStream.Current);
        }
    }

    public Node OpComp()
    {
        switch (CurrentTokenCategory)
        {
            case TokenCategory.EQUAL_COMPARISON:
                return new EqualComparison()
                {
                    AnchorToken = Expect(TokenCategory.EQUAL_COMPARISON)
                };
            case TokenCategory.NOT_EQUAL:
                return new NotEqualComparison()
                {
                    AnchorToken = Expect(TokenCategory.NOT_EQUAL)
                };
            default:
                throw new SyntaxError(firstOfComparisons, _tokenStream.Current);
        }
    }
}
