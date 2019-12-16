using System;
using System.Collections.Generic;
using System.Linq;

namespace MaximumAndMinimumElement
{
    class Program
    {
        static void Main(string[] args)
        {
            int numberOfQueries = int.Parse(Console.ReadLine());

            int maxElement = int.MinValue;
            int minElement = int.MaxValue;

            Stack<int> numbers = new Stack<int>();

            for (int i = 0; i < numberOfQueries; i++)
            {
                int[] query = Console.ReadLine()
                    .Split(' ')
                    .Select(int.Parse)
                    .ToArray();

                if (query[0] == 1)
                {
                    numbers.Push(query[1]);
                }
                else if (query[0] == 2 && numbers.Count > 0)
                {
                    numbers.Pop();
                }
                else if (query[0] == 3 && numbers.Count > 0)
                {
                    foreach (var item in numbers.ToArray())
                    {
                        if (item >= maxElement)
                        {
                            maxElement = item;
                        }
                    }
                    Console.WriteLine(maxElement);
                    
                }
                else if (query[0] == 4 && numbers.Count > 0)
                {
                    foreach (var item in numbers.ToArray())
                    {
                        if (item <= minElement)
                        {
                            minElement = item;
                        }
                    }
                    Console.WriteLine(minElement);
                }
            }
            if (numbers.Count > 0)
            {
                while (numbers.Count > 0)
                {
                    if (numbers.Count != 1)
                    {
                        Console.Write($"{numbers.Pop()}, ");
                    }
                    else if (numbers.Count == 1)
                    {
                        Console.WriteLine(numbers.Pop());
                    }
                }
            }
        }
    }
}
