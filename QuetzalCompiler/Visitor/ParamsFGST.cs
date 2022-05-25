namespace QuetzalCompiler.Visitor;

public class ParamsFGST{
    private bool isPrimary;
    private int arity;

    public HashSet<string> refLST {
        get;
        set;
    }
    public ParamsFGST(bool isPrimary, int arity, HashSet<string> refLST){
        this.isPrimary = isPrimary;
        this.arity = arity;
        this.refLST = refLST;
    }

}