namespace QuetzalCompiler;

/* Quetzal LL(1)
   ‹program›    →→    ‹def-list›
   ‹def-list›’    →→    ‹def-list› ‹def›
   ‹def-list›    →→    ε
   ‹def›    →→    ‹var-def›
   ‹def›    →→    ‹fun-def›
   ‹var-def›    →→    var ‹var-list› ;
   ‹var-list›    →→    ‹id-list›
   ‹id-list›    →→    ‹id› ‹id-list-cont›
   ‹id-list-cont›    →→    , ‹id› ‹id-list-cont›
   ‹id-list-cont›    →→    ε
   ‹fun-def›    →→    ‹id› ( ‹param-list› ) { ‹var-def-list› ‹stmt-list› }
   ‹param-list›    →→    ‹id-list›
   ‹param-list›    →→    ε
   ‹var-def-list›’    →→    ‹var-def-list› ‹var-def›
   ‹var-def-list›    →→    ε
   ‹stmt-list›’    →→    ‹stmt-list› ‹stmt›
   ‹stmt-list›    →→    ε
   ‹stmt›    →→    ‹stmt-assign›
   ‹stmt›    →→    ‹stmt-incr›
   ‹stmt›    →→    ‹stmt-decr›
   ‹stmt›    →→    ‹stmt-fun-call›
   ‹stmt›    →→    ‹stmt-if›
   ‹stmt›    →→    ‹stmt-loop›
   ‹stmt›    →→    ‹stmt-break›
   ‹stmt›    →→    ‹stmt-return›
   ‹stmt›    →→    ‹stmt-empty›
   ‹stmt-assign›    →→    ‹id› = ‹expr› ;
   ‹stmt-incr›    →→    inc ‹id› ;
   ‹stmt-decr›    →→    dec ‹id› ;
   ‹stmt-fun-call›    →→    ‹fun-call› ;
   ‹fun-call›    →→    ‹id› ( ‹expr-list› )
   ‹expr-list›    →→    ‹expr› ‹expr-list-cont›
   ‹expr-list›    →→    ε
   ‹expr-list-cont›    →→    , ‹expr› ‹expr-list-cont›
   ‹expr-list-cont›    →→    ε
   ‹stmt-if›    →→    if ( ‹expr› ) { ‹stmt-list› } ‹else-if-list› ‹else›
   ‹else-if-list›’    →→    ‹else-if-list› elif ( ‹expr› ) { ‹stmt-list› }
   ‹else-if-list›    →→    ε
   ‹else›    →→    else { ‹stmt-list› }
   ‹else›    →→    ε
   ‹stmt-loop›    →→    loop { ‹stmt-list› }
   ‹stmt-break›    →→    break ;
   ‹stmt-return›    →→    return ‹expr› ;
   ‹stmt-empty›    →→    ;
   ‹expr›    →→    ‹expr-or›
   ‹expr-or›’    →→    ‹expr-or› or ‹expr-and›
   ‹expr-or›    →→    ‹expr-and› ‹expr-or›’ or ε
   ‹expr-and›’    →→    ‹expr-and› and ‹expr-comp›
   ‹expr-and›    →→    ‹expr-comp›‹expr-and›’ or ε
   ‹expr-comp›’    →→    ‹expr-comp› ‹op-comp› ‹expr-rel›
   ‹expr-comp›    →→    ‹expr-rel›‹expr-comp›’ or ε
   ‹op-comp›    →→    ==
   ‹op-comp›    →→    !=
   ‹expr-rel›’    →→    ‹expr-rel› ‹op-rel› ‹expr-add›
   ‹expr-rel›    →→    ‹expr-add› ‹expr-rel›’ or ε
   ‹op-rel›    →→    <
   ‹op-rel›    →→    <=
   ‹op-rel›    →→    >
   ‹op-rel›    →→    >=
   ‹expr-add›’    →→    ‹expr-add› ‹op-add› ‹expr-mul›
   ‹expr-add›    →→    ‹expr-mul› ‹expr-add›’ or ε
   ‹op-add›    →→    +
   ‹op-add›    →→    −
   ‹expr-mul›’    →→    ‹expr-mul› ‹op-mul› ‹expr-unary›
   ‹expr-mul›    →→    ‹expr-unary› ‹expr-mul›’ or ε
   ‹op-mul›    →→    *
   ‹op-mul›    →→    /
   ‹op-mul›    →→    %
   ‹expr-unary›    →→    ‹op-unary› ‹expr-unary›
   ‹expr-unary›    →→    ‹expr-primary›
   ‹op-unary›    →→    +
   ‹op-unary›    →→    −
   ‹op-unary›    →→    not
   ‹expr-primary›    →→    ‹id›
   ‹expr-primary›    →→    ‹fun-call›
   ‹expr-primary›    →→    ‹array›
   ‹expr-primary›    →→    ‹lit›
   ‹expr-primary›    →→    ( ‹expr› )
   ‹array›    →→    [ ‹expr-list› ]
   ‹lit›    →→    ‹lit-bool›
   ‹lit›    →→    ‹lit-int›
   ‹lit›    →→    ‹lit-char›
   ‹lit›    →→    ‹lit-str›
 */
public class Parser
{
    private readonly IEnumerator<Token> _tokenStream;
    
    public Parser(IEnumerator<Token> tokenStream) {
        this._tokenStream = tokenStream;
        this._tokenStream.MoveNext();
    }

    public TokenCategory CurrentToken {
        get { return _tokenStream.Current.Category; }
    }

    public Token Expect(TokenCategory category) {
        if (CurrentToken == category) {
            Token current = _tokenStream.Current;
            _tokenStream.MoveNext();
            return current;
        } else {
            throw new NotImplementedException();
        }
    }

}