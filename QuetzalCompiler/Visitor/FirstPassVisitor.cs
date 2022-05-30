namespace QuetzalCompiler.Visitor;

public class FirstPassVisitor
{
    public IDictionary<string, ParamsFGST> FGST { get; } = new Dictionary<string, ParamsFGST>()
    {
        {"println", new ParamsFGST(true, 0, null)},
        {"readi", new ParamsFGST(true, 0, null)},
        {"reads", new ParamsFGST(true, 0, null)},
        {"printi", new ParamsFGST(true, 1, null)},
        {"printc", new ParamsFGST(true, 1, null)},
        {"prints", new ParamsFGST(true, 1, null)},
        {"new", new ParamsFGST(true, 1, null)},
        {"size", new ParamsFGST(true, 1, null)},
        {"add", new ParamsFGST(true, 2, null)},
        {"get", new ParamsFGST(true, 2, null)},
        {"set", new ParamsFGST(true, 3, null)},
    };

    public HashSet<string> VGST { get; } = new();


    public void Visit(DefList node)
    {
        foreach (var child in node)
        {
            Visit((dynamic) child);
        }
    }

    public void Visit(VarDef node)
    {
        var varName = node[0].AnchorToken.Lexeme;
        if (!VGST.Contains(varName))
            VGST.Add(varName);
        else
            throw new SemanticError("Var declaration found Twice", node[0].AnchorToken);
    }

    public void Visit(Function node)
    {
        var funName = node[0].AnchorToken.Lexeme;
        if (funName == "main")
        {
            var arity = node[1].CountChildren();
            if (arity != 0)
            {
                throw new SemanticError("Function 'main' should have no parameters");
            }
        }
        if (!FGST.ContainsKey(funName))
        {
            var arity = node[1].CountChildren();
            FGST[funName] = new ParamsFGST(false, arity, new HashSet<string>());
        }
        else
            throw new SemanticError("Function definition found Twice", node.AnchorToken);
    }
}