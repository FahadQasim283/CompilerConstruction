using System;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        Console.WriteLine("This pattern ensures that:");
        Console.WriteLine("== (equal to) is matched.");
        Console.WriteLine("!= (not equal to) is matched.");
        Console.WriteLine("< (less than) is matched.");
        Console.WriteLine("> (greater than) is matched.");
        Console.WriteLine("<= (less than or equal to) is matched.");
        Console.WriteLine(">= (greater than or equal to) is matched.\n\n");
        Console.WriteLine("Enter a relational operator:");
        string input = Console.ReadLine();

        Regex relationalOperatorsRegex = new Regex(@"^(==|!=|<=|>=|<|>)$");

        if (relationalOperatorsRegex.IsMatch(input))
            Console.WriteLine("Valid relational operator.");
        else
            Console.WriteLine("Invalid relational operator.");
    }
}

/*
==	Valid relational operator.
!=	Valid relational operator.
<=	Valid relational operator.
<>	Invalid relational operator.
greater	Invalid relational operator.
*/ 
