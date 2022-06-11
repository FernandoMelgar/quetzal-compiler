using System.Collections.Generic;

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

    public Node Program()
    {
           return DefList(); 
    }

    public Node DefList()
    {
        var defList = new DefList();
        while (firstOfDefinition.Contains(CurrentTokenCategory))
        {
            defList.Add(Def());
        }

        return defList;
    }
    
    public Node Def()
    {
        switch (CurrentTokenCategory)
        {
            case TokenCategory.VAR:
                return VarDef();
            case TokenCategory.IDENTIFIER:
                return FunctionDefinition();
            default:
                throw new SyntaxError(firstOfDefinition, _tokenStream.Current);
        }
    }

    public Node VarDef()
    {
        Expect(TokenCategory.VAR);
        var varDef = new VarDef() {Id()};
        while (CurrentTokenCategory == TokenCategory.COMMA)
        {
            Expect(TokenCategory.COMMA);
            varDef.Add(Id());
        }
        Expect(TokenCategory.SEMICOLON);
        return varDef;
    }

    public Node FunctionDefinition()
    {
        var id = Id();
        Expect(TokenCategory.PAR_LEFT);
        var paramList = ParamList();
        Expect(TokenCategory.PAR_RIGHT);
        Expect(TokenCategory.CURLY_LEFT);
        var varDefList = VarDefList();
        var stmtList = StmtList();
        Expect(TokenCategory.CURLY_RIGHT);
        return new Function() {id, paramList, varDefList, stmtList};
    }

    public Node StmtList()
    {
        var stmtList = new StmtList();
        while (firstOfStatement.Contains(CurrentTokenCategory))
        {
            stmtList.Add(Stmt());
        }

        return stmtList;
    }

    public Node VarDefList()
    {
        var varDefList = new VarDefList();
        while (CurrentTokenCategory == TokenCategory.VAR)
        {
            varDefList.Add(VarDef());
        }
        return varDefList;
    }
    
    public Node ParamList()
    {
        var paramList = new ParamList();
        if (CurrentTokenCategory == TokenCategory.IDENTIFIER)
        {
            paramList.Add(Id());
            while (CurrentTokenCategory == TokenCategory.COMMA)
            {
                Expect(TokenCategory.COMMA);
                paramList.Add(Id());
            }
        }

        return paramList;
    }

    public Node Id()
    {
        return new Id()
        {
            AnchorToken = Expect(TokenCategory.IDENTIFIER)
        };
    }

    public Node Stmt()
    {
        switch (CurrentTokenCategory)
        {
            case TokenCategory.IDENTIFIER:
                var id = Id();
                if (CurrentTokenCategory == TokenCategory.PAR_LEFT) // Te vas por function-call
                {
                    Expect(TokenCategory.PAR_LEFT);
                    var funCall = new FunCall() {id, ExprList()};
                    Expect(TokenCategory.PAR_RIGHT);
                    Expect(TokenCategory.SEMICOLON);
                    return funCall;

                }
                else
                {
                    Expect(TokenCategory.ASSIGN);
                    var assign = new Assign() {id, Expr()};
                    Expect(TokenCategory.SEMICOLON);
                    return assign;
                }
            case TokenCategory.INC:
                return StmtInc();
            case TokenCategory.DEC:
                return StmtDec();
            case TokenCategory.IF:
                return StmtIf();
            case TokenCategory.LOOP:
                return StmtLoop();
            case TokenCategory.BREAK:
                return StmtBreak();
            case TokenCategory.RETURN:
                return StmtReturn();
            case TokenCategory.SEMICOLON:
                return StmtEmpty(); // TODO: How to handle this case?
            default:
                throw new SyntaxError(firstOfStatement, _tokenStream.Current);
        }
        return new NotImplementedNode();
    }
    
    public Node StmtEmpty()
    {
        return new StmtEmpty() {AnchorToken = Expect(TokenCategory.SEMICOLON)};
    }

    public Node StmtReturn()
    {
        var returnStmt =  new StmtReturn()
        {
            AnchorToken = Expect(TokenCategory.RETURN)
        };
        returnStmt.Add(Expr());
        Expect(TokenCategory.SEMICOLON);
        return returnStmt;
    }

    public Node StmtBreak()
    {
        var stmBreak = Expect(TokenCategory.BREAK);
        Expect(TokenCategory.SEMICOLON);
        return new StmtBreak()
        {
            AnchorToken = stmBreak
        };
    }

    public Node StmtLoop()
    {
        var stmtLoop  = new Loop()
        {
            AnchorToken = Expect(TokenCategory.LOOP)
        };
        Expect(TokenCategory.CURLY_LEFT);
         stmtLoop.Add(StmtList());
        Expect(TokenCategory.CURLY_RIGHT);
        return stmtLoop;
    }

    public Node StmtIf()
    {
        var stmtIf = new If();
        Expect(TokenCategory.IF);
        Expect(TokenCategory.PAR_LEFT);
        stmtIf.Add(Expr());
        Expect(TokenCategory.PAR_RIGHT);
        Expect(TokenCategory.CURLY_LEFT);
        stmtIf.Add(StmtList());
        Expect(TokenCategory.CURLY_RIGHT);
        stmtIf.Add(ElseIfList());
        stmtIf.Add(Else());
        return stmtIf;
    }

    public Node ElseIfList()
    {
        var elifList = new ElifList();
        while(CurrentTokenCategory == TokenCategory.ELIF)
        {
            var elif = new Elif();
            Expect(TokenCategory.ELIF);
            Expect(TokenCategory.PAR_LEFT);
            elif.Add(Expr());
            Expect(TokenCategory.PAR_RIGHT);
            Expect(TokenCategory.CURLY_LEFT);
            elif.Add(StmtList());
            Expect(TokenCategory.CURLY_RIGHT);
            elifList.Add(elif);
        }
        return elifList;
    }

    public Node Else()
    {
        var elseStmt = new Else();
        if (CurrentTokenCategory == TokenCategory.ELSE)
        {
            Expect(TokenCategory.ELSE);
            Expect(TokenCategory.CURLY_LEFT);
           elseStmt.Add(StmtList());
            Expect(TokenCategory.CURLY_RIGHT);
            return elseStmt;
        }

        return elseStmt;
    }

    

    public Node FunCall()
    {
        var functionCall = new FunCall() {Id()};
        Expect(TokenCategory.PAR_LEFT);
        functionCall.Add(ExprList());
        Expect(TokenCategory.PAR_RIGHT);
        return functionCall;
    }

    public Node ExprList()
    {
        var exprList = new ExprList();
        if (firstOfExpression.Contains(CurrentTokenCategory))
        {
            exprList.Add(Expr());
            while (CurrentTokenCategory == TokenCategory.COMMA)
            {
                Expect(TokenCategory.COMMA);
                exprList.Add(Expr());
            }
        }

        return exprList;
    }

    public Node StmtDec()
    {
        Expect(TokenCategory.DEC);
        var stmt = new StmtDec()
        {
            Id()
        };
        Expect(TokenCategory.SEMICOLON);
        return stmt;
    }

    public Node StmtInc()
    {
        Expect(TokenCategory.INC);
        var stmt = new StmtInc()
        {
            Id()
        };
        Expect(TokenCategory.SEMICOLON);
        return stmt;
    }
    

    public Node Expr()
    {
        var exprAnd = ExprAnd();
        var exprOr = new Or() { exprAnd };
        while (CurrentTokenCategory == TokenCategory.OR)
        {
            Expect(TokenCategory.OR);
            exprOr.Add(ExprAnd());
        }

        return exprOr.CountChildren() == 1 ? exprAnd : exprOr;

    }

    public Node ExprAnd()
    {
        var exprComp = ExprComp();
        var expAnd = new And() { exprComp };
        
        while (CurrentTokenCategory == TokenCategory.AND)
        {
            Expect(TokenCategory.AND);
            expAnd.Add(ExprComp());
        }

        return expAnd.CountChildren() == 1 ? exprComp : expAnd;
    }

    public Node ExprComp()
    {
        var exprComp = ExprRel();
        while (firstOfComparisons.Contains(CurrentTokenCategory))
        {
            var op = OpComp();
            op.Add(exprComp);
            op.Add(ExprRel());
            exprComp = op;
        }

        return exprComp;
    }

    public Node ExprRel()
    {
        var exprRel = ExprAdd();
        while (firstOfOperationRelative.Contains(CurrentTokenCategory))
        {
            var op = OpRel();
            op.Add(exprRel);
            op.Add(ExprAdd());
            exprRel = op;
        }
        return exprRel;
    }

    public Node OpRel()
    {
        switch (CurrentTokenCategory)
        {
            case TokenCategory.GREATER_THAN:
                return new GreaterThan()
                {
                    AnchorToken = Expect(TokenCategory.GREATER_THAN)
                };
            case TokenCategory.G_EQUAL:
                return new GreaterEqual()
                {
                    AnchorToken = Expect(TokenCategory.G_EQUAL)
                };
            case TokenCategory.LOWER_THAN:
                return new LowerThan()
                {
                    AnchorToken = Expect(TokenCategory.LOWER_THAN)
                };
            case TokenCategory.L_EQUAL:
                return new LowerEqual()
                {
                    AnchorToken = Expect(TokenCategory.L_EQUAL)
                };
            default:
                throw new SyntaxError(firstOfOperationRelative, _tokenStream.Current);

        }
    }

    // ‹expr-mul› (‹op-add› ‹expr-mul›)+
    public Node ExprAdd()
    {
        var exprAdd = ExprMul();
        while (firstOfAdditions.Contains(CurrentTokenCategory))
        {
            var op = OpAdd();
            op.Add(exprAdd);
            op.Add(ExprMul());
            exprAdd = op;
        }

        return exprAdd;
    }

    public Node OpAdd()
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
            default:
                throw new SyntaxError(firstOfAdditions, _tokenStream.Current);
        }
    }

    public Node ExprMul()
    {
        var exprMul = ExprUnary();
        while (firstOfMultiplications.Contains(CurrentTokenCategory))
        {
            var op = OpMul();
            op.Add(exprMul);
            op.Add(ExprUnary());
            exprMul = op;
        }

        return exprMul;
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

    public Node ExprUnary()
    {
        if (!firstOfUnary.Contains(CurrentTokenCategory))
            return ExprPrimary();

        var node = OpUnary();
        node.Add(ExprPrimary());
        return node;

    }

    public Node ExprPrimary()
    {
        if (CurrentTokenCategory == TokenCategory.PAR_LEFT) {
            Expect(TokenCategory.PAR_LEFT);
            var exprList = ExprList();
            Expect(TokenCategory.PAR_RIGHT);
            return exprList;
        }

        if (CurrentTokenCategory == TokenCategory.IDENTIFIER)
        {
            var id = Id();
            if (CurrentTokenCategory != TokenCategory.PAR_LEFT) return id;
            
            //  FUNCTION-CALL    
            Expect(TokenCategory.PAR_LEFT);
            var functionCall = new FunCall() {id, ExprList()};
            Expect(TokenCategory.PAR_RIGHT);
            return functionCall;

        }

        if (CurrentTokenCategory == TokenCategory.BRACKET_LEFT)
            return Array();
        if (firstOfLit.Contains(CurrentTokenCategory))
            return Lit();
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

    public Node Array()
    {
        Expect(TokenCategory.BRACKET_LEFT);
       var exprList =  ExprList();
        Expect(TokenCategory.BRACKET_RIGHT);
        return exprList;
    }

    public Node OpUnary()
    {
        switch(CurrentTokenCategory)
        {
            case TokenCategory.PLUS:
                return new UnaryPlus()
                {
                    AnchorToken = Expect(TokenCategory.PLUS)
                };
            case TokenCategory.MINUS:
                return new UnaryMinus()
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
