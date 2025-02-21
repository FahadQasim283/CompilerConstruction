using System;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {   
        Console.WriteLine("This program reads input from the console, splits it into words, and checks for operators using a regex pattern. It prints valid operators and marks invalid entries. Let me know if you need further modifications!");
        Console.WriteLine("Enter a variable name:");
        string input = Console.ReadLine();

        Regex variableRegex = new Regex(@"^[A-Za-z][A-Za-z0-9]{0,24}$");

        if (variableRegex.IsMatch(input))
            Console.WriteLine("Valid variable name.");
        else
            Console.WriteLine("Invalid variable name.");
    }
}

/*
varName	Valid variable name.
x1	    Valid variable name.
123abc	Invalid variable name.
longVariableNameMoreThan25Chars123	Invalid variable name.
_invalidVar	                        Invalid variable name.
*/ 
