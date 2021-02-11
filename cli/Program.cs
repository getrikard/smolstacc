using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

namespace cli
{
    class Program
    {
        private static readonly Stack<int> Stack = new Stack<int>();

        private static readonly Dictionary<string, Action> Library = new Dictionary<string, Action>
        {
            // std
            {".", () =>
                {
                    Console.WriteLine(Stack.Pop());
                }
            },

            {":", () =>
            {

            }},

            // math
            {"+", () =>
            {
                if (!IsStackLongEnough(2)) return;

                var a = Stack.Pop();
                var b = Stack.Pop();

                var result = b + a;

                Stack.Push(result);
            }},

            {"-", () =>
            {
                if (!IsStackLongEnough(2)) return;

                var a = Stack.Pop();
                var b = Stack.Pop();
                var result = b - a;

                Stack.Push(result);
            }},

            {"*", () =>
            {
                if (!IsStackLongEnough(2)) return;

                var a = Stack.Pop();
                var b = Stack.Pop();
                var result = b * a;

                Stack.Push(result);
            }},

            {"/", () =>
            {
                if (!IsStackLongEnough(2)) return;

                var a = Stack.Pop();
                var b = Stack.Pop();
                var result = b / a;

                Stack.Push(result);
            }},

            {"^2", () =>
            {
                if (!IsStackLongEnough(2)) return;

                var n = Stack.Pop();

                Stack.Push(n * n);
            }},

            // logic
            {">", () =>
            {
                if (!IsStackLongEnough(2)) return;

                var a = Stack.Pop();
                var b = Stack.Pop();
                var result = b > a;

                Stack.Push(result ? 1 : 0);
            }},

            {"<", () =>
            {
                if (!IsStackLongEnough(2)) return;

                var a = Stack.Pop();
                var b = Stack.Pop();
                var result = b < a;

                Stack.Push(result ? 1 : 0);
            }},

            {"=", () =>
            {
                if (!IsStackLongEnough(2)) return;

                var a = Stack.Pop();
                var b = Stack.Pop();
                var result = b == a;

                Stack.Push(result ? 1 : 0);
            }},

            {"?", () =>
            {
                if (!IsStackLongEnough(1)) return;

                Stack.Push(Stack.Pop() > 0 ? 1 : 0);
            }},

            {"dup", () =>
            {
                if (!IsStackLongEnough(1)) return;

                var n = Stack.Pop();

                Stack.Push(n);
                Stack.Push(n);
            }},

        };

        private static readonly Dictionary<string, string> Vocabulary = new Dictionary<string, string>();

        static void Main(string[] args)
        {
            var isDefiningNewWord = false;
            List<string> newWordDef = new List<string>();

            while (true)
            {
                var line = PrintStackAndGetInput();
                if (line == null) continue;

                var words = line.Split(' ', '\n', '\t', '\r');
                foreach (var word in words.Select(w => w.Trim()))
                {
                    if (word == ":")
                    {
                        isDefiningNewWord = true;
                        newWordDef = new List<string>();
                        continue;
                    }

                    if (word == ";")
                    {
                        isDefiningNewWord = false;

                        if (Vocabulary.ContainsKey(newWordDef[0]))
                        {
                            Vocabulary[newWordDef[0]] = string.Join(' ', newWordDef.Skip(1));
                        }
                        else
                        {
                            Vocabulary.Add(newWordDef[0], string.Join(' ', newWordDef.Skip(1)));
                        }

                        //Console.WriteLine($"\n{{: {string.Join(", ", Vocabulary)} ;}}");
                        continue;
                    }

                    if (isDefiningNewWord)
                    {
                        newWordDef.Add(word);
                    }
                    else
                    {
                        TryExecuteWord(word);
                    }
                }
            }
        }

        static void TryExecuteWord(string word)
        {
            try
            {
                Stack.Push(int.Parse(word));
            }
            catch
            {
                if (Vocabulary.ContainsKey(word))
                {
                    foreach (var subword in Vocabulary[word].Split(' ', '\n', '\t', '\r'))
                    {
                        TryExecuteWord(subword);
                    }
                }
                else if (Library.ContainsKey(word))
                {
                    Library[word]();
                }
                else
                {
                    if (word != "") Console.WriteLine("Unknown word.");
                }
            }
        }

        static bool IsStackLongEnough(int length)
        {
            if (Stack.Count >= length) return true;
            Console.WriteLine($"Not enough items on stack. Need > {length}.");
            return false;

        }

        static string PrintStackAndGetInput()
        {
            // var stdin = Console.In;
            // var stdin = Console.OpenStandardInput();

            Console.WriteLine($"\n[ {string.Join(", ", Stack.Reverse())} ]");
            Console.Write("> ");
            return Console.In.ReadLine(); // TODO: Sjekk med Console.ReadLine() også.
        }
    }
}
