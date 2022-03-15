namespace QuetzalCompiler;

public class Token
{
    private readonly string _lexeme;

    private readonly TokenCategory _category;
    
    readonly int _row;

    readonly int _column;

    
    public string lexeme => _lexeme;

    public TokenCategory Category => _category;

    public int Row => _row;

    public int Column => _column;

    public Token(string lexeme,
        TokenCategory category,
        int row,
        int column) {
        this._lexeme = lexeme;
        this._category = category;
        this._row = row;
        this._column = column;
    }

    public override string ToString() {
        return $"{{{_category}, \"{lexeme}\", @({_row}, {_column})}}";
    }
}