namespace Evaluator.Core;

public class ExpressionEvaluator
{
    public static double Evaluate(string infix)
    {
        var postfix = InfixToPostfix(infix);
        return Calculate(postfix);
    }

    private static string InfixToPostfix(string infix)
    {
        var stack = new Stack<char>();
        var postfix = new System.Text.StringBuilder();
        var numberBuffer = new System.Text.StringBuilder();

        for (int i = 0; i < infix.Length; i++)
        {
            char item = infix[i];

            if (char.IsDigit(item) || item == '.')
            {
                numberBuffer.Append(item);

                if (i + 1 >= infix.Length || (!char.IsDigit(infix[i + 1]) && infix[i + 1] != '.'))
                {
                    if (numberBuffer.Length > 0)
                    {
                        postfix.Append(numberBuffer.ToString()).Append(' ');
                        numberBuffer.Clear();
                    }
                }
            }
            else if (IsOperator(item))
            {
                if (item == ')')
                {
                    while (stack.Count > 0 && stack.Peek() != '(')
                    {
                        postfix.Append(stack.Pop()).Append(' ');
                    }
                    if (stack.Count > 0) stack.Pop();
                }
                else if (item == '(')
                {
                    stack.Push(item);
                }
                else
                {
                    while (stack.Count > 0 &&
                           stack.Peek() != '(' &&
                           PriorityInfix(item) <= PriorityStack(stack.Peek()))
                    {
                        postfix.Append(stack.Pop()).Append(' ');
                    }
                    stack.Push(item);
                }
            }
            else if (!char.IsWhiteSpace(item))
            {
                throw new Exception($"Carácter inválido: '{item}'");
            }
        }

        if (numberBuffer.Length > 0)
        {
            postfix.Append(numberBuffer.ToString()).Append(' ');
        }

        while (stack.Count > 0)
        {
            postfix.Append(stack.Pop()).Append(' ');
        }

        return postfix.ToString().Trim();
    }

    private static bool IsOperator(char item) => item is '^' or '/' or '*' or '%' or '+' or '-' or '(' or ')';

    private static int PriorityInfix(char op) => op switch
    {
        '^' => 4,
        '*' or '/' or '%' => 2,
        '-' or '+' => 1,
        '(' => 5,
        _ => throw new Exception("Expresión inválida."),
    };

    private static int PriorityStack(char op) => op switch
    {
        '^' => 3,
        '*' or '/' or '%' => 2,
        '-' or '+' => 1,
        '(' => 0,
        _ => throw new Exception("Expresión inválida."),
    };

    private static double Calculate(string postfix)
    {
        var stack = new Stack<double>();
        var tokens = postfix.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        foreach (string token in tokens)
        {
            if (token.Length == 1 && IsOperator(token[0]))
            {
                if (stack.Count < 2)
                    throw new Exception("Expresión inválida: operandos insuficientes");

                var op2 = stack.Pop();
                var op1 = stack.Pop();
                stack.Push(Calculate(op1, token[0], op2));
            }
            else
            {
                if (double.TryParse(token, System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture, out double number))
                {
                    stack.Push(number);
                }
                else
                {
                    throw new Exception($"Formato de número inválido: '{token}'");
                }
            }
        }

        if (stack.Count != 1)
            throw new Exception("Expresión inválida");

        return stack.Pop();
    }

    private static double Calculate(double op1, char item, double op2) => item switch
    {
        '*' => op1 * op2,
        '/' => op2 == 0 ? throw new DivideByZeroException("División por cero") : op1 / op2,
        '^' => Math.Pow(op1, op2),
        '+' => op1 + op2,
        '-' => op1 - op2,
        '%' => op1 % op2,
        _ => throw new Exception("Operador inválido."),
    };
}