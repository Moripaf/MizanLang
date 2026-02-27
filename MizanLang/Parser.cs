using MizanLang.Syntax;

namespace MizanLang;

using System;
using System.Linq;
using System.Collections.Immutable;
using Pidgin;
using static Pidgin.Parser;

public static class RuleParser
{
    // Tokenizer helpers: parses strings/tokens safely and skips trailing whitespace
    private static readonly Parser<char, string> NoiseWords =
        Try(String("است")).Or(Try(String("باشد"))).Or(Try(String("که"))).Or(Try(String("مقدار")));

    // Skip whitespaces OR noise words
    private static readonly Parser<char, Unit> SkipJunk =
        Whitespace.IgnoreResult().Or(NoiseWords.IgnoreResult()).Many().IgnoreResult();

    private static Parser<char, T> Tok<T>(Parser<char, T> p) => p.Before(SkipJunk);
    private static Parser<char, string> Tok(string s) => Tok(Try(String(s)));

    // Helper to evaluate left-associative binary expressions (e.g. 1 + 2 + 3)
    private static Parser<char, Expression> ChainLeft(Parser<char, Expression> operand, Parser<char, BinaryOperator> op)
    {
        var rest = Map((o, r) => new { Op = o, Right = r }, op, operand).Many();
        return Map(
            (first, tail) => tail.Aggregate(first, (acc, x)
                => new BinaryExpression(acc, x.Op, x.Right)),
            operand,
            rest
        );
    }

    // --- Literals ---
    private static readonly Parser<char, Expression> NumberLit =
        Tok(Real).Select<Expression>(d => new LiteralExpression(d));

    private static readonly Parser<char, Expression> StringLit =
        Tok(Char('"').Then(AnyCharExcept('"').ManyString()).Before(Char('"')))
        .Select<Expression>(s => new LiteralExpression(s));

    private static readonly Parser<char, Expression> BoolLit =
        Tok(String("true").ThenReturn(true).Or(String("false").ThenReturn(false)))
        .Select<Expression>(b => new LiteralExpression(b));

    private static readonly Parser<char, Expression> Literal =
        NumberLit.Or(StringLit).Or(BoolLit);

    // --- Identifier ---
    private static readonly Parser<char, Expression> Identifier =
        Tok(Char('[').Then(AnyCharExcept(']').ManyString()).Before(Char(']')))
        .Select<Expression>(name => new IdentifierExpression(name));

    // Forward declaration to allow recursive parentheses (e.g., (A + B))
    private static readonly Parser<char, Expression> Expr = Rec(() => ExpressionRule);

    // --- Precedence Level 1: Primary ---
    private static readonly Parser<char, Expression> Primary =
        Tok("(").Then(Expr).Before(Tok(")"))
        .Or(Identifier)
        .Or(Literal);

    // --- Precedence Level 2: Arithmetic ---
    private static readonly Parser<char, Expression> Multiplicative = ChainLeft(Primary,
        Tok("*").ThenReturn(BinaryOperator.Multiply)
        .Or(Tok("/").ThenReturn(BinaryOperator.Divide))
        .Or(Tok("%").ThenReturn(BinaryOperator.Modulo)));

    private static readonly Parser<char, Expression> Additive = ChainLeft(Multiplicative,
        Tok("+").ThenReturn(BinaryOperator.Add)
        .Or(Tok("-").ThenReturn(BinaryOperator.Subtract)));


// 4. Expanded Comparisons (Longest match first!)
    private static readonly Parser<char, BinaryOperator> CompareOp =
        Tok("بزرگتر مساوی").Or(Tok(">=")).ThenReturn(BinaryOperator.GreaterThanOrEqual)
            .Or(Tok("کوچکتر مساوی").Or(Tok("<=")).ThenReturn(BinaryOperator.LessThanOrEqual))
            .Or(Tok("برابر با").Or(Tok("برابر")).Or(Tok("مساوی")).Or(Tok("=")).ThenReturn(BinaryOperator.Equal))
            .Or(Tok("مخالف").Or(Tok("نابرابر")).Or(Tok("!=")).ThenReturn(BinaryOperator.NotEqual))
            .Or(Tok("بزرگتر از").Or(Tok(">")).ThenReturn(BinaryOperator.GreaterThan))
            .Or(Tok("کوچکتر از").Or(Tok("<")).ThenReturn(BinaryOperator.LessThan));

    private static readonly Parser<char, ImmutableArray<Expression>> ValueList =
        Literal.Separated(Tok(",")).Select(e => e.ToImmutableArray());

    // Matches the right-hand side of a comparison, an IN clause, or a BETWEEN clause
    private static readonly Parser<char, Func<Expression, Expression>> ComparisonTail =
        CompareOp.Then(Additive, (op, right) =>
                (Func<Expression, Expression>)(left => new BinaryExpression(left, op, right)))
        .Or(Tok("در").Then(Tok("لیست")).Then(Tok("(")).Then(ValueList).Before(Tok(")"))
            .Select(list => (Func<Expression, Expression>)(left => new InListExpression(left, list))))
        .Or(Tok("بین").Then(Literal).Before(Tok("و")).Then(Literal, (lower, upper)
            => (Func<Expression, Expression>)(left => new BetweenExpression(left, lower, upper))));

    private static readonly Parser<char, Expression> Comparison = Map(
        (left, tail) => tail.HasValue ? tail.Value(left) : left,
        Additive,
        ComparisonTail.Optional()
    );
    private static readonly Parser<char, Expression> NotTerm = Map(
        (expr, hasNot) => hasNot ? new UnaryExpression(UnaryOperator.Not, expr) : expr,
        Comparison,
        Tok("نیست").Or(Tok("نمی‌باشد")).Optional().Select(x => x.HasValue)
    );
    // --- Precedence Level 4 & 5: Logical Operators ---
    private static readonly Parser<char, Expression> AndTerm = ChainLeft(NotTerm, Tok("و").ThenReturn(BinaryOperator.And));

    private static readonly Parser<char, Expression> OrTerm = ChainLeft(AndTerm,
        Tok("یا").ThenReturn(BinaryOperator.Or));

    private static Parser<char, Expression> ExpressionRule => OrTerm;

    // --- Root Rule Parser ---
    public static readonly Parser<char, Rule> ParseRule =
        SkipWhitespaces.Then(
            Map(
                (filter, req) => new Rule(filter, req),
                Tok("اگر").Then(Expr),
                Tok("باید").Then(Expr)
            )
        );

    // Public API
    public static Rule Parse(string code) => ParseRule.ParseOrThrow(code);
}