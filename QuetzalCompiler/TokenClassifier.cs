using System.Text.RegularExpressions;

namespace QuetzalCompiler;

public class TokenClassifier
{
    private static readonly Regex _regex = new Regex(
        @"
        # Palabras reservadas
        (?<Return> return)
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

",
        RegexOptions.IgnorePatternWhitespace
        | RegexOptions.Compiled
        | RegexOptions.Multiline
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
            {"GreaterThan", TokenCategory.GREATER_THAN}
        };
    public LinkedList<Token> classify(string input)
    {
        var tokenizedInput = new LinkedList<Token>();
        
        foreach (Match match in _regex.Matches(input))
        {
            tokenizedInput.AddLast(_findToken(match));
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

