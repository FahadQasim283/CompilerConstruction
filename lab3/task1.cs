using System;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        Console.WriteLine("""
        a regular expression to validate floating-point numbers with a maximum length of 6. Let me know if you need further refinements!\n
        """);
        Console.WriteLine("Enter a floating-point number:");
        string input = Console.ReadLine();

        Regex floatRegex = new Regex(@"^\d{1,6}(\.\d{1,6})?$");

        if (floatRegex.IsMatch(input))
            Console.WriteLine("Valid floating-point number.");
        else
            Console.WriteLine("Invalid floating-point number.");
    }
}
/*
123.45	    Valid floating-point number.
0.1	        Valid floating-point number.
1000000.1	Invalid floating-point number.
12.1234567	Invalid floating-point number.
.45	        Invalid floating-point number.
*/ 