using MizanLang.Syntax;

namespace MizanLang.Tests;
using Xunit;

public class RuleParserUnitTests
{
    [Fact]
    public void Parse_BasicRule_ReturnsCorrectTree()
    {
        string code = "اگر [Age] > 18 باید [Status] = \"Adult\"";
        var rule = RuleParser.Parse(code);

        Assert.NotNull(rule);

        var filter = Assert.IsType<BinaryExpression>(rule.Filter);
        Assert.Equal(BinaryOperator.GreaterThan, filter.Operator);
        Assert.Equal("Age", ((IdentifierExpression)filter.Left).Parts.Last());
        Assert.Equal(18.0, ((LiteralExpression<double>)filter.Right).Value);

        var req = Assert.IsType<BinaryExpression>(rule.Requirement);
        Assert.Equal(BinaryOperator.Equal, req.Operator);
        Assert.Equal("Adult", ((LiteralExpression<string>)req.Right).Value);
    }

    [Fact]
    public void Parse_SynonymsAndNoiseWords_IgnoresNoiseAndParsesCorrectly()
    {
        // "است", "که", "مقدار" are noise words. "برابر" and "مساوی" are synonyms for "=".
        string code = "اگر مقدار [A] برابر 5 است باید [B] مساوی 10 باشد";
        var rule = RuleParser.Parse(code);

        var filter = Assert.IsType<BinaryExpression>(rule.Filter);
        Assert.Equal(BinaryOperator.Equal, filter.Operator);

        var req = Assert.IsType<BinaryExpression>(rule.Requirement);
        Assert.Equal(BinaryOperator.Equal, req.Operator);
    }

    [Fact]
    public void Parse_MathematicalPrecedence_MultipliesBeforeAdding()
    {
        // $1 + 2 * 3$ should group as $1 + (2 * 3)$
        string code = "اگر [X] = 1 + 2 * 3 باید [Y] = 1";
        var rule = RuleParser.Parse(code);

        var filter = Assert.IsType<BinaryExpression>(rule.Filter);
        var math = Assert.IsType<BinaryExpression>(filter.Right); // Right side of '='

        Assert.Equal(BinaryOperator.Add, math.Operator);

        var multi = Assert.IsType<BinaryExpression>(math.Right);
        Assert.Equal(BinaryOperator.Multiply, multi.Operator);
    }

    [Fact]
    public void Parse_InListAndBetween_ParsesCustomNodes()
    {
        string code = "اگر [Role] در لیست (\"Admin\", \"User\") باید [Age] بین 20 و 30";
        var rule = RuleParser.Parse(code);

        var inList = Assert.IsType<InListExpression>(rule.Filter);
        Assert.Equal("Role", ((IdentifierExpression)inList.Target).Parts.Last());
        Assert.Equal(2, inList.Values.Length);

        var between = Assert.IsType<BetweenExpression>(rule.Requirement);
        Assert.Equal("Age", ((IdentifierExpression)between.Target).Parts.Last());
        Assert.Equal(20.0, ((LiteralExpression<double>)between.LowerBound).Value);
        Assert.Equal(30.0, ((LiteralExpression<double>)between.UpperBound).Value);
    }

    [Fact]
    public void Parse_LogicalOperators_ParsesAndOrProperly()
    {
        string code = "اگر [A] > 1 و [B] < 2 یا [C] = 3 باید [Valid] = true";
        var rule = RuleParser.Parse(code);

        var rootOr = Assert.IsType<BinaryExpression>(rule.Filter);
        Assert.Equal(BinaryOperator.Or, rootOr.Operator);

        var leftAnd = Assert.IsType<BinaryExpression>(rootOr.Left);
        Assert.Equal(BinaryOperator.And, leftAnd.Operator);
    }

    [Fact]
    public void Parse_UnaryNot_WrapsExpression_Multipart_Ids()
    {
        string code = "اگر [Store.Status] برابر \"Closed\" نیست باید [Action] = true";
        var rule = RuleParser.Parse(code);

        var notExpr = Assert.IsType<UnaryExpression>(rule.Filter);
        Assert.Equal(UnaryOperator.Not, notExpr.Operator);

        var innerEq = Assert.IsType<BinaryExpression>(notExpr.Operand);
        Assert.Equal(BinaryOperator.Equal, innerEq.Operator);
        Assert.Equal("Status", ((IdentifierExpression)innerEq.Left).Parts[1]);
        Assert.Equal("Store", ((IdentifierExpression)innerEq.Left).Parts[0]);
    }
}