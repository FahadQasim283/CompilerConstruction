using System;
using System.Data;
using System.Collections.Generic;
class Program
{
    static void Main()
    {
        int var34 = 0; 

        // Hardcoded values for x and y
        int x = 3;
        int y = 4;

        string input = "x:userinput; y:userinput; z:4; result: x * y + z;";
        Dictionary<string, int> variables = new Dictionary<string, int>();

        // Pre-populate x and y with hardcoded values
        variables["x"] = x;
        variables["y"] = y;

        string expression = "";
        string[] segments = input.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string segment in segments)
        {
            string[] parts = segment.Split(':');
            if (parts.Length < 2)
                continue;
            string name = parts[0].Trim();
            string value = parts[1].Trim();

            if (name.ToLower() == "result")
            {
                expression = value;
            }
            else if (name.ToLower() == "z") // Always get z from user
            {
                Console.Write($"Enter value for {name}: ");
                int inputValue;
                while (!int.TryParse(Console.ReadLine(), out inputValue))   
                {
                    Console.Write("Invalid input. Please enter an integer: ");
                }
                variables[name] = inputValue;
            }
            // Skip x and y as we've already set them with hardcoded values
            else if (name.ToLower() != "x" && name.ToLower() != "y")
            {
                if (value == "userinput")
                {
                    Console.Write($"Enter value for {name}: ");
                    int inputValue;
                    while (!int.TryParse(Console.ReadLine(), out inputValue))
                    {
                        Console.Write("Invalid input. Please enter an integer: ");
                    }
                    variables[name] = inputValue;
                }
                else
                {
                    if (int.TryParse(value, out int numValue))
                    {
                        variables[name] = numValue;
                    }
                    else
                    {
                        Console.WriteLine($"Invalid value for {name}: {value}. Defaulting to 0.");
                        variables[name] = 0;
                    }
                }
            }
        }

        foreach (var variable in variables)
        {
            expression = expression.Replace(variable.Key, variable.Value.ToString());
        }

        DataTable table = new DataTable();
        int result = Convert.ToInt32(table.Compute(expression, ""));

        foreach (var variable in variables)
        {
            Console.WriteLine($"{variable.Key} = {variable.Value}");
        }
        Console.WriteLine($"Result = {result}");
    }
}