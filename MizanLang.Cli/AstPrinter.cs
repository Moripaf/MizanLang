using MizanLang.Syntax;
namespace MizanLang.Cli;

using System;

public static class AstPrinter
{
    public static void Print(Rule rule)
    {
        Console.WriteLine("Rule");
        Console.WriteLine("├── Filter (اگر)");
        PrintNode(rule.Filter, "│   ", true);
        Console.WriteLine("└── Requirement (باید)");
        PrintNode(rule.Requirement, "    ", true);
    }

    private static void PrintNode(Expression expr, string indent, bool isLast)
    {
        string marker = isLast ? "└── " : "├── ";
        Console.Write(indent + marker);

        string childIndent = indent + (isLast ? "    " : "│   ");

        switch (expr)
        {
            case BinaryExpression b:
                Console.WriteLine($"BinaryExpression ({b.Operator})");
                PrintNode(b.Left, childIndent, false);
                PrintNode(b.Right, childIndent, true);
                break;

            case UnaryExpression u:
                Console.WriteLine($"UnaryExpression ({u.Operator})");
                PrintNode(u.Operand, childIndent, true);
                break;

            case InListExpression i:
                Console.WriteLine("InListExpression");
                PrintNode(i.Target, childIndent, i.Values.Length == 0);
                for (int j = 0; j < i.Values.Length; j++)
                {
                    PrintNode(i.Values[j], childIndent, j == i.Values.Length - 1);
                }
                break;

            case BetweenExpression bw:
                Console.WriteLine("BetweenExpression");
                PrintNode(bw.Target, childIndent, false);
                PrintNode(bw.LowerBound, childIndent, false);
                PrintNode(bw.UpperBound, childIndent, true);
                break;

            case IdentifierExpression id:
                Console.WriteLine($"Identifier: [{id.Name}]");
                break;

            case LiteralExpression lit:
                string valStr = lit.Value is string s ? $"\"{s}\"" : (lit.Value?.ToString() ?? "null");
                Console.WriteLine($"Literal: {valStr}");
                break;

            default:
                Console.WriteLine(expr?.GetType().Name ?? "Unknown");
                break;
        }
    }
}