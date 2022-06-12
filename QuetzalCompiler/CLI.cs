using System;
using System.Data;
using System.IO;
using QuetzalCompiler.Visitor;

namespace QuetzalCompiler;

public class CLI
{
    private string[] IncludeChanges = {"1. Lexical Analysis"};

    private void ApplicationHeader()
    {
        Console.WriteLine("Quetzal compiler, version 0.1");
    }

    void ReleaseIncludes()
    {
        Console.WriteLine("Included in this release:");
        foreach (var phase in IncludeChanges)
        {
            Console.WriteLine("   * " + phase);
        }
    }

    public void start(string[] args)
    {
        ApplicationHeader();
        ReleaseIncludes();
        Console.WriteLine();


        try
        {
            var inputPath = "/Users/quality/RiderProjects/QuetzalSolution/QuetzalCompiler/001_hello.quetzal";
            var input = File.ReadAllText(inputPath);

            Console.WriteLine(
                $"===== Tokens from: \"{inputPath}\" =====");
            var count = 1;
            var parser = new Parser(new TokenClassifier().ClassifyAsEnumerable(input).GetEnumerator());
            var rootNode = parser.Program();
            Console.WriteLine(rootNode.ToStringTree());
            var firstPassVisitor = new FirstPassVisitor();
            firstPassVisitor.Visit((dynamic) rootNode);
            var secondPassVisitor = new SecondPassVisitor(new Dictionary<string, ParamsFGST>(firstPassVisitor.FGST), new HashSet<string>(firstPassVisitor.VGST));
            secondPassVisitor.Visit((dynamic) rootNode);
            Console.WriteLine("=== Variables ===");
            foreach (var variable in secondPassVisitor.VGST)
            {
                Console.WriteLine(variable);
            }

            Console.WriteLine("=== Functions ===");
            foreach (var function in secondPassVisitor.FGST)
            {
                Console.WriteLine(function.Key);
                if (function.Value.refLST != null && function.Value.refLST.Count > 0)
                {
                    foreach (var varName in function.Value.refLST)
                    {
                        Console.WriteLine($"- {varName}");
                    }
                }
            }

            var watVisitor = new WatVisitor(secondPassVisitor.FGST, firstPassVisitor.VGST);
            string result = watVisitor.Visit((dynamic) rootNode);
            Console.WriteLine(result);
        }
        catch (FileNotFoundException e)
        {
            Console.Error.WriteLine(e.Message);
            Environment.Exit(1);
        }
        catch (SyntaxErrorException e)
        {
            Console.Error.WriteLine(e.Message);
            Environment.Exit(1);
        }
        catch (SemanticError e)
        {
            Console.Error.WriteLine(e.Message);
            Environment.Exit(1);
        }
    }
}