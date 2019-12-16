using System;
using System.Collections.Generic;

namespace PeriodicTable
{
    class Program
    {
        static void Main(string[] args)
        {
            int compounds = int.Parse(Console.ReadLine());

            SortedSet<string> elements = new SortedSet<string>();

            for (int i = 0; i < compounds; i++)
            {
                string[] input = Console.ReadLine()
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                foreach (var element in input)
                {
                    elements.Add(element);
                }
            }
            Console.WriteLine(string.Join(" ", elements));
        }
    }
}
