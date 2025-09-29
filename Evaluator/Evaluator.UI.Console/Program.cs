using Evaluator.Core;

Console.WriteLine("Hello, Evaluator");
var infix1 = "4*5/(4+6)";
var result1 = ExpressionEvaluator.Evaluate(infix1);
Console.WriteLine($"{infix1}={result1}");

var infix2 = "4*(5+6-(8/2^3)-7)-1";
var result2 = ExpressionEvaluator.Evaluate(infix2);
Console.WriteLine($"{infix2}={result2}");

var infix3 = "123.89^(1.6/2.789)";
var result3 = ExpressionEvaluator.Evaluate(infix3);
Console.WriteLine($"{infix3}={result3}");

var infix4 = "45.2+45.2";
var result4 = ExpressionEvaluator.Evaluate(infix4);
Console.WriteLine($"{infix4}={result4}");