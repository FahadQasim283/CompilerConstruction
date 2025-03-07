using System;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        Console.WriteLine("This pattern ensures that");
        Console.WriteLine("&& (AND operator) is matched");
        Console.WriteLine("|| (OR operator) is matched");
        Console.WriteLine("! (NOT operator) is matched");
        Console.WriteLine("Enter a logical operator:");
        Console.WriteLine("Enter a logical operator:");
        string input = Console.ReadLine();

        Regex logicalOperatorsRegex = new Regex(@"^(\&\&|\|\||!)$");

        if (logicalOperatorsRegex.IsMatch(input))
            Console.WriteLine("Valid logical operator.");
        else
            Console.WriteLine("Invalid logical operator.");
    }
}

/*
&&	Valid logical operator.
`	
!	Valid logical operator.
&	Invalid logical operator.
and	Invalid logical operator.
*/ 