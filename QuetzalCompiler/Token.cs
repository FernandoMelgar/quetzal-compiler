namespace QuetzalCompiler;

public class Token
{
    private readonly string _lexeme;

    private readonly TokenCategory _category;
    
    public string lexeme {
        get { return _lexeme; }
    }

    public TokenCategory Category {
        get { return _category; }
    }

    public Token(string lexeme, TokenCategory category)
    {
        _lexeme = lexeme;
        _category = category;
    }

    public override string ToString()
    {
        return $"{_category}, \"{lexeme}\"";
    }
}