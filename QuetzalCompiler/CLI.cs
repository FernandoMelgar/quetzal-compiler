using System.Data;

namespace QuetzalCompiler;

public class CLI
{
    private string[] IncludeChanges = { "1. Lexical Analysis" };
    
    private void ApplicationHeader()
    {
        Console.WriteLine("Quetzal compiler, version 0.1"  );
        
    }

    void ReleaseIncludes() {
        Console.WriteLine("Included in this release:");
        foreach (var phase in IncludeChanges) {
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
            parser.Program();

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
    }
}

