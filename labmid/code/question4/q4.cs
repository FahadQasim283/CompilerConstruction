using System;
using System.Collections.Generic;
using System.Linq;

public class GrammarProcessor
{
    private Dictionary<string, List<List<string>>> grammar = new Dictionary<string, List<List<string>>>();
    private HashSet<string> nonTerminals = new HashSet<string>();
    private HashSet<string> terminals = new HashSet<string>();
    private Dictionary<string, HashSet<string>> firstSets = new Dictionary<string, HashSet<string>>();
    private Dictionary<string, HashSet<string>> followSets = new Dictionary<string, HashSet<string>>();
    private string startSymbol;

    public void ProcessGrammar()
    {
        ReadGrammar();
        CheckLeftRecursion();
        ComputeFirstSets();
        ComputeFollowSets();
        CheckAmbiguity();
        PrintFirstAndFollow();
    }

    private void ReadGrammar()
    {
        Console.WriteLine("Enter grammar rules (e.g., 'E -> T X | ε'). Enter empty line to finish.");
        string line;
        while (true)
        {
            line = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(line)) break;

            string[] parts = line.Split(new[] { "->" }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
            {
                Console.WriteLine("Invalid rule format. Use 'NonTerminal -> Production1 | Production2'");
                continue;
            }

            string lhs = parts[0].Trim();
            if (!nonTerminals.Contains(lhs))
            {
                nonTerminals.Add(lhs);
                if (startSymbol == null)
                    startSymbol = lhs;
            }

            string[] productions = parts[1].Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            List<List<string>> productionList = new List<List<string>>();
            foreach (var prod in productions)
            {
                List<string> symbols = prod.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                productionList.Add(symbols);
                foreach (var symbol in symbols)
                {
                    if (symbol != "ε" && !nonTerminals.Contains(symbol) && !terminals.Contains(symbol))
                        terminals.Add(symbol);
                }
            }
            grammar[lhs] = productionList;
        }

        terminals.Add("$");
    }

    private void CheckLeftRecursion()
    {
        foreach (var nt in nonTerminals)
        {
            if (grammar.TryGetValue(nt, out var productions))
            {
                foreach (var production in productions)
                {
                    if (production.Count > 0 && production[0] == nt)
                    {
                        Console.WriteLine($"Left recursion found in {nt} -> {string.Join(" ", production)}");
                        Console.WriteLine("Grammar invalid for top-down parsing.");
                        Environment.Exit(0);
                    }
                }
            }
        }
    }

    private void ComputeFirstSets()
    {
        foreach (var t in terminals)
            firstSets[t] = new HashSet<string> { t };
        foreach (var nt in nonTerminals)
            firstSets[nt] = new HashSet<string>();

        bool changed;
        do
        {
            changed = false;
            foreach (var nt in nonTerminals)
            {
                foreach (var production in grammar[nt])
                {
                    HashSet<string> currentFirst = new HashSet<string>();
                    bool canDeriveEpsilon = true;
                    foreach (var symbol in production)
                    {
                        if (!firstSets.ContainsKey(symbol))
                            firstSets[symbol] = new HashSet<string>();

                        var symbolFirst = firstSets[symbol];
                        var withoutEpsilon = symbolFirst.Where(f => f != "ε").ToHashSet();
                        currentFirst.UnionWith(withoutEpsilon);

                        if (!symbolFirst.Contains("ε"))
                        {
                            canDeriveEpsilon = false;
                            break;
                        }
                    }
                    if (canDeriveEpsilon)
                        currentFirst.Add("ε");

                    int before = firstSets[nt].Count;
                    firstSets[nt].UnionWith(currentFirst);
                    if (firstSets[nt].Count > before)
                        changed = true;
                }
            }
        } while (changed);
    }

