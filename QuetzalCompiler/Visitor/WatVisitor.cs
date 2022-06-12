using System.Text;

namespace QuetzalCompiler.Visitor;

public class WatVisitor
{
    IDictionary<string, ParamsFGST> table;
    HashSet<string> varTable;
    public string CurrentFunction;

    public WatVisitor(IDictionary<string, ParamsFGST> table, HashSet<string> fpvVgst)
    {
        this.table = table;
        this.varTable = fpvVgst;
    }


    public string Visit(DefList node)
    {
        var sb = new StringBuilder();
        sb.Append("(module\n");
        //TODO: Imports de funciones default
        sb.Append(VisitGlobalVars());
        foreach (var child in node)
        {
            if (child.GetType().Name == "VarDef")
                continue;
            sb.Append(Visit((dynamic) child));
        }

        return sb.ToString();
    }

    private string VisitGlobalVars()
    {
        var sb = new StringBuilder();
        foreach (var varName in varTable)
        {
            sb.Append($"  (global ${varName} (mut i32) (i32.const 0))\n");
        }

        return sb.ToString();
    }


    public string Visit(Function node)
    {
        CurrentFunction = node[0].AnchorToken.Lexeme;
        if (CurrentFunction == "main")
            return VisitMain(node);
        var paramList = node[1];
        Visit((dynamic) paramList);
        var varDefList = node[2];

        var sb = new StringBuilder();
        sb.Append($"  (func ${CurrentFunction})\n" +
                  Visit((dynamic) paramList) +
                  $"    (result i32)\n" +
                  Visit((dynamic) varDefList) +
                  Visit((dynamic) node[3]) + // Stmt List
                  $"    i32.const 0\n" +
                  $"  )\n");

        return sb.ToString();
    }

    private string VisitMain(Function node)
    {
        var varDefList = node[2];
        var sb = new StringBuilder();
        sb.Append($"  (func\n" +
                  $"    (export \"main\")\n" + // Param List
                  $"    (result i32)\n" +
                  Visit((dynamic) varDefList) + // VarDefList
                  Visit((dynamic) node[3]) + // Stmt List
                  $"    i32.const 0\n" +
                  $"  )\n");

        return sb.ToString();
    }

    public string Visit(ParamList node)
    {
        var sb = new StringBuilder();
        foreach (var n in node)
        {
            sb.Append($"    (param ${n.AnchorToken.Lexeme} i32)\n");
        }

        return sb.ToString();
    }

    public string Visit(VarDefList node)
    {
        var sb = new StringBuilder();
        var localTable = table[CurrentFunction].refLST;
        foreach (var varName in localTable)
        {
            sb.Append($"    (local ${varName} i32)\n");
        }

        return sb.ToString();
    }


    public string VisitChildren(Node node)
    {
        var sb = new StringBuilder();
        foreach (var n in node)
        {
            sb.Append(Visit((dynamic) n));
        }

        return sb.ToString();
    }

    public string Visit(StmtList node)
    {
        return VisitChildren(node);
    }

    public string Visit(Assign node)
    {
        var varName = node[0].AnchorToken.Lexeme;
        var sb = new StringBuilder()
            .Append(Visit((dynamic) node[1]));
        if (varTable.Contains(varName))
            sb.Append($"    global.set ${varName}\n");
        else
            sb.Append($"    local.set ${varName}\n");
        return sb.ToString();
    }

    public string Visit(Int node)
    {
        return $"    i32.const {node.AnchorToken.Lexeme}\n";
    }
}