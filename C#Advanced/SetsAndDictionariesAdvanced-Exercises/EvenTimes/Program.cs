using System;
using System.Collections.Generic;

namespace EvenTimes
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine());

            HashSet<int> numbersInSet = new HashSet<int>();
            List<int> numbersInList = new List<int>();
            List<int> evenTimes = new List<int>();

            for (int i = 0; i < n; i++)
            {
                int input = int.Parse(Console.ReadLine());

                numbersInList.Add(input);
                numbersInSet.Add(input);
            }
            foreach (var item in numbersInSet)
            {
                int count = 0;
                while (numbersInList.Contains(item))
                {
                    numbersInList.Remove(item);
                    count++;
                }
                if (count % 2 == 0)
                {
                    evenTimes.Add(item);
                }
            }
            Console.WriteLine(string.Join(" ",evenTimes));
        }
    }
}
