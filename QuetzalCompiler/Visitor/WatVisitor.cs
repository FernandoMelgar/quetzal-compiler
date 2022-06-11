namespace QuetzalCompiler.Visitor;

public class WatVisitor
{
    HashSet<string> table;

    public WatVisitor(HashSet<string> table)
    {
        this.table = table;
    }
}