using System.Text;

namespace QuetzalCompiler.Visitor;

public class WatVisitor
{
    IDictionary<string, ParamsFGST> table;
    public string CurrentFunction;
    
    public WatVisitor(IDictionary<string, ParamsFGST> table)
    {
        this.table = table;
    }

    public string Visit(Function node)
    {
        CurrentFunction = node[0].AnchorToken.Lexeme;
        var paramList = node[1];
        Visit((dynamic) paramList);
        var varDefList = node[2];

        var sb = new StringBuilder(); 
        sb.Append($"  (func\n" + 
                  $"    (export \"{CurrentFunction}\")\n" + 
                  Visit((dynamic) paramList) +              // Param List
                  $"    (result i32)\n" + 
                  Visit((dynamic) varDefList) +             // VarDefList
                  // Visit((dynamic) node[3]) +                // Stmt List
                  $"    i32.const 0\n" + 
                  $"  )\n");

        return sb.ToString();
    }
    
    public string Visit(ParamList node)
    {
        var sb = new StringBuilder();
        foreach (var n in node) {
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
    
    public string Visit(DefList node)
    {
        var sb = new StringBuilder();
        
        foreach (var child in node)
        {
            if (child.GetType().Name == "VarDef")
                continue;
            sb.Append(Visit((dynamic)child));
        }

        return sb.ToString();
    }

    public string VisitChildren(Node node)
    {
        var sb = new StringBuilder();
        foreach (var n in node) {
            sb.Append(Visit((dynamic) n));
        }
        return sb.ToString();
    }

}