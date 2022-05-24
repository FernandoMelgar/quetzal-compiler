namespace QuetzalCompiler;

using System;
using System.Collections.Generic;

public class SemanticVisitor{


    //-----------------------------------------------------------
    static readonly IDictionary<TokenCategory, Type> typeMapper =
        new Dictionary<TokenCategory, Type>() {
            { TokenCategory.BOOL, Type.BOOL },
            { TokenCategory.INT, Type.INT }
        };
    
    //---------------------------------------------------
    public IDictionary<string, Int32> VGST {
        get;
        private set;
    }

    //---------------------------------------------------
    public SemanticVisitor(){
        VGST = new SortedDictionary<string, Int32>();
    }
    
    // TODO: Agregué unos nodos a SpecificNode.cs para poder checar las variables
    // Los nodos son Program, DeclarationList, Declaration, StatementList, Assignment e Identifier
    // Sólo los agregué a SpecificNode.cs

    //-----------------------------------------------------------
    public void Visit(Program node) {
        Visit((dynamic) node[0]);
        Visit((dynamic) node[1]);
    }

    //-----------------------------------------------------------
    public void Visit(DeclarationList node) {
        VisitChildren(node);
    }

    //-----------------------------------------------------------
    public void Visit(Declaration node) {

        var variableName = node[0].AnchorToken.Lexeme;

        if (VGST.ContainsKey(variableName)) {
            throw new SemanticError(
                "Duplicated variable: " + variableName,
                node[0].AnchorToken);

        } else {
            VGST.Add(variableName, Int32);                      // TODO: Agregar cada variable al VGST
        }
    }

    //-----------------------------------------------------------
    public void Visit(StatementList node) {
        VisitChildren(node);
    }

    //-----------------------------------------------------------
    public void Visit(Assignment node) {

        var variableName = node.AnchorToken.Lexeme;

        if (VGST.ContainsKey(variableName)) {

            var expectedType = VGST[variableName];

            if (expectedType != Visit((dynamic) node[0])) {
                throw new SemanticError(
                    "Expecting type " + expectedType
                    + " in assignment statement",
                    node.AnchorToken);
            }

        } else {
            throw new SemanticError(
                "Undeclared variable: " + variableName,
                node.AnchorToken);
        }
    }

    //-----------------------------------------------------------
    public void Visit(Identifier node) {

        // TODO
        
    }

    
    //---------------------------------------------------
    public void Visit(Comparison node){
        string op = ">, <, >=, <=";
        if (node[0] == null ||
            node[1] == null) {
            throw new SemanticError(
                $"Operators {op} require two operands",
                node.AnchorToken);
        }
    }

    //---------------------------------------------------
    public void Visit(EqualComparison node){
        string op = "==";
        if (node[0] == null ||
            node[1] == null) {
            throw new SemanticError(
                $"Operator {op} requires two operands",
                node.AnchorToken);
        }
    }

    //---------------------------------------------------
    public void Visit(NotEqualComparison node){
        string op = "!=";
        if (node[0] == null ||
            node[1] == null) {
            throw new SemanticError(
                $"Operator {op} requires two operands",
                node.AnchorToken);
        }
    }

    //---------------------------------------------------
    public void Visit(Plus node){
        VisitBinaryOperator('+', node);
    }

    //---------------------------------------------------
    public void Visit(Minus node){
        VisitBinaryOperator('-', node);
    }

    //---------------------------------------------------
    public void Visit(Not node){
        if (node[0] == null) {
                throw new SemanticError(
                    $"Reserved word not requires an operand",
                    node.AnchorToken);
            }
    }

    //---------------------------------------------------
    public void Visit(True node){
        // TODO: Usualmente solo se regresa el tipo BOOL pero puede que podamos borrar este tipo de nodo (?)
    }

    //---------------------------------------------------
    public void Visit(False node){
        // TODO: Lo mismo que el False
    }

    //---------------------------------------------------
    public void Visit(Int node){
        var intStr = node.AnchorToken.Lexeme;
        int value;

        if (!Int32.TryParse(intStr, out value)) {
            throw new SemanticError(
                $"Integer literal too large: {intStr}",
                node.AnchorToken);
        }
    }

    //---------------------------------------------------
    public void Visit(Char node){
        var character = node.AnchorToken.Lexeme;
        char result;

        if (!char.TryParse(character, out result)) {            // Checa si character es null o tiene una longitud mayor a 1
            throw new SemanticError(
                $"Literal not a character: {character}",
                node.AnchorToken);
        }
    }

    //---------------------------------------------------
    public void Visit(String node){
        // TODO: No hay tryParse para String entonces maybe podemos deshacernos del nodo, maybe.
    }

    //---------------------------------------------------
    public void Visit(Multiplication node){
        VisitBinaryOperator('*', node);
    }

    //---------------------------------------------------
    public void Visit(Division node){
        VisitBinaryOperator('/', node);
    }

    //---------------------------------------------------
    public void Visit(ModuleOp node){
        VisitBinaryOperator('%', node);
    }

    //-----------------------------------------------------------
    void VisitChildren(Node node) {
        foreach (var n in node) {
            Visit((dynamic) n);
        }
    }

    //---------------------------------------------------
    void VisitBinaryOperator(char op, Node node) {
        if (node[0] == null ||
            node[1] == null) {
            throw new SemanticError(
                $"Operator {op} requires two operands",
                node.AnchorToken);
        }
    }



}