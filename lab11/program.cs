using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        // Sample input tokens (parsed from source code)
        List<string> tokens = new List<string>
        {
            "int", "main", "(", ")", "{",
            "int", "x", ";",
            "int", "y", ";",
            "x", "=", "5", "+", "10", ";",
            "y", "=", "x", "+", "15", ";",
            "}"
        };

        // Initial symbol table (can be empty at start)
        List<List<string>> symbolTable = new List<List<string>>();

        // Perform semantic analysis
        SyntaxDirectedTranslator translator = new SyntaxDirectedTranslator(tokens, symbolTable);
        translator.PerformSemanticAnalysis();

        Console.WriteLine("\n\n✅ Final Symbol Table:");
        Console.WriteLine("--------------------------------------");
        Console.WriteLine("Name\tKind\t\tType\tValue");
        Console.WriteLine("--------------------------------------");
        foreach (var entry in symbolTable)
        {
            Console.WriteLine($"{entry[0]}\t{entry[1]}\t{entry[2]}\t{entry[3]}");
        }

        Console.WriteLine("\n✅ Constants Extracted:");
        foreach (var constant in translator.Constants)
        {
            Console.WriteLine($"- {constant}");
        }
    }
}

public class SyntaxDirectedTranslator
{
    private List<string> finalArray;
    private List<List<string>> Symboltable;
    public List<int> Constants;
    private Regex variable_Reg;
    private List<string> Errors;

    public SyntaxDirectedTranslator(List<string> tokens, List<List<string>> symbolTable)
    {
        finalArray = tokens;
        Symboltable = symbolTable;
        Constants = new List<int>();
        variable_Reg = new Regex(@"^[A-Za-z_][A-Za-z0-9_]*$");
        Errors = new List<string>();
    }

    public void PerformSemanticAnalysis()
    {
        ProcessProgram(0);

        if (Errors.Count > 0)
        {
            Console.WriteLine("\n❌ Semantic Errors Found:");
            foreach (string error in Errors)
                Console.WriteLine("Error: " + error);
        }
        else
        {
            Console.WriteLine("✅ Semantic analysis completed successfully.");
        }
    }

    private void ProcessProgram(int startIndex)
    {
        if (finalArray[startIndex] == "int" && finalArray[startIndex + 1] == "main")
        {
            int openBrace = finalArray.IndexOf("{", startIndex);
            ProcessDeclarations(openBrace + 1);
            ProcessStatements(openBrace + 1);
        }
        else
        {
            AddSemanticError("Program must start with 'int main()'");
        }
    }

    private void ProcessDeclarations(int index)
    {
        while (index < finalArray.Count && IsTypeSpecifier(finalArray[index]))
        {
            index = ProcessDeclaration(index);
        }
    }

    private int ProcessDeclaration(int index)
    {
        string type = finalArray[index];
        string name = finalArray[index + 1];

        if (!variable_Reg.IsMatch(name))
        {
            AddSemanticError($"Invalid variable name: {name}");
        }
        else if (IsDeclared(name))
        {
            AddSemanticError($"Variable '{name}' is already declared");
        }
        else
        {
            Symboltable.Add(new List<string> { name, "variable", type, "undefined" });
        }

        return index + 3; // Skip type, name, ;
    }

    private void ProcessStatements(int index)
    {
        while (index < finalArray.Count && finalArray[index] != "}")
        {
            string token = finalArray[index];

            if (variable_Reg.IsMatch(token) && index + 1 < finalArray.Count && finalArray[index + 1] == "=")
            {
                index = ProcessAssignment(index);
            }
            else
            {
                index++;
            }
        }
    }

    private int ProcessAssignment(int index)
    {
        string name = finalArray[index];
        int symbolIndex = GetSymbolIndex(name);

        if (symbolIndex == -1)
        {
            AddSemanticError($"Undeclared variable '{name}'");
            return FindNextSemicolon(index) + 1;
        }

        int exprStart = index + 2;
        int exprEnd = FindNextSemicolon(index);
        string type = Symboltable[symbolIndex][2];

        bool valid = true;
        for (int i = exprStart; i < exprEnd; i++)
        {
            string token = finalArray[i];

            if (IsOperator(token)) continue;
            if (IsIntegerLiteral(token))
            {
                Constants.Add(int.Parse(token));
                if (type != "int") AddSemanticError($"Cannot assign int to {type}");
            }
            else if (variable_Reg.IsMatch(token))
            {
                int otherVarIndex = GetSymbolIndex(token);
                if (otherVarIndex == -1)
                {
                    AddSemanticError($"Variable '{token}' used before declaration");
                    valid = false;
                }
                else if (Symboltable[otherVarIndex][2] != type)
                {
                    AddSemanticError($"Type mismatch in expression: {token} is {Symboltable[otherVarIndex][2]}, expected {type}");
                    valid = false;
                }
            }
        }

        if (valid)
        {
            Symboltable[symbolIndex][3] = "evaluated"; // Simplified
        }

        return exprEnd + 1;
    }

    private int FindNextSemicolon(int index)
    {
        for (int i = index; i < finalArray.Count; i++)
        {
            if (finalArray[i] == ";") return i;
        }
        return finalArray.Count - 1;
    }

    private int GetSymbolIndex(string name)
    {
        for (int i = 0; i < Symboltable.Count; i++)
        {
            if (Symboltable[i][0] == name)
                return i;
        }
        return -1;
    }

    private bool IsTypeSpecifier(string token)
    {
        return token == "int" || token == "float" || token == "char";
    }

    private bool IsDeclared(string name)
    {
        return GetSymbolIndex(name) != -1;
    }

    private bool IsOperator(string token)
    {
        return token == "+" || token == "-" || token == "*" || token == "/";
    }

    private bool IsIntegerLiteral(string token)
    {
        return int.TryParse(token, out _);
    }

    private void AddSemanticError(string msg)
    {
        Errors.Add(msg);
    }
}
