using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MiniLanguageParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Mini-Language Parser");
            Console.WriteLine("-------------------");
            Console.WriteLine("Enter your code (example: var a1 = 12@; float b2 = 3.14$$;):");

            string inputCode = Console.ReadLine();

            // Process the input code
            ProcessCode(inputCode);

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        static void ProcessCode(string code)
        {
            // Regex pattern to match variables that:
            // 1. Start with a, b, or c
            // 2. End with digits
            // 3. Have non-alphanumeric special characters in the value
            string pattern = @"((?:var|float|int|double|string)\s+([abc]\w*\d+)\s*=\s*([^;]*?[^\w\s;][^;]*);)";

            var matches = Regex.Matches(code, pattern);

            if (matches.Count == 0)
            {
                Console.WriteLine("No matching variables found.");
                return;
            }

            // Store results
            var results = new List<(string VarName, string SpecialSymbol, string TokenType)>();

            foreach (Match match in matches)
            {
                string fullDeclaration = match.Groups[1].Value;
                string varName = match.Groups[2].Value;
                string value = match.Groups[3].Value.Trim();

                // Extract special symbols
                string specialSymbols = Regex.Replace(value, @"[\w\s\.]", "");

                // Determine token type
                string tokenType = DetermineTokenType(fullDeclaration);

                if (!string.IsNullOrEmpty(specialSymbols))
                {
                    results.Add((varName, specialSymbols, tokenType));
                }
            }

            // Display results in a table
            if (results.Count > 0)
            {
                DisplayResultsTable(results);
            }
            else
            {
                Console.WriteLine("No variables with special characters found.");
            }
        }

        static string DetermineTokenType(string declaration)
        {
            if (declaration.StartsWith("var"))
                return "Variable";
            else if (declaration.StartsWith("int"))
                return "Integer";
            else if (declaration.StartsWith("float"))
                return "Float";
            else if (declaration.StartsWith("double"))
                return "Double";
            else if (declaration.StartsWith("string"))
                return "String";
            else
                return "Unknown";
        }

        static void DisplayResultsTable(List<(string VarName, string SpecialSymbol, string TokenType)> results)
        {
            // Calculate column widths
            int varNameWidth = Math.Max("VarName".Length, results.Max(r => r.VarName.Length)) + 2;
            int specialSymbolWidth = Math.Max("SpecialSymbol".Length, results.Max(r => r.SpecialSymbol.Length)) + 2;
            int tokenTypeWidth = Math.Max("Token Type".Length, results.Max(r => r.TokenType.Length)) + 2;

            // Draw table header
            string headerLine = "+" + new string('-', varNameWidth + 2) + "+" +
                               new string('-', specialSymbolWidth + 2) + "+" +
                               new string('-', tokenTypeWidth + 2) + "+";

            Console.WriteLine(headerLine);
            Console.WriteLine("| " + "VarName".PadRight(varNameWidth) +
                             "| " + "SpecialSymbol".PadRight(specialSymbolWidth) +
                             "| " + "Token Type".PadRight(tokenTypeWidth) + "|");
            Console.WriteLine(headerLine);

            // Draw table rows
            foreach (var result in results)
            {
                Console.WriteLine("| " + result.VarName.PadRight(varNameWidth) +
                                 "| " + result.SpecialSymbol.PadRight(specialSymbolWidth) +
                                 "| " + result.TokenType.PadRight(tokenTypeWidth) + "|");
            }

            // Draw table footer
            Console.WriteLine(headerLine);
        }
    }
}