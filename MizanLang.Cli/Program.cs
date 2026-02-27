using System;
using System.IO;
using MizanLang;
using MizanLang.Syntax;


namespace MizanLang.Cli;
class Program
{
    static void Main(string[] args)
    {
        if (args.Length > 0 && (args[0] == "-h" || args[0] == "--help"))
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("  RuleEngine [filepath]");
            Console.WriteLine("  Or pipe input: echo 'اگر [Age] > 18 باید [Status] = \"Adult\"' | RuleEngine");
            return;
        }

        string input;

        if (args.Length > 0)
        {
            // Read from file
            string filePath = args[0];
            if (!File.Exists(filePath))
            {
                Console.Error.WriteLine($"Error: File not found -> {filePath}");
                Environment.Exit(1);
            }
            input = File.ReadAllText(filePath);
        }
        else
        {
            // Read from standard input (stdin)
            if (Console.IsInputRedirected)
            {
                input = Console.In.ReadToEnd();
            }
            else
            {
                Console.WriteLine("Enter your rule (Press Ctrl+Z on Windows or Ctrl+D on Unix to execute):");
                input = Console.In.ReadToEnd();
            }
        }

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.Error.WriteLine("Error: No input provided.");
            Environment.Exit(1);
        }

        try
        {
            // Parse the input
            Rule parsedRule = RuleParser.Parse(input);

            // Print the abstract syntax tree
            Console.WriteLine("\n--- Parsed Abstract Syntax Tree ---");
            AstPrinter.Print(parsedRule);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("\n--- Parsing Error ---");
            Console.Error.WriteLine(ex.Message);
            Environment.Exit(1);
        }
    }
}