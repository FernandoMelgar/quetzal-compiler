using System.Text;

namespace QuetzalCompiler.Visitor;

public class WatVisitor
{
    IDictionary<string, ParamsFGST> table;
    HashSet<string> varTable;
    public string CurrentFunction;
    private string _currentBreakLabel;
    private int _identationLevel = 0;
    public string _t = "";

    public void IncreaseIndentation()
    {
        _identationLevel += 1;
        _t += "  ";
    }


    public void DecreaseIndentation()
    {
        if (_identationLevel <= 0)
            return;
        _identationLevel -= 1;
        _t = _t[..^2];
    }

    public WatVisitor(IDictionary<string, ParamsFGST> table, HashSet<string> fpvVgst)
    {
        this.table = table;
        varTable = fpvVgst;
    }

    int _labelCounter = 0;

    public string GenerateLabel()
    {
        return $"${_labelCounter++:00000}";
    }

    public string Visit(DefList node)
    {
        var sb = new StringBuilder()
            .Append("(module\n");
        IncreaseIndentation();
        sb.Append(""); //TODO: Imports de funciones default
        sb.Append(VisitGlobalVars());
        foreach (var child in node)
        {
            if (child.GetType().Name == "VarDef")
                continue;
            sb.Append(Visit((dynamic) child));
        }

        DecreaseIndentation();
        return sb.Append(')').ToString();
    }

    private string VisitGlobalVars()
    {
        var sb = new StringBuilder();
        foreach (var varName in varTable)
            sb.Append($"{_t}(global ${varName} (mut i32) (i32.const 0))\n");
        return sb.ToString();
    }


    public string Visit(Function node)
    {
        CurrentFunction = node[0].AnchorToken.Lexeme;
        if (CurrentFunction == "main")
            return VisitMain(node);
        var paramList = node[1];
        var sb = new StringBuilder().Append($"{_t}(func ${CurrentFunction})\n");
        IncreaseIndentation();
        sb.Append(Visit((dynamic) paramList))
            .Append($"{_t}(result i32)\n")
            .Append(Visit((dynamic) node[2]))
            .Append(Visit((dynamic) node[3]))
            .Append($"{_t}i32.const 0\n");
        DecreaseIndentation();
        return sb.Append($"{_t})\n").ToString();
    }

    private string VisitMain(Function node)
    {
        var sb = new StringBuilder()
            .Append($"{_t}(func\n");
        IncreaseIndentation();
        sb.Append($"{_t}(export \"main\")\n")
            .Append($"{_t}(result i32)\n")
            .Append(Visit((dynamic) node[2])) //VarDefList
            .Append(Visit((dynamic) node[3])) //StmtList
            .Append($"{_t}i32.const 0\n");
        DecreaseIndentation();
        sb.Append($"{_t})\n");
        return sb.ToString();
    }

    public string Visit(ParamList node)
    {
        var sb = new StringBuilder();
        foreach (var n in node)
            sb.Append($"{_t}(param ${n.AnchorToken.Lexeme} i32)\n");
        return sb.ToString();
    }

    public string Visit(VarDefList _)
    {
        var sb = new StringBuilder();
        var localTable = table[CurrentFunction].refLST;
        foreach (var varName in localTable)
            sb.Append($"{_t}(local ${varName} i32)\n");
        return sb.ToString();
    }

    private string VisitChildren(Node node)
    {
        var sb = new StringBuilder();
        foreach (var n in node)
            sb.Append(Visit((dynamic) n));
        return sb.ToString();
    }

    public string Visit(StmtList node)
    {
        return VisitChildren(node);
    }

    public string Visit(Assign node)
    {
        var varName = node[0].AnchorToken.Lexeme;
        return new StringBuilder()
            .Append(Visit((dynamic) node[1]))
            .Append(varTable.Contains(varName)
                ? $"{_t}global.set ${varName}\n"
                : $"{_t}local.set ${varName}\n")
            .ToString();
    }

    public string Visit(Int node)
    {
        return node.AnchorToken.Lexeme.Contains('-')
            ? $"{_t}i32.const 0\n{_t}i32.const {node.AnchorToken.Lexeme[1..]}\n{_t}i32.sub\n"
            : $"{_t}i32.const {node.AnchorToken.Lexeme}\n";
    }

    public string Visit(StmtInc node)
    {
        return new StringBuilder()
            .Append($"{_t}i32.const 1\n")
            .Append($"{_t}local.get ${node[0].AnchorToken.Lexeme}\n")
            .Append($"{_t}$i32.add\n")
            .Append($"{_t}local.set ${node[0].AnchorToken.Lexeme}\n")
            .ToString();
    }

