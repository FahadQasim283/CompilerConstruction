using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        List<SymbolEntry> symbolTable = new List<SymbolEntry>();
        int lineNumber = 1;

        Console.WriteLine("Enter variable declarations (e.g., 'int val33 = 999;')");
        Console.WriteLine("Type 'exit' to finish and show results\n");

        while (true)
        {
            Console.Write($"[Line {lineNumber}] > ");
            string inputLine = Console.ReadLine()?.Trim() ?? "";

            if (inputLine.ToLower() == "exit")
                break;

            if (TryParseLine(inputLine, out string type, out string name, out string value))
            {
                string palindrome = FindPalindromeSubstring(name);
                if (palindrome != null)
                {
                    symbolTable.Add(new SymbolEntry(
                        inputLine,
                        lineNumber,
                        type,
                        name,
                        value,
                        palindrome
                    ));
                    Console.WriteLine($"Accepted: Found palindrome '{palindrome}'");
                }
                else
                {
                    Console.WriteLine("Rejected: No palindrome ≥3 characters found");
                }
            }
            else
            {
                Console.WriteLine("Invalid format. Use: [type] [name] = [value];");
            }

            lineNumber++;
        }

        PrintSymbolTable(symbolTable);
    }

    static bool TryParseLine(string line, out string type, out string name, out string value)
    {
        type = name = value = null;
        var match = Regex.Match(line, @"^\s*(\w+)\s+(\w+)\s*=\s*(.*?)\s*;\s*$");
        if (!match.Success) return false;

        type = match.Groups[1].Value;
        name = match.Groups[2].Value;
        value = match.Groups[3].Value.Trim();
        return true;
    }

    static string FindPalindromeSubstring(string name)
    {
        for (int l = name.Length; l >= 3; l--)
        {
            for (int i = 0; i <= name.Length - l; i++)
            {
                string substr = name.Substring(i, l);
                if (IsPalindrome(substr))
                    return substr;
            }
        }
        return null;
    }

    static bool IsPalindrome(string s)
    {
        for (int i = 0; i < s.Length / 2; i++)
        {
            if (s[i] != s[s.Length - 1 - i])
                return false;
        }
        return true;
    }

    static void PrintSymbolTable(List<SymbolEntry> table)
    {
        Console.WriteLine("\n\nSYMBOL TABLE");
        Console.WriteLine("┌───────┬───────────────┬──────────┬───────────┬──────────┬───────────────┐");
        Console.WriteLine("│ Line  │ Input String  │ Type     │ Name      │ Value    │ Palindrome    │");
        Console.WriteLine("├───────┼───────────────┼──────────┼───────────┼──────────┼───────────────┤");

        foreach (var entry in table)
        {
            Console.WriteLine($"│ {entry.LineNumber,-5} │ {entry.OriginalLine,-13} │ {entry.Type,-8} │ {entry.VarName,-9} │ {entry.Value,-8} │ {entry.Palindrome,-13} │");
        }

        Console.WriteLine("└───────┴───────────────┴──────────┴───────────┴──────────┴───────────────┘");
    }
}

class SymbolEntry
{
    public string OriginalLine { get; }
    public int LineNumber { get; }
    public string Type { get; }
    public string VarName { get; }
    public string Value { get; }
    public string Palindrome { get; }

    public SymbolEntry(string line, int num, string type, string name, string value, string palindrome)
    {
        OriginalLine = line.Length > 13 ? line.Substring(0, 10) + "..." : line;
        LineNumber = num;
        Type = type;
        VarName = name;
        Value = value;
        Palindrome = palindrome;
    }
}