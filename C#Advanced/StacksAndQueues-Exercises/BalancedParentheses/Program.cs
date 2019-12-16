using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Try
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = Console.ReadLine();

            Stack<char> symbols = new Stack<char>();

            foreach (var symbol in input)
            {
                if (symbol == '(' || symbol == '{' || symbol == '[')
                {
                    symbols.Push(symbol);
                }
                else
                {
                    if (symbols.Any())
                    {
                        switch (symbol)
                        {
                            case ')':
                                {
                                    if (symbols.Pop() == '(')
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        Console.WriteLine("NO");
                                        return;
                                    }
                                }
                            case '}':
                                {
                                    if (symbols.Pop() == '{')
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        Console.WriteLine("NO");
                                        return;
                                    }
                                }
                            case ']':
                                {
                                    if (symbols.Pop() == '[')
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        Console.WriteLine("NO");
                                        return;
                                    }
                                }
                        }
                    }
                    else
                    {
                        Console.WriteLine("NO");
                        return;
                    }
                }
            }
            Console.WriteLine("YES");
        }
    }
}