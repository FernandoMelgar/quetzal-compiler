namespace QuetzalCompiler.Visitor;

public class SecondPassVisitor
{
    public string CurrentFunction;
    public int LoopLevel;
    public IDictionary<string, ParamsFGST> FGST { get; }
    public HashSet<string> VGST { get; }

    public SecondPassVisitor(IDictionary<string, ParamsFGST> FGST, HashSet<string> VGST)
    {
        this.FGST = FGST;
        this.VGST = VGST;
    }

    public void Visit(DefList node)
    {
        foreach (var child in node)
        {
            if (child.GetType().Name == "VarDef")
                continue;
            Visit((dynamic) child);
        }
    }
    
    public void Visit(If node) {
        Visit((dynamic) node[0]); // Expr
        Visit((dynamic) node[1]); // Stmtlist
        Visit((dynamic) node[2]); // ElseIfList
        Visit((dynamic) node[3]); // Else
    }

    public void Visit(Elif node) {
        //Console.WriteLine("Elif");
        Visit((dynamic) node[0]); // Expr
        Visit((dynamic) node[1]); // StmtList
    }
    public void Visit(ElifList node) {
        //Console.WriteLine("ElifList");
        if (node.CountChildren() > 0){
            VisitChildren(node);
        }
    }
    public void Visit(Else node) {
        //Console.WriteLine("Else");
        if (node.CountChildren() > 0){
            VisitChildren(node);
        }
    }
    public void Visit(Function node)
    {
        CurrentFunction = node[0].AnchorToken.Lexeme;
        var paramList = node[1];
        DeclareInScope(paramList);
        Visit((dynamic) paramList);
        if (node[2].CountChildren() > 0)
            Visit((dynamic) node[2]); // Var Def List
        Visit((dynamic) node[3]); // Stmt List
    }

    public void Visit(ParamList node)
    {
        VisitChildren(node);
    }

    public void Visit(Loop node) {
        //Console.WriteLine("Loop");
        LoopLevel++;
        Visit((dynamic)node[0]); // StmtList
        LoopLevel--;
    }
    
    public void Visit(StmtBreak node) {
        if (LoopLevel == 0){
            throw new SemanticError($"Incorrect usage of break statement", node.AnchorToken);
        }
    }
    public void Visit(VarDefList node)
    {
        if (node[0].CountChildren() > 0)
            DeclareInScope(node[0]);
        VisitChildren(node);
    }

    public void Visit(VarDef node)
    {
        var variableName = node[0].AnchorToken.Lexeme;
        if (!VGST.Contains(variableName))
            VGST.Add(variableName);
        else
            throw new SemanticError(
                "Duplicated variable: " + variableName,
                node[0].AnchorToken);
    }

    public void Visit(StmtList node)
    {
        VisitChildren(node);
    }

    public void Visit(Assign node)
    {
        var variableName = node[0].AnchorToken.Lexeme;
        var localTable = FGST[CurrentFunction].refLST;
        if (localTable.Contains(variableName))
        {
            VisitChildren(node);
        }
        else if (!VGST.Contains(variableName))
        {
            throw new SemanticError(
                "Undeclared variable: " + variableName,
                node[0].AnchorToken);
        }
    }

    public void Visit(Id node)
    {
        var variableName = node.AnchorToken.Lexeme;
        var localTable = FGST[CurrentFunction].refLST;
        if (localTable.Contains(variableName))
        {
            VisitChildren(node);
        }
        else if (!VGST.Contains(variableName))
        {
            throw new SemanticError(
                $"Variable not in scope: {node.AnchorToken.Lexeme}",
                node.AnchorToken
            );
        }
    }

    public void Visit(LowerThan node)
    {
        Visit((dynamic) node[0]);
        Visit((dynamic) node[1]);
    }
    public void Visit(LowerEqual node)
    {
        Visit((dynamic) node[0]);
        Visit((dynamic) node[1]);
    }
    public void Visit(GreaterEqual node)
    {
        Visit((dynamic) node[0]);
        Visit((dynamic) node[1]);
    }
    public void Visit(GreaterThan node)
    {
        Visit((dynamic) node[0]);
        Visit((dynamic) node[1]);
    }

    public void Visit(EqualComparison node)
    {
        Visit((dynamic) node[0]);
        Visit((dynamic) node[1]);
    }

    public void Visit(NotEqualComparison node)
    {
        Visit((dynamic) node[0]);
        Visit((dynamic) node[1]);
    }

    public void Visit(Plus node)
    {
        Visit((dynamic) node[0]);
        Visit((dynamic) node[1]);
    }

    public void Visit(Minus node)
    {
        Visit((dynamic) node[0]);
        Visit((dynamic) node[1]);
    }

    public void Visit(Not node)
    {
        Visit((dynamic) node[0]);
    }

    public void Visit(And node) {
        Visit((dynamic) node[0]);
        Visit((dynamic) node[1]);
    }
    public void Visit(Or node) {
        VisitChildren(node);
    }
    public void Visit(True node)
    {
    }

    public void Visit(False node)
    {
    }

    public void Visit(Int node)
    {
        var intStr = node.AnchorToken.Lexeme;
        int value;

        if (!Int32.TryParse(intStr, out value))
        {
            throw new SemanticError(
                $"Integer literal too large: {intStr}",
                node.AnchorToken);
        }
    }

    public void Visit(Char node)
    {
    }

    public void Visit(String node)
    {
    }

    public void Visit(Multiplication node)
    {
        Visit((dynamic) node[0]);
        Visit((dynamic) node[1]);
    }

    public void Visit(Division node)
    {
        Visit((dynamic) node[0]);
        Visit((dynamic) node[1]);
    }

    public void Visit(ModuleOp node)
    {
        Visit((dynamic) node[0]);
        Visit((dynamic) node[1]);
    }

    void VisitChildren(Node node)
    {
        foreach (var n in node)
        {
            Visit((dynamic) n);
        }
    }
    
    public void Visit(StmtInc node) {
        Visit((dynamic) node[0]);
    }
    
    public void Visit(StmtDec node) {
        Visit((dynamic) node[0]);
    }

    public void Visit(FunCall node)
    {
        var exprListNode = node[1];
        if(FGST.ContainsKey(node[0].AnchorToken.Lexeme)){
            var functionInfo = FGST[node[0].AnchorToken.Lexeme];
            if (functionInfo.arity != exprListNode.CountChildren()){
                throw new SemanticError(
                    $"Incorrect number of params for function: '{node[0].AnchorToken.Lexeme}', should be {functionInfo.arity} not {exprListNode.CountChildren()} :)",
                    node[0].AnchorToken
                );
            }
        }
        else
        {
            throw new SemanticError(
                $"Function '{node[0].AnchorToken.Lexeme}' Is not Created ",
                node[0].AnchorToken
            );
        }
    }
    public void Visit(ExprList node)
    {
        VisitChildren(node);
    }

    public void Visit(StmtReturn node)
    {
        VisitChildren(node);
    }
    

    private void DeclareInScope(Node paramList)
    {
        var localTable = FGST[CurrentFunction].refLST;
        foreach (var param in paramList)
        {
            var varName = param.AnchorToken.Lexeme;
            if (!localTable.Contains(varName))
                localTable.Add(varName);
            else
                throw new SemanticError("Var declaration found twice: ", param.AnchorToken);
        }
    }
}