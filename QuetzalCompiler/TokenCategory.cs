namespace QuetzalCompiler;

public enum TokenCategory
{
    // Palabras Reservadas∫
    RETURN,
    TRUE,
    AND,
    FALSE,
    OR,
    BREAK,
    IF,
    DEC,
    INC,
    ELIF,
    LOOP,
    VAR,
    ELSE,
    NOT,
    // Operaciones
    ASSIGN,
    EQUAL_COMPARISON,
    NOT_EQUAL,
    L_EQUAL,
    G_EQUAL,
    GREATER_THAN,
    LOWER_THAN,
    // Math
    PLUS,
    MINUS,
    MULTIPLY,
    DIVIDE,
    MODULE,
    INT_LITERAL,
    // SYMBOLS
    SEMICOLON,
    PAR_LEFT,
    PAR_RIGHT,
    CURLY_LEFT,
    CURLY_RIGHT,
    BRACKET_LEFT,
    BRACKET_RIGHT,
    COMMA,
    
    // Others
    IDENTIFIER,
    OTHER,
    STRING,
    CHAR,

    // To Ignore
    NEWLINE,
    WHITESPACE,
    BLOCK_COMMENT,
    LINE_COMMENT,
    ILLEGAL_CHAR,
}