using System.Text.RegularExpressions;

namespace QuetzalCompiler;

public class TokenClassifier
{
    private static readonly Regex _regex = new Regex(
        @"
        (?<BlockComment> [/][*].*?[*][/])
        | (?<LineComment> [/][/].*)
        | (?<Newline> \n )
        | (?<WhiteSpace> \s )  
        # Palabras reservadas
        | (?<Return> return)
        | (?<True> true)
        | (?<False> false)
        | (?<And> and)
        | (?<Or> or)
        | (?<Break> break)
        | (?<If> if)
        | (?<Dec> dec)
        | (?<Inc> inc)
        | (?<Elif> elif)
        | (?<Loop> loop)
        | (?<Var> var)
        | (?<Else> else)
        | (?<Not> not)
        # Operaciones
        | (?<EqualComparison> ==)
        | (?<NotEqual> !=)
        | (?<LowerEqual> <=)
        | (?<GreaterEqual> >=)
        | (?<Assign> [=])
        | (?<GreaterThan> [>])
        | (?<LowerThan> [<])
        | (?<Plus> [+])
        | (?<Minus> [-])
        | (?<Multiply> [*])
        | (?<Divide> [/])
        | (?<IntLiteral> \d+ )
        | (?<Semicolon> [;])
        | (?<ParLeft> [(]) 
        | (?<ParRight> [)])
        | (?<CurlyLeft> [{])
        | (?<CurlyRight> [}])
        | (?<Module> [%])
        | (?<BracketLeft> [[])
        | (?<BracketRight> []])
        | (?<Identifier> [a-zA-Z]+ )     # Must go after all keywords
        | (?<Other> . ) 
",
        RegexOptions.IgnorePatternWhitespace
        | RegexOptions.Compiled
        | RegexOptions.Multiline 
        | RegexOptions.Singleline
    );

    private static readonly IDictionary<string, TokenCategory> tokenMap =
        new Dictionary<string, TokenCategory>()
        {
            {"Return", TokenCategory.RETURN},
            {"True", TokenCategory.TRUE},
            {"And", TokenCategory.AND},
            {"Or", TokenCategory.OR},
            {"False", TokenCategory.FALSE},
            {"Break", TokenCategory.BREAK},
            {"If", TokenCategory.IF},
            {"Dec", TokenCategory.DEC},
            {"Inc", TokenCategory.INC},
            {"Elif", TokenCategory.ELIF},
            {"Loop", TokenCategory.LOOP },
            {"Var", TokenCategory.VAR},
            {"Else", TokenCategory.ELSE},
            {"Not", TokenCategory.NOT},
            {"EqualComparison", TokenCategory.EQUAL_COMPARISON},
            {"NotEqual", TokenCategory.NOT_EQUAL},
            {"Assign", TokenCategory.ASSIGN},
            {"LowerEqual", TokenCategory.L_EQUAL},
            {"GreaterEqual", TokenCategory.G_EQUAL},
            {"GreaterThan", TokenCategory.GREATER_THAN},
            {"LowerThan", TokenCategory.LOWER_THAN},
            {"Plus", TokenCategory.PLUS},
            {"Minus", TokenCategory.MINUS},
            {"Multiply", TokenCategory.MULTIPLY},
            {"Divide",TokenCategory.DIVIDE},
            {"Semicolon", TokenCategory.SEMICOLON},
            {"ParLeft", TokenCategory.PAR_LEFT},
            {"ParRight", TokenCategory.PAR_RIGHT},
            {"CurlyLeft", TokenCategory.CURLY_LEFT},
            {"CurlyRight", TokenCategory.CURLY_RIGHT},
            {"Module", TokenCategory.MODULE},
            {"BracketLeft", TokenCategory.BRACKET_LEFT},
            {"BracketRight", TokenCategory.BRACKET_RIGHT},
            {"Identifier", TokenCategory.IDENTIFIER},
            {"Other", TokenCategory.OTHER},
            {"Newline", TokenCategory.NEWLINE},
            {"WhiteSpace", TokenCategory.WHITESPACE},
            {"IntLiteral", TokenCategory.INT_LITERAL},
            {"BlockComment", TokenCategory.BLOCK_COMMENT},
            {"LineComment", TokenCategory.LINE_COMMENT}
        };
    
    public LinkedList<Token> classify(string input)
    {
        var tokenizedInput = new LinkedList<Token>();
        
        foreach (Match match in _regex.Matches(input))
        {
            if (match.Groups["Newline"].Success 
                || match.Groups["WhiteSpace"].Success
                || match.Groups["BlockComment"].Success
                || match.Groups["LineComment"].Success)
            { }
            else
            {
                tokenizedInput.AddLast(_findToken(match));
            }
        }

        return tokenizedInput;
    }

    private static Token _findToken(Match match)
    {
        foreach (var tokenCategory in tokenMap.Keys)
        {
            if (match.Groups[tokenCategory].Success)
            {
                return new Token(match.Value, tokenMap[tokenCategory]);
            }
        }
        throw new InvalidOperationException(
            "regex and tokenMap are inconsistent: " + match.Value);
    }
}

