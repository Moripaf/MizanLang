# Mizan DSL

## Summary

**Mizan** is a declarative, domain-specific language designed for data quality validation. It allows non-technical Persian-speaking users to define conditional logic without knowledge of SQL or C#.

**Core Logic:** Implication (`P -> Q`).

* **Semantics:** "If **Condition P** is true, then **Requirement Q** must hold."
* **Success Criteria:** If P is false, the row is skipped (valid). If P is true and Q is true, the row is valid. If P is true and Q is false, the row is invalid.

---

## Language Grammar (EBNF Structure)

For Version 1.0, we prioritize explicit delimiters for identifiers to avoid ambiguity with whitespace and keywords.

```ebnf
/* Root */
Rule            ::= FilterClause RequirementClause

/* Structure */
FilterClause    ::= "اگر" Expression
RequirementClause ::= "باید" Expression

/* Expressions */
Expression      ::= OrTerm
OrTerm          ::= AndTerm { "یا" AndTerm }
AndTerm         ::= Comparison { "و" Comparison }

/* Comparisons */
Comparison      ::= Additive [ CompareOp Additive ]
                  | Additive "در" "لیست" "(" ValueList ")"   /* IN Operator */
                  | Additive "بین" Literal "و" Literal      /* BETWEEN Operator */

/* Arithmetic */
Additive        ::= Multiplicative { ("+" | "-") Multiplicative }
Multiplicative  ::= Primary { ("*" | "/" | "%") Primary }

/* Primitives */
Primary         ::= "(" Expression ")"
                  | Identifier
                  | Literal

/* Terminals */
Identifier      ::= "[" QualifiedName "]"   /* e.g. [Personnel.Age] */
Literal         ::= Number | StringLiteral | Boolean
CompareOp       ::= "بزرگتر از" | "کوچکتر از" | "برابر با" | "مخالف" | "بزرگتر مساوی" | "کوچکتر مساوی" | ">" | "<" | "=" | "!="
```

**Noise Words:** The parser acts as a "lenient consumer," optionally skipping words like `است`, `باشد`, `مقدار`, `که` to allow natural phrasing.

**Note for UI/UX:** To handle the requirement of "choosing identifiers from a list," the UI should inject identifiers enclosed in brackets, e.g.
, `[پرسنلی.کد_ملی]`. The parser relies on `[]` to distinguish columns from Farsi keywords.

---

## Lexicon / Keyword Map

| Concept | Farsi Keyword(s) | Logical / Operator |
| :--- | :--- | :--- |
| **Structure** | `اگر` | `IF` (Filter Start) |
| | `باید` | `THEN` (Assertion Start) |
| **Logical** | `و` | `AND` |
| | `یا` | `OR` |
| | `نیست` / `نمی‌باشد` | `NOT` (Unary) |
| **Comparison** | `برابر` / `برابر با` / `مساوی` | `Equal` (`==`) |
| | `مخالف` / `نابرابر` | `NotEqual` (`!=`) |
| | `بزرگتر از` | `GreaterThan` (`>`) |
| | `کوچکتر مساوی` | `LessThanOrEqual` (`=<`) |
| | `بزرگتر مساوی` | `GreaterThanOrEqual` (`>=`) |
| | `کوچکتر از` | `LessThan` (`<`) |
| **Sets/Range** | `در لیست` / `شامل` | `In` (Set Inclusion) |
| | `بین` ... `و` ... | `Between` (Range) |
| **Noise** (Ignored) | `است`, `باشد`, `که`, `مقدار` | `NoOp` |

---

## Backend Interface

The system uses the **Visitor Pattern** to separate parsing from code generation.

```csharp
public interface IMizanVisitor<T>
{
    T Visit(ValidationRule rule);
    T Visit(BinaryExpression binary);
    T Visit(UnaryExpression unary);
    T Visit(InSetExpression inSet);
    T Visit(BetweenExpression between);
    T Visit(IdentifierExpression identifier);
    T Visit(LiteralExpression literal);
}

// Example Implementation Signatures
public class SqlGenerator : IMizanVisitor<string> { ... }
public class DotNetExpressionBuilder : IMizanVisitor<System.Linq.Expressions.Expression> { ... }
```

---

## Examples

the examples here show the code generated as sql expressions that can be put in a where clause <br/>
but backends can be made to generate source code from the ast (available in `./MizanLang/Syntax.cs`) for any other language

### Example 1: Simple Comparison

**Context:** Salary validation based on age.

* **Farsi Input:**
    `اگر [سن] بزرگتر از 18 باشد باید [وضعیت] برابر با 'بزرگسال' باشد`
* **SQL Output (Boolean Logic):**

    ```sql
    -- Logic: NOT(Filter) OR (Requirement)
    NOT ([Age] > 18) OR ([Status] = 'بزرگسال')
    ```

### Example 2: Set Inclusion (IN)

**Context:** Specific cities must have specific codes.

* **Farsi Input:**
    `اگر [شهر] در لیست ('تهران', 'شیراز') است باید [کد_منطقه] کوچکتر از 5 باشد`
* **SQL Output:**

    ```sql
    NOT ([City] IN ('تهران', 'شیراز')) OR ([RegionCode] < 5)
    ```

### Example 3: Complex Logic (AND/OR + Qualified Name)

**Context:** Financial verification involving nested logic.

* **Farsi Input:**
    `اگر [مالی.گردش] بزرگتر از 1000 و [پرسنلی.نوع] مخالف 'مدیر' باشد باید [تاییدیه] برابر 1 یا [توضیحات] مخالف '' باشد`

* **SQL Output:**

    ```sql
    NOT (([Finance].[Turnover] > 1000) AND ([Personnel].[Type] <> 'مدیر')) 
    OR 
    (([Approved] = 1) OR ([Description] <> ''))
    ```
  
## Contributions

all contributions. especially adding backends for languages are welcome