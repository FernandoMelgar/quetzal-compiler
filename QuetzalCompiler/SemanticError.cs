namespace QuetzalCompiler;
using System;
/*
 * Authors:
 *   - A01748354: Fernando Manuel Melgar Fuentes
 *   - A01376364: Alex Serrano Durán
 */
public class SemanticError : Exception
{
    public SemanticError(string message, Token token) :
        base($"Semantic Error: {message} \n"
             + $"at row {token.Row}, column {token.Column}.")
    {
    }

    public SemanticError(string message) :
        base($"Semantic Error: {message} \n"
        )
    {
    }
}