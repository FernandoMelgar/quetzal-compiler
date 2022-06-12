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
        sb.Append($"{_t}(import \"quetzal\" \"printi\" (func $printi (param i32) (result i32)))\n")
            .Append($"{_t}(import \"quetzal\" \"printc\" (func $printc (param i32) (result i32)))\n")
            .Append($"{_t}(import \"quetzal\" \"prints\" (func $prints (param i32) (result i32)))\n")
            .Append($"{_t}(import \"quetzal\" \"println\" (func $println (result i32)))\n")
            .Append($"{_t}(import \"quetzal\" \"readi\" (func $readi (result i32)))\n")
            .Append($"{_t}(import \"quetzal\" \"reads\" (func $reads (result i32)))\n")
            .Append($"{_t}(import \"quetzal\" \"new\" (func $new (param i32) (result i32)))\n")
            .Append($"{_t}(import \"quetzal\" \"size\" (func $size (param i32) (result i32)))\n")
            .Append($"{_t}(import \"quetzal\" \"add\" (func $add (param i32 i32) (result i32)))\n")
            .Append($"{_t}(import \"quetzal\" \"get\" (func $get (param i32 i32) (result i32)))\n")
            .Append($"{_t}(import \"quetzal\" \"set\" (func $set (param i32 i32 i32) (result i32)))\n")
            .Append(VisitGlobalVars());
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
        var sb = new StringBuilder().Append($"{_t}(func ${CurrentFunction}\n");
        IncreaseIndentation();
        sb.Append(Visit((dynamic) paramList))
            .Append($"{_t}(result i32)\n")
            .Append(Visit((dynamic) node[2]))
            .Append($"{_t}(local $_temp i32)\n")
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
            .Append($"{_t}(local $_temp i32)\n")
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

    public string Visit(VarDefList node)
    {
        if (node.CountChildren() > 0)
        {
            var sb = new StringBuilder();
            foreach (var child in node)
            {
                if (child.CountChildren() == 1)
                    sb.Append($"{_t}(local ${child[0].AnchorToken.Lexeme} i32)\n");
                else
                    foreach (var child2 in node[0])
                        sb.Append($"{_t}(local ${child2.AnchorToken.Lexeme} i32)\n");
            }

            return sb.ToString();
        }

        return "";
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

    public string Visit(UnaryPlus node)
    {
        return Visit((dynamic) node[0]);
    }

    public string Visit(Int node)
    {
        return node.AnchorToken.Lexeme.Contains('-')
            ? $"{_t}i32.const 0\n{_t}i32.const {node.AnchorToken.Lexeme[1..]}\n{_t}i32.sub\n"
            : $"{_t}i32.const {node.AnchorToken.Lexeme}\n";
    }

    public string Visit(StmtBreak node)
    {
        return $"{_t}br {_currentBreakLabel}\n";
    }

    public string Visit(Char node)
    {
        var character = node.AnchorToken.Lexeme.ToCharArray();
        var value = (int) character[1];
        if (value != '\\')
        {
            return $"{_t}i32.const {value}\n";
        }
        else
        {
            switch (character[2])
            {
                case 'n':
                    return $"{_t}i32.const 10\n";
                case 'r':
                    return $"{_t}i32.const 13\n";
                case 't':
                    return $"{_t}i32.const 9\n";
                case '\\':
                    return $"{_t}i32.const 92\n";
                case '\'':
                    return $"{_t}i32.const 39\n";
                case '"':
                    return $"{_t}i32.const 34\n";
                default:
                    var unicode = node.AnchorToken.Lexeme.Substring(3, 6);
                    var intValue = int.Parse(unicode, System.Globalization.NumberStyles.HexNumber);
                    return $"{_t}i32.const {intValue}\n";
            }
        }
    }

    public string Visit(String node)
    {
        var characterList = node.AnchorToken.Lexeme.ToList();
        characterList.RemoveAt(0);
        characterList.RemoveAt(characterList.Count - 1);
        var stringList = new List<string> { };
        foreach (var character in characterList)
        {
            var charToString = character.ToString();
            stringList.Add(charToString);
        }

        for (var i = 0; i < stringList.Count; i++)
        {
            if (stringList[i] != "\\")
            {
                continue;
            }

            if (stringList[i + 1] == "u")
            {
                for (var j = 1; j <= 7; j++)
                {
                    stringList[i] += stringList[i + 1];
                    stringList.RemoveAt(i + 1);
                }
            }
            else
            {
                stringList[i] += stringList[i + 1];
                stringList.RemoveAt(i + 1);
            }
        }

        var sb = new StringBuilder()
            .Append($"{_t}i32.const 0\n")
            .Append($"{_t}call $new\n")
            .Append($"{_t}local.set $_temp\n");
        for (var i = 0; i < stringList.Count + 1; i++)
            sb.Append($"{_t}local.get $_temp\n");
        foreach (var element in stringList)
        {
            var character = element.ToCharArray();
            sb.Append('\n')
                .Append(VisitCharFromString(character))
                .Append($"{_t}call $add\n")
                .Append($"{_t}drop\n");
        }

        return sb.Append('\n').ToString();
    }

    private string VisitCharFromString(char[] charArray)
    {
        var value = (int) charArray[0];
        if (value != '\\')
        {
            return $"{_t}i32.const {value}\n";
        }
        else
        {
            switch (charArray[1])
            {
                case 'n':
                    return $"{_t}i32.const 10\n";
                case 'r':
                    return $"{_t}i32.const 13\n";
                case 't':
                    return $"{_t}i32.const 9\n";
                case '\\':
                    return $"{_t}i32.const 92\n";
                case '\'':
                    return $"{_t}i32.const 39\n";
                case '"':
                    return $"{_t}i32.const 34\n";
                default:
                    var unicodeString = "";
                    for (int i = 2; i < 8; i++)
                    {
                        unicodeString += charArray[i];
                    }

                    var intValue = int.Parse(unicodeString, System.Globalization.NumberStyles.HexNumber);
                    return $"{_t}i32.const {intValue}\n";
            }
        }
    }

    public string Visit(StmtInc node)
    {
        
        var varName = node[0].AnchorToken.Lexeme;
        return new StringBuilder()
            .Append($"{_t}i32.const 1\n")
            .Append(Visit((dynamic) node[0]))
            .Append($"{_t}i32.add\n")
            .Append(varTable.Contains(varName)
                ? $"{_t}global.set ${varName}\n"
                : $"{_t}local.set ${varName}\n")
            .ToString();
    }

    public string Visit(StmtDec node)
    {
        return new StringBuilder()
            .Append($"{_t}local.get ${node[0].AnchorToken.Lexeme}\n")
            .Append($"{_t}i32.const 1\n")
            .Append($"{_t}i32.sub\n")
            .Append($"{_t}local.set ${node[0].AnchorToken.Lexeme}\n")
            .ToString();
    }

    public string Visit(Not node)
    {
        return new StringBuilder()
            .Append(Visit((dynamic) node[0]))
            .Append($"{_t}i32.eqz\n")
            .ToString();
    }

    public string Visit(And node)
    {
        var sb = new StringBuilder();

        sb.Append(Visit((dynamic) node[0]))
            .Append($"{_t}if (result i32)\n");
        IncreaseIndentation();
        sb.Append(Visit((dynamic) node[1]))
            .Append($"{_t}i32.eqz\n")
            .Append($"{_t}i32.eqz\n");
        DecreaseIndentation();
        sb.Append($"{_t}else\n");
        IncreaseIndentation();
        sb.Append($"{_t}i32.const 0\n");
        DecreaseIndentation();
        sb.Append($"{_t}end\n");

        return sb.ToString();
    }

    public string Visit(Or node)
    {
        var sb = new StringBuilder();

        sb.Append(Visit((dynamic) node[0]))
            .Append($"{_t}if (result i32)\n");
        IncreaseIndentation();
        sb.Append($"{_t}i32.const 1\n");
        DecreaseIndentation();
        sb.Append($"{_t}else\n");
        IncreaseIndentation();
        sb.Append(Visit((dynamic) node[1]))
            .Append($"{_t}i32.eqz\n")
            .Append($"{_t}i32.eqz\n");
        DecreaseIndentation();
        sb.Append($"{_t}end\n");

        return sb.ToString();
    }

    public string Visit(True node)
    {
        return $"{_t}i32.const 1\n";
    }

    public string Visit(False node)
    {
        return $"{_t}i32.const 0\n";
    }

    public string Visit(Loop node)
    {
        var et1 = GenerateLabel();
        var et2 = GenerateLabel();
        var oldLabel = _currentBreakLabel;
        _currentBreakLabel = et1;
        
        var sb = new StringBuilder()
            .Append($"{_t}block {et1}\n");
        IncreaseIndentation();
        sb.Append($"{_t}loop {et2}\n");
        IncreaseIndentation();
        foreach (var child in node[0])
            sb.Append(Visit((dynamic) child));
        sb.Append($"{_t}br {et2}\n");
        DecreaseIndentation();
        sb.Append($"{_t}end\n");
        DecreaseIndentation();
        _currentBreakLabel = oldLabel;
        return sb.Append($"{_t}end\n")
            .ToString();
    }


    public string Visit(ExprList node)
    {
        return VisitChildren(node);
    }

    public string Visit(ExprListArray node)
    {
        var sb = new StringBuilder()
            .Append($"{_t}i32.const 0\n")
            .Append($"{_t}call $new\n")
            .Append($"{_t}local.set $_temp\n");
        for (var i = 0; i < node.CountChildren() + 1; i++)
            sb.Append($"{_t}local.get $_temp\n");
        foreach (var child in node)
            sb.Append('\n')
                .Append(Visit((dynamic) child))
                .Append($"{_t}call $add\n")
                .Append($"{_t}drop\n");
        return sb.Append('\n').ToString();
    }

    public string Visit(StmtReturn node)
    {
        var sb = new StringBuilder();
        sb.Append(VisitChildren(node));
        sb.Append($"{_t}return\n");
        return sb.ToString();
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

    public string Visit(Plus node)
    {
        var sb = new StringBuilder();
        sb.Append(Visit((dynamic) node[0]))
            .Append(Visit((dynamic) node[1]))
            .Append($"{_t}i32.add\n");
        return sb.ToString();
    }

    public string Visit(Minus node)
    {
        var sb = new StringBuilder();
        sb.Append(Visit((dynamic) node[0]))
            .Append(Visit((dynamic) node[1]))
            .Append($"{_t}i32.sub\n");
        return sb.ToString();
    }

    public string Visit(Multiplication node)
    {
        var sb = new StringBuilder();
        sb.Append(Visit((dynamic) node[0]))
            .Append(Visit((dynamic) node[1]))
            .Append($"{_t}i32.mul\n");
        return sb.ToString();
    }

    public string Visit(Division node)
    {
        var sb = new StringBuilder();
        sb.Append(Visit((dynamic) node[0]))
            .Append(Visit((dynamic) node[1]))
            .Append($"{_t}i32.div_s\n");
        return sb.ToString();
    }

    public string Visit(ModuleOp node)
    {
        var sb = new StringBuilder();
        sb.Append(Visit((dynamic) node[0]))
            .Append(Visit((dynamic) node[1]))
            .Append($"{_t}i32.rem_s\n");
        return sb.ToString();
    }

    public string Visit(FunCall node)
    {
        return new StringBuilder()
            .Append(Visit((dynamic) node[1]))
            .Append($"{_t}call ${node[0].AnchorToken.Lexeme}\n{_t}drop\n")
            .ToString();
    }

    public string Visit(ExprFunCall node)
    {
        return new StringBuilder()
            .Append(Visit((dynamic) node[1]))
            .Append($"{_t}call ${node[0].AnchorToken.Lexeme}\n")
            .ToString();
    }

    public string Visit(UnaryMinus node)
    {
        return $"{_t}i32.const 0\n{Visit((dynamic) node[0])}{_t}i32.sub\n";
    }

    public string Visit(Id node)
    {
        var varName = node.AnchorToken.Lexeme;
        if (table[CurrentFunction].refLST.Contains(varName))
            return $"{_t}local.get ${varName}\n";
        return $"{_t}global.get ${varName}\n";
    }
}