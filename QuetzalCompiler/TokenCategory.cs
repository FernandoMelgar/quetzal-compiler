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
    GREATER_THAN
}