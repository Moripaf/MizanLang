// ReSharper disable once CheckNamespace
namespace MizanLang.Syntax;

using System.Collections.Immutable;
using Sawmill;

public class Rule
{
    public Expression Filter { get; }
    public Expression Requirement { get; }

    public Rule(Expression filter, Expression requirement)
    {
        Filter = filter;
        Requirement = requirement;
    }
}

public enum BinaryOperator
{
    Or, And,
    Add, Subtract, Multiply, Divide, Modulo,
    GreaterThan, LessThan, Equal, NotEqual, GreaterThanOrEqual, LessThanOrEqual
}

public enum UnaryOperator { Not }

public abstract class Expression: IRewritable<Expression>
{
    public abstract int CountChildren();
    public abstract void GetChildren(Span<Expression> childrenReceiver);
    public abstract Expression SetChildren(ReadOnlySpan<Expression> newChildren);
}

public class BinaryExpression(Expression left, BinaryOperator op, Expression right) : Expression
{
    public Expression Left { get; } = left;
    public BinaryOperator Operator { get; } = op;
    public Expression Right { get; } = right;

    public override int CountChildren() => 2;

    public override void GetChildren(Span<Expression> childrenReceiver)
    {
        childrenReceiver[0] = Left;
        childrenReceiver[1] = Right;
    }

    public override Expression SetChildren(ReadOnlySpan<Expression> newChildren)
        => new BinaryExpression(newChildren[0], Operator, newChildren[1]);
}

public class UnaryExpression : Expression
{
    public UnaryOperator Operator { get; }
    public Expression Operand { get; }

    public UnaryExpression(UnaryOperator op, Expression operand)
    {
        Operator = op;
        Operand = operand;
    }

    public override int CountChildren() => 1;
    public override void GetChildren(Span<Expression> childrenReceiver) => childrenReceiver[0] = Operand;
    public override Expression SetChildren(ReadOnlySpan<Expression> newChildren) => new UnaryExpression(Operator, newChildren[0]);
}

public class InListExpression(Expression target, ImmutableArray<Expression> values) : Expression
{
    public Expression Target { get; } = target;
    public ImmutableArray<Expression> Values { get; } = values;

    public override int CountChildren() => 1 + Values.Length;

    public override void GetChildren(Span<Expression> childrenReceiver)
    {
        childrenReceiver[0] = Target;
        for (int i = 0; i < Values.Length; i++)
        {
            childrenReceiver[i + 1] = Values[i];
        }
    }

    public override Expression SetChildren(ReadOnlySpan<Expression> newChildren)
    {
        var newTarget = newChildren[0];
        var newValues = newChildren.Slice(1).ToArray().ToImmutableArray();
        return new InListExpression(newTarget, newValues);
    }
}

public class BetweenExpression(Expression target, Expression lowerBound, Expression upperBound)
    : Expression
{
    public Expression Target { get; } = target;
    public Expression LowerBound { get; } = lowerBound;
    public Expression UpperBound { get; } = upperBound;

    public override int CountChildren() => 3;

    public override void GetChildren(Span<Expression> childrenReceiver)
    {
        childrenReceiver[0] = Target;
        childrenReceiver[1] = LowerBound;
        childrenReceiver[2] = UpperBound;
    }

    public override Expression SetChildren(ReadOnlySpan<Expression> newChildren)
        => new BetweenExpression(newChildren[0], newChildren[1], newChildren[2]);
}

public class IdentifierExpression(string name) : Expression
{
    public string Name { get; } = name;

    public override int CountChildren() => 0;
    public override void GetChildren(Span<Expression> childrenReceiver) { }
    public override Expression SetChildren(ReadOnlySpan<Expression> newChildren) => this;
}

public class LiteralExpression(object value) : Expression
{
    // Value can hold int, double, string, or bool natively
    public object Value { get; } = value;

    public override int CountChildren() => 0;
    public override void GetChildren(Span<Expression> childrenReceiver) { }
    public override Expression SetChildren(ReadOnlySpan<Expression> newChildren) => this;
}