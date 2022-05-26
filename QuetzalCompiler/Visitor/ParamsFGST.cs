namespace QuetzalCompiler.Visitor;

public class ParamsFGST{
    public bool isPrimary;
    public int arity;

    public HashSet<string> refLST
    {
        get;
        set;
    } = new();
    
    public ParamsFGST(bool isPrimary, int arity, HashSet<string> refLST){
        this.isPrimary = isPrimary;
        this.arity = arity;
        this.refLST = refLST;
    }

}