    public string Visit(StmtDec node)
    {
        return new StringBuilder()
            .Append($"{_t}local.get ${node[0].AnchorToken.Lexeme}\n")
            .Append($"{_t}i32.const 1\n")
            .Append($"{_t}$i32.sub\n")
            .Append($"{_t}local.set ${node[0].AnchorToken.Lexeme}\n")
            .ToString();
    }

    public string Visit(Not node)
    {
        return new StringBuilder()
            .Append($"{_t}i32.const {node[0].AnchorToken.Lexeme}\n")
            .Append($"{_t}i32.eqz\n")
            .ToString();
    }

    public string Visit(Loop node)
    {
        _currentBreakLabel = GenerateLabel();
        var loopLabel = GenerateLabel();
        var sb = new StringBuilder()
            .Append($"{_t}block {_currentBreakLabel}\n");
        IncreaseIndentation();
        sb.Append($"{_t}loop {loopLabel}\n");
        IncreaseIndentation();
        foreach (var child in node[0])
            sb.Append(Visit((dynamic) child));
        sb.Append($"{_t}br {loopLabel}\n");
        DecreaseIndentation();
        sb.Append($"{_t}end\n");
        DecreaseIndentation();
        return sb.Append($"{_t}end\n")
            .ToString();
    }

    public string Visit(ExprList node)
    {
        return VisitChildren(node);
    }

    public string Visit(If node)
    {
        var sb = new StringBuilder()
            .Append(Visit((dynamic) node[0])) // Expr
            .Append($"{_t}if\n");
        IncreaseIndentation();
        sb.Append(Visit((dynamic) node[1])); // StmtList
        DecreaseIndentation();
        var baseIndentationLevel = _identationLevel;
        if (node[2].CountChildren() > 0)
            sb.Append(Visit((dynamic) node[2])); // ElseIfList
        if (node[3].CountChildren() > 0)
            sb.Append(Visit((dynamic) node[3])); // Else
        do
        {
            sb.Append($"{_t}end\n");
            DecreaseIndentation();
        } while (_identationLevel >= baseIndentationLevel);
        IncreaseIndentation(); // Compesa un identation que estoy quitando de más en el loop
        return sb.ToString();
    }

    public string Visit(Elif node)
    {
        var sb = new StringBuilder();
        DecreaseIndentation();
        sb.Append($"{_t}else\n");
        IncreaseIndentation();
        sb.Append(Visit((dynamic) node[0]));
        sb.Append($"{_t}if\n");
        IncreaseIndentation();
        sb.Append(Visit((dynamic) node[1])); // StmtList
        DecreaseIndentation();
        return sb.ToString();
    }

    public string Visit(ElifList node)
    {
        var sb = new StringBuilder();
        foreach (var child in node)
        {
            IncreaseIndentation();
            sb.Append(Visit((dynamic) child));
        }

        return sb.ToString();
    }

    public string Visit(Else node)
    {
        var sb = new StringBuilder();
        sb.Append($"{_t}else\n");
        IncreaseIndentation();
        if (node.CountChildren() > 0)
            sb.Append(VisitChildren(node));
        DecreaseIndentation();
        return sb.ToString();
    }


    public string Visit(LowerThan node)
    {
        var sb = new StringBuilder();
        sb.Append(Visit((dynamic) node[0]));
        sb.Append(Visit((dynamic) node[1]));
        sb.Append($"{_t}i32.lt_s\n");
        return sb.ToString();
    }

    public string Visit(LowerEqual node)
    {
        var sb = new StringBuilder();
        sb.Append(Visit((dynamic) node[0]));
        sb.Append(Visit((dynamic) node[1]));
        sb.Append($"{_t}i32.le_s\n");
        return sb.ToString();
    }

    public string Visit(GreaterEqual node)
    {
        var sb = new StringBuilder();
        sb.Append(Visit((dynamic) node[0]));
        sb.Append(Visit((dynamic) node[1]));
        sb.Append($"{_t}i32.ge_s\n");
        return sb.ToString();
    }

    public string Visit(GreaterThan node)
    {
        var sb = new StringBuilder();
        sb.Append(Visit((dynamic) node[0]));
        sb.Append(Visit((dynamic) node[1]));
        sb.Append($"{_t}i32.gt_s\n");
        return sb.ToString();
    }

    public string Visit(EqualComparison node)
    {
        var sb = new StringBuilder();
        sb.Append(Visit((dynamic) node[0]));
        sb.Append(Visit((dynamic) node[1]));
        sb.Append($"{_t}i32.eq\n");
        return sb.ToString();
    }

    public string Visit(NotEqualComparison node)
    {
        var sb = new StringBuilder();
        sb.Append(Visit((dynamic) node[0]));
        sb.Append(Visit((dynamic) node[1]));
        sb.Append($"{_t}i32.ne\n");
        return sb.ToString();
    }
}