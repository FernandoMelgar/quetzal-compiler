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

    public void Visit(Function node)
    {
        var funName = node[0].AnchorToken.Lexeme;
        var paramList = node[1];
        foreach (var child in paramList)
            DeclareInScope((dynamic) child);
        Visit((dynamic) paramList);
        if (node[2].CountChildren() > 0) 
            Visit((dynamic) node[2]); // Var Def List
        Visit((dynamic) node[3]); // Stmt List
    }

    //-----------------------------------------------------------
    public void Visit(VarDef node) {

        var variableName = node[0].AnchorToken.Lexeme;

        if (VGST.Contains(variableName)) {
            throw new SemanticError(
                "Duplicated variable: " + variableName,
                node[0].AnchorToken);

        } else {
            VGST.Add(variableName);
        }
    }

    //-----------------------------------------------------------
    public void Visit(StmtList node) {
        VisitChildren(node);
    }

    //-----------------------------------------------------------
    public void Visit(Assign node) {

        var variableName = node.AnchorToken.Lexeme;

        if (!VGST.Contains(variableName)) {
            throw new SemanticError(
                "Undeclared variable: " + variableName,
                node.AnchorToken);
        }
    }

    //-----------------------------------------------------------
    public void Visit(Id node) {

        // TODO
        
    }

    
    //---------------------------------------------------
    public void Visit(Comparison node){
        string op = ">, <, >=, <=";
        if (node[0] == null ||
            node[1] == null) {
            throw new SemanticError(
                $"Operators {op} require two operands",
                node.AnchorToken);
        }
    }

    //---------------------------------------------------
    public void Visit(EqualComparison node){
        string op = "==";
        if (node[0] == null ||
            node[1] == null) {
            throw new SemanticError(
                $"Operator {op} requires two operands",
                node.AnchorToken);
        }
    }

    //---------------------------------------------------
    public void Visit(NotEqualComparison node){
        string op = "!=";
        if (node[0] == null ||
            node[1] == null) {
            throw new SemanticError(
                $"Operator {op} requires two operands",
                node.AnchorToken);
        }
    }

    //---------------------------------------------------
    public void Visit(Plus node){
        VisitBinaryOperator('+', node);
    }

    //---------------------------------------------------
    public void Visit(Minus node){
        VisitBinaryOperator('-', node);
    }

    //---------------------------------------------------
    public void Visit(Not node){
        if (node[0] == null) {
                throw new SemanticError(
                    $"Reserved word not requires an operand",
                    node.AnchorToken);
            }
    }

    //---------------------------------------------------
    public void Visit(True node){
        // TODO: Usualmente solo se regresa el tipo BOOL pero puede que podamos borrar este tipo de nodo (?)
    }

    //---------------------------------------------------
    public void Visit(False node){
        // TODO: Lo mismo que el False
    }

    //---------------------------------------------------
    public void Visit(Int node){
        var intStr = node.AnchorToken.Lexeme;
        int value;

        if (!Int32.TryParse(intStr, out value)) {
            throw new SemanticError(
                $"Integer literal too large: {intStr}",
                node.AnchorToken);
        }
    }

    //---------------------------------------------------
    public void Visit(Char node){
        var character = node.AnchorToken.Lexeme;
        char result;

        if (!char.TryParse(character, out result)) {            // Checa si character es null o tiene una longitud mayor a 1
            throw new SemanticError(
                $"Literal not a character: {character}",
                node.AnchorToken);
        }
    }

    //---------------------------------------------------
    public void Visit(String node){
        // TODO: No hay tryParse para String entonces maybe podemos deshacernos del nodo, maybe.
    }

    //---------------------------------------------------
    public void Visit(Multiplication node){
        VisitBinaryOperator('*', node);
    }

    //---------------------------------------------------
    public void Visit(Division node){
        VisitBinaryOperator('/', node);
    }

    //---------------------------------------------------
    public void Visit(ModuleOp node){
        VisitBinaryOperator('%', node);
    }

    //-----------------------------------------------------------
    void VisitChildren(Node node) {
        foreach (var n in node) {
            Visit((dynamic) n);
        }
    }

    //---------------------------------------------------
    void VisitBinaryOperator(char op, Node node) {
        if (node[0] == null ||
            node[1] == null) {
            throw new SemanticError(
                $"Operator {op} requires two operands",
                node.AnchorToken);
        }
    }


    private void DeclareInScope(dynamic child)
    {
        var localTable = FGST[CurrentFunction].refLST;
        foreach (Node param in child)
        {
            var varName = param.AnchorToken.Lexeme;
            if (!localTable.Contains(varName))
                localTable.Add(varName);
            else
                throw new SemanticError("Var declaration found twice", param.AnchorToken);
        }
    }
}