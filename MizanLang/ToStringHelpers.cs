using MizanLang.Syntax;

namespace MizanLang;

public static class ToStringHelpers
{
    public static string ToDecompiledString(this BinaryOperator binaryOperator)
        => binaryOperator switch
        {
            BinaryOperator.Or => "یا",
            BinaryOperator.And => "و",
            BinaryOperator.Equal => "=",
            BinaryOperator.NotEqual => "!=",
            BinaryOperator.LessThan => "<",
            BinaryOperator.LessThanOrEqual => "<=",
            BinaryOperator.GreaterThan => ">",
            BinaryOperator.GreaterThanOrEqual => ">=",
            _ => "عملگر"
        };

    public static string ToDecompiledString(this UnaryOperator unaryOperator)
        => unaryOperator switch
        {
            UnaryOperator.Not => "نیست",
            _ => ""
        };
}
/*
 *
            case BinaryOperator.Add:
       break;
   case BinaryOperator.Subtract:
       break;
   case BinaryOperator.Multiply:
       break;
   case BinaryOperator.Divide:
       break;
   case BinaryOperator.Modulo:
       break;
   case BinaryOperator.GreaterThan:
       break;
   case BinaryOperator.LessThan:
       break;
   case BinaryOperator.Equal:
       break;
   case BinaryOperator.NotEqual:
       break;
   case BinaryOperator.GreaterThanOrEqual:
       break;
   case BinaryOperator.LessThanOrEqual:
       break;
   default:
       throw new ArgumentOutOfRangeException(nameof(binaryOperator), binaryOperator, null);
*/