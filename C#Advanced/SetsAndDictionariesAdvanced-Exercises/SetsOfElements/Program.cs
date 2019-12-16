using System;
using System.Collections.Generic;
using System.Linq;

namespace SetsOfElements
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] setsCount = Console.ReadLine().
                Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            int firstSetCount = setsCount[0];
            int secondSetCount = setsCount[1];

            HashSet<int> firstSet = new HashSet<int>();
            HashSet<int> secondSet = new HashSet<int>();

            for (int i = 1; i <= firstSetCount; i++)
            {
                int number = int.Parse(Console.ReadLine());
                firstSet.Add(number);
            }
            for (int i = firstSetCount + 1; i <= firstSetCount + secondSetCount; i++)
            {
                int numberSecond = int.Parse(Console.ReadLine());
                secondSet.Add(numberSecond);
            }
            List<int> numbersInBothSets = new List<int>();

            foreach (var number in firstSet)
            {
                if (secondSet.Contains(number))
                {
                    numbersInBothSets.Add(number);
                }
            }

            Console.WriteLine(string.Join(" ", numbersInBothSets));
        }
    }
}
