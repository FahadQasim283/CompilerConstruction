using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace SemanticAnalyzerLab
{
    class Program
    {
        static List<List<string>> Symboltable = new List<List<string>>();
        static List<string> finalArray = new List<string>();
        static List<int> Constants = new List<int>();
        static Regex variable_Reg = new Regex(@"^[A-Za-z_][A-Za-z0-9]*$");

        // Parsing table (ACTION and GOTO)
        static Dictionary<(int, string), string> actionTable = new Dictionary<(int, string), string>
        {
            // State 0: Start
            {(0, "int"), "s1"}, {(0, "id"), "s4"}, {(0, "$"), "r2"},
            // State 1: After int
            {(1, "id"), "s2"},
            // State 2: After int id
            {(2, ";"), "s3"},
            // State 3: After int id ;
            {(3, "int"), "s1"}, {(3, "id"), "s4"}, {(3, "$"), "r2"},
            // State 4: After id
            {(4, "="), "s5"},
            // State 5: After id =
            {(5, "id"), "s7"}, {(5, "num"), "s8"}, {(5, "("), "s9"},
            // State 6: After E ;
            {(6, ";"), "s10"},
            // State 7: After T (id)
            {(7, "+"), "s11"}, {(7, "-"), "s12"}, {(7, "*"), "s13"}, {(7, "/"), "s14"}, {(7, ";"), "r6"},
            // State 8: After T (num)
            {(8, "+"), "r8"}, {(8, "-"), "r8"}, {(8, "*"), "s13"}, {(8, "/"), "s14"}, {(8, ";"), "r8"},
            // State 9: After (
            {(9, "id"), "s7"}, {(9, "num"), "s8"}, {(9, "("), "s9"},
            // State 10: After S
            {(10, "int"), "s1"}, {(10, "id"), "s4"}, {(10, "$"), "r2"},
            // State 11: After E +
            {(11, "id"), "s7"}, {(11, "num"), "s8"}, {(11, "("), "s9"},
            // State 12: After E -
            {(12, "id"), "s7"}, {(12, "num"), "s8"}, {(12, "("), "s9"},
            // State 13: After T *
            {(13, "id"), "s7"}, {(13, "num"), "s8"}, {(13, "("), "s9"},
            // State 14: After T /
            {(14, "id"), "s7"}, {(14, "num"), "s8"}, {(14, "("), "s9"},
            // State 15: After E )
            {(15, ")"), "s16"},
            // State 16: After ( E )
            {(16, "+"), "r8"}, {(16, "-"), "r8"}, {(16, "*"), "s13"}, {(16, "/"), "s14"}, {(16, ";"), "r8"},
            // State 17: Accept
            {(17, "$"), "acc"}
        };

        static Dictionary<(int, string), int> gotoTable = new Dictionary<(int, string), int>
        {
            {(0, "S"), 17}, {(0, "P"), 18}, {(0, "D"), 19},
            {(3, "P"), 20}, {(3, "D"), 19}, {(3, "S"), 21},
            {(5, "E"), 6}, {(5, "T"), 22},
            {(7, "T"), 23}, {(8, "T"), 24}, {(9, "E"), 15}, {(9, "T"), 22},
            {(10, "P"), 20}, {(10, "D"), 19}, {(10, "S"), 25},
            {(11, "T"), 26}, {(12, "T"), 27}, {(13, "T"), 28}, {(14, "T"), 29}
        };

        static Stack<int> stateStack = new Stack<int>();
        static Stack<string> symbolStack = new Stack<string>();
        static int tokenIndex;

        static void Main(string[] args)
        {
            Console.WriteLine("Starting program...");
            Console.Out.Flush();

            InitializeSymbolTable();
            InitializeFinalArray();
            PrintLexerOutput();

            // Perform SLR parsing
            if (Parse())
            {
                Console.WriteLine("\nSyntax analysis completed successfully.");
                Console.Out.Flush();
                // Perform semantic analysis only if parsing succeeds
                for (int i = 0; i < finalArray.Count; i++)
                {
                    Semantic_Analysis(i);
                }
                Console.WriteLine("\nSemantic Analysis Completed.");
                Console.Out.Flush();
            }
            else
            {
                Console.WriteLine("\nSyntax analysis failed.");
                Console.Out.Flush();
            }
        }

        static void InitializeSymbolTable()
        {
            Symboltable.Add(new List<string> { "x", "id", "int", "0" });
            Symboltable.Add(new List<string> { "y", "id", "int", "0" });
            Symboltable.Add(new List<string> { "i", "id", "int", "0" });
            Symboltable.Add(new List<string> { "l", "id", "char", "0" });
        }

        static void InitializeFinalArray()
        {
            finalArray.AddRange(new string[] {
                "int", "x", ";",
                "x", "=", "2", "+", "5", "+", "(", "4", "*", "8", ")", "+", "l", "/", "9.0", ";"
            });
        }

        static void PrintLexerOutput()
        {
            Console.WriteLine("Tokenizing input...");
            int row = 1, col = 1;
            foreach (string token in finalArray)
            {
                if (token == "int") Console.WriteLine($"INT ({row},{col})");
                else if (token == ";") Console.WriteLine($"SEMI ({row},{col})");
                else if (token == "=") Console.WriteLine($"ASSIGN ({row},{col})");
                else if (token == "+") Console.WriteLine($"PLUS ({row},{col})");
                else if (token == "-") Console.WriteLine($"MINUS ({row},{col})");
                else if (token == "*") Console.WriteLine($"TIMES ({row},{col})");
                else if (token == "/") Console.WriteLine($"DIV ({row},{col})");
                else if (token == "(") Console.WriteLine($"LPAREN ({row},{col})");
                else if (token == ")") Console.WriteLine($"RPAREN ({row},{col})");
                else if (Regex.IsMatch(token, @"^[0-9]+$")) Console.WriteLine($"INT_CONST ({row},{col}): {token}");
                else if (Regex.IsMatch(token, @"^[0-9]+\.[0-9]+$")) Console.WriteLine($"FLOAT_CONST ({row},{col}): {token}");
                else if (variable_Reg.Match(token).Success) Console.WriteLine($"ID ({row},{col}): {token}");
                else Console.WriteLine($"UNKNOWN ({row},{col}): {token}");

                col += token.Length + 1;
                if (token == ";") row++;
                Console.Out.Flush();
            }
            Console.WriteLine($"EOF ({row},{col})");
            Console.Out.Flush();
        }

        static bool Parse()
        {
            stateStack.Clear();
            symbolStack.Clear();
            stateStack.Push(0);
            tokenIndex = 0;

            Console.WriteLine("\nParsing steps:");
            Console.WriteLine("Stack\t\tInput\t\tAction");
            Console.WriteLine("---------------------------------------------");
            Console.Out.Flush();

            while (tokenIndex < finalArray.Count)
            {
                int currentState = stateStack.Peek();
                string currentToken = finalArray[tokenIndex];
                if (Regex.IsMatch(currentToken, @"^[0-9]+$")) currentToken = "num";
                if (Regex.IsMatch(currentToken, @"^[0-9]+\.[0-9]+$")) currentToken = "float";

                // Print current state
                PrintParseState(currentToken);

                if (!actionTable.TryGetValue((currentState, currentToken), out string action))
                {
                    Console.WriteLine($"Parsing error at state {currentState}, token {currentToken}");
                    Console.Out.Flush();
                    return false;
                }

                if (action.StartsWith("s"))
                {
                    // Shift
                    int nextState = int.Parse(action.Substring(1));
                    stateStack.Push(nextState);
                    symbolStack.Push(currentToken);
                    tokenIndex++;
                }
                else if (action.StartsWith("r"))
                {
                    // Reduce
                    int rule = int.Parse(action.Substring(1));
                    (string lhs, int popCount) = GetProduction(rule);

                    // Pop states and symbols
                    for (int i = 0; i < popCount; i++)
                    {
                        stateStack.Pop();
                        symbolStack.Pop();
                    }

                    // Push non-terminal and new state
                    int newState = stateStack.Peek();
                    symbolStack.Push(lhs);
                    if (!gotoTable.TryGetValue((newState, lhs), out int gotoState))
                    {
                        Console.WriteLine($"No GOTO for state {newState}, non-terminal {lhs}");
                        Console.Out.Flush();
                        return false;
                    }
                    stateStack.Push(gotoState);
                }
                else if (action == "acc")
                {
                    return true;
                }
            }

            // Check for accept at end of input
            if (actionTable.TryGetValue((stateStack.Peek(), "$"), out string finalAction) && finalAction == "acc")
            {
                return true;
            }

            Console.WriteLine("Parsing error: Input not fully consumed");
            Console.Out.Flush();
            return false;
        }

        static (string, int) GetProduction(int rule)
        {
            switch (rule)
            {
                case 0: return ("S", 1); // S → P
                case 1: return ("P", 3); // P → D P
                case 2: return ("P", 0); // P → ε
                case 3: return ("P", 3); // P → S P
                case 4: return ("D", 3); // D → int id ;
                case 5: return ("S", 4); // S → id = E ;
                case 6: return ("E", 1); // E → T
                case 7: return ("E", 3); // E → E + T
                case 8: return ("T", 1); // T → id
                case 9: return ("T", 1); // T → num
                case 10: return ("T", 3); // T → ( E )
                case 11: return ("T", 3); // T → T * T
                case 12: return ("T", 3); // T → T / T
                case 13: return ("E", 3); // E → E - T
                default: throw new Exception($"Invalid rule: {rule}");
            }
        }

        static void PrintParseState(string currentToken)
        {
            string stackStr = string.Join(" ", stateStack);
            string inputStr = tokenIndex < finalArray.Count
                ? string.Join(" ", finalArray.GetRange(tokenIndex, finalArray.Count - tokenIndex))
                : "$";
            string action = actionTable.TryGetValue((stateStack.Peek(), currentToken), out string act) ? act : "error";
            Console.WriteLine($"{stackStr}\t\t{inputStr}\t\t{action}");
            Console.Out.Flush();
        }

        static void Semantic_Analysis(int k)
        {
            if (k >= finalArray.Count) return;

            if (finalArray[k] == "+" || finalArray[k] == "-")
            {
                if (k > 0 && k < finalArray.Count - 1 &&
                    variable_Reg.Match(finalArray[k - 1]).Success &&
                    variable_Reg.Match(finalArray[k + 1]).Success)
                {
                    string type = finalArray[k - 4];
                    string left_side = finalArray[k - 3];
                    string before = finalArray[k - 1];
                    string after = finalArray[k + 1];

                    int left_side_i = FindSymbol(left_side);
                    int before_i = FindSymbol(before);
                    int after_i = FindSymbol(after);

                    if (left_side_i == -1 || before_i == -1 || after_i == -1)
                    {
                        Console.WriteLine($"Semantic error: Undefined variable at index {k}");
                        Console.Out.Flush();
                        return;
                    }

                    if (type == Symboltable[before_i][2] && type == Symboltable[after_i][2])
                    {
                        int ans = finalArray[k] == "+" ?
                            Convert.ToInt32(Symboltable[before_i][3]) + Convert.ToInt32(Symboltable[after_i][3]) :
                            Convert.ToInt32(Symboltable[before_i][3]) - Convert.ToInt32(Symboltable[after_i][3]);
                        Constants.Add(ans);
                    }

                    if (Symboltable[left_side_i][2] == Symboltable[before_i][2] &&
                        Symboltable[left_side_i][2] == Symboltable[after_i][2])
                    {
                        int ans = finalArray[k] == "+" ?
                            Convert.ToInt32(Symboltable[before_i][3]) + Convert.ToInt32(Symboltable[after_i][3]) :
                            Convert.ToInt32(Symboltable[before_i][3]) - Convert.ToInt32(Symboltable[after_i][3]);
                        if (Constants.Count > 0) Constants.RemoveAt(Constants.Count - 1);
                        Constants.Add(ans);
                        Symboltable[left_side_i][3] = ans.ToString();
                    }
                    else
                    {
                        Console.WriteLine($"Semantic error: Type mismatch at index {k}");
                        Console.Out.Flush();
                    }
                }
            }
            else if (finalArray[k] == "/" && finalArray[k + 1] == "float")
            {
                Console.WriteLine($"Semantic error: Float operations not supported at index {k}");
                Console.Out.Flush();
            }
        }

        static int FindSymbol(string name)
        {
            for (int i = 0; i < Symboltable.Count; i++)
            {
                if (Symboltable[i][0] == name)
                    return i;
            }
            return -1;
        }
    }
}
