using System.Text;

namespace CollectionsTask.Helpers
{
    public static class RequestInput
    {
        public static string? RequestText(string message, bool censored = false, int charMax = -1, int charMin = -1, ConsoleKey escapeKey = ConsoleKey.Escape)
        {
            System.Console.Write(message);
            StringBuilder input = new();
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(intercept: true);

                switch (key.Key)
                {
                    case ConsoleKey.Enter :
                        if (input.Length < charMin)
                        {
                            Console.WriteLine();
                            System.Console.WriteLine($"Please input at least {charMin} characters.");
                            System.Console.Write(message);
                            input.Clear();
                        }
                        else
                            Console.WriteLine();
                        break;
                    case ConsoleKey.Backspace :
                        if (input.Length > 0)
                        {
                            input.Remove(input.Length - 1, 1);
                            Console.Write("\b");
                        }
                        break;
                    case ConsoleKey.Escape :
                        Console.WriteLine();
                        return null;
                    default :
                        if (charMax < 0 || input.Length < charMax)
                        {
                            input.Append(key.KeyChar);
                            if (censored) Console.Write("*");
                            else if (!char.IsControl(key.KeyChar))
                                Console.Write(key.KeyChar);
                        } 
                        break;
                }
            } while (key.Key != ConsoleKey.Enter);

            return input.ToString();
        }

        public static int? RequestInteger(string message, bool censored = false, bool allowNegative = false)
        {
            string toParse = RequestText(message, censored);
            
            if (toParse == null) 
                return null;
            else if (Int32.TryParse(toParse, out int n) && (allowNegative || n >= 0))
                return n;
            else 
            {
                Console.WriteLine("Please input a valid response.");
                return RequestInteger(message, censored, allowNegative);
            }
        }

        public static bool? RequestBoolean(string message)
        {
            string toParse = RequestText(message);
            
            if (toParse == null) 
                return null;
            else if (bool.TryParse(toParse, out bool n))
                return n;
            else 
            {
                Console.WriteLine("Please input true or false.");
                return RequestBoolean(message);
            }
        }

        public static double? RequestDouble(string message, bool censored = false, bool allowNegative = false)
        {
            string toParse = RequestText(message, censored);

            if (toParse == null)
                return null;
            else if (double.TryParse(toParse, out double n) && (allowNegative || n >= 0))
                return n;
            else 
            {
                Console.WriteLine("Please input a valid response.");
                return RequestDouble(message, censored, allowNegative);
            }
        }
    }
}