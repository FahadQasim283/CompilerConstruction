using System;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        Console.WriteLine("""This pattern ensures that:
                            && (AND operator) is matched.
                            || (OR operator) is matched.
                            ! (NOT operator) is matched\n\n""");
        Console.WriteLine("Enter a logical operator:");
        string input = Console.ReadLine();

        Regex logicalOperatorsRegex = new Regex(@"^(\&\&|\|\||!)$");

        if (logicalOperatorsRegex.IsMatch(input))
            Console.WriteLine("Valid logical operator.");
        else
            Console.WriteLine("Invalid logical operator.");
    }
}