    private void ComputeFollowSets()
    {
        foreach (var nt in nonTerminals)
            followSets[nt] = new HashSet<string>();

        followSets[startSymbol].Add("$");

        bool changed;
        do
        {
            changed = false;
            foreach (var nt in nonTerminals)
            {
                foreach (var production in grammar[nt])
                {
                    for (int i = 0; i < production.Count; i++)
                    {
                        string B = production[i];
                        if (!nonTerminals.Contains(B))
                            continue;

                        List<string> beta = production.Skip(i + 1).ToList();
                        HashSet<string> firstBeta = ComputeFirstOfSequence(beta);
                        HashSet<string> firstBetaWithoutEpsilon = firstBeta.Where(f => f != "ε").ToHashSet();

                        int before = followSets[B].Count;
                        followSets[B].UnionWith(firstBetaWithoutEpsilon);
                        if (followSets[B].Count > before)
                            changed = true;

                        if (firstBeta.Contains("ε"))
                        {
                            before = followSets[B].Count;
                            followSets[B].UnionWith(followSets[nt]);
                            if (followSets[B].Count > before)
                                changed = true;
                        }
                    }
                }
            }
        } while (changed);
    }

    private HashSet<string> ComputeFirstOfSequence(List<string> symbols)
    {
        HashSet<string> result = new HashSet<string>();
        bool allEpsilon = true;
        foreach (var symbol in symbols)
        {
            if (!firstSets.ContainsKey(symbol))
                firstSets[symbol] = new HashSet<string>();

            var first = firstSets[symbol];
            result.UnionWith(first.Where(f => f != "ε"));
            if (!first.Contains("ε"))
            {
                allEpsilon = false;
                break;
            }
        }
        if (allEpsilon)
            result.Add("ε");
        return result;
    }

    private void CheckAmbiguity()
    {
        foreach (var nt in nonTerminals)
        {
            var productions = grammar[nt];
            for (int i = 0; i < productions.Count; i++)
            {
                var alpha = productions[i];
                var firstAlpha = ComputeFirstOfSequence(alpha);
                bool alphaDerivesEpsilon = firstAlpha.Contains("ε");

                for (int j = i + 1; j < productions.Count; j++)
                {
                    var beta = productions[j];
                    var firstBeta = ComputeFirstOfSequence(beta);
                    bool betaDerivesEpsilon = firstBeta.Contains("ε");

                    if (firstAlpha.Intersect(firstBeta).Any())
                    {
                        Console.WriteLine($"Ambiguity: Productions {i + 1} and {j + 1} of {nt} have overlapping FIRST sets.");
                        Console.WriteLine("Grammar invalid for top-down parsing.");
                        Environment.Exit(0);
                    }

                    if (alphaDerivesEpsilon && followSets[nt].Intersect(firstBeta).Any())
                    {
                        Console.WriteLine($"Ambiguity: Production {i + 1} of {nt} derives ε, and FIRST of production {j + 1} overlaps with FOLLOW({nt}).");
                        Console.WriteLine("Grammar invalid for top-down parsing.");
                        Environment.Exit(0);
                    }

                    if (betaDerivesEpsilon && followSets[nt].Intersect(firstAlpha).Any())
                    {
                        Console.WriteLine($"Ambiguity: Production {j + 1} of {nt} derives ε, and FIRST of production {i + 1} overlaps with FOLLOW({nt}).");
                        Console.WriteLine("Grammar invalid for top-down parsing.");
                        Environment.Exit(0);
                    }
                }
            }
        }
    }

    private void PrintFirstAndFollow()
    {
        Console.WriteLine("\nFIRST Sets:");
        foreach (var nt in nonTerminals)
            Console.WriteLine($"FIRST({nt}) = {{ {string.Join(", ", firstSets[nt])} }}");

        Console.WriteLine("\nFOLLOW Sets:");
        foreach (var nt in nonTerminals)
            Console.WriteLine($"FOLLOW({nt}) = {{ {string.Join(", ", followSets[nt])} }}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        GrammarProcessor processor = new GrammarProcessor();
        processor.ProcessGrammar();
    }
}