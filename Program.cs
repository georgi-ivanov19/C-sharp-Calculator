using Calculator;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
Console.WriteLine("------- Console Calculator -------");
Console.WriteLine("Supported operators:\n\t* Addition (+)\n\t* Subtraction (-)\n\t* Multiplication (*)\n\t* Division (/)\n");
Console.WriteLine("⚠️ Parenthesis ARE supported ⚠️\n");
Console.WriteLine("⚠️ You can refer to the previous answer in an expression by using \"ans\" ⚠️\n");

Console.WriteLine("For available commands, input \"help\"\n");

decimal? _prevAnswer = null;
List<string> _history = new List<string>();

while (true)
{
    try
    {
        Console.Write("Input: ");
        string input = Console.ReadLine().Trim().ToLower();

        if (string.IsNullOrEmpty(input))
        {
            Console.Error.WriteLine("Please input an expression!");
        }
        else if(input == "help")
        {
            Console.WriteLine("To exit the calculator, input \"q\"");
            Console.WriteLine("To see an example expression, input \"example\"");
            Console.WriteLine("To see a history of your expressions, input \"history\"");
            Console.WriteLine("To clear the console, history and previous answer, input \"clear\"\n");
        }
        else if (input == "q")
        {
            Console.WriteLine("Exiting...");
            break;
        }
        else if (input == "example")
        {
            Console.WriteLine("(10 + 2.5) * 4)\n");
        }
        else if (input == "clear") 
        {
            _history = new List<string>();
            _prevAnswer = null;
            Console.Clear();
        }
        else if (input.Contains("ans") && _prevAnswer == null)
        {
            Console.Error.WriteLine("No previous answer to use in expression\n");
        }
        else if (input == "history")
        {
            if (_history.Count == 0)
            {
                Console.Error.WriteLine("No history available yet. Evaluate some expressions before using the command\n");
            }
            else
            {
                foreach (string line in _history)
                {
                    Console.WriteLine($"{line}");
                }
                Console.WriteLine();
            }
        }
        else
        {
            if (input.Contains("ans"))
            {
                int index = input.IndexOf("ans");
                input = input.Remove(index, 3).Insert(index, _prevAnswer.ToString());
            }
            _prevAnswer = Parser.Parse(input).Eval();
            Console.WriteLine($"Answer: {Math.Round((decimal)_prevAnswer, 4)}\n");
            _history.Add($"{input} = {_prevAnswer}");
        }


    }
    catch (Exception ex)
    {
        Console.Error.WriteLine(ex.Message);
    }
}

