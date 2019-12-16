using System;
using System.Collections.Generic;
using System.Linq;

namespace CupsAndBottles
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] cupsCapacity = Console.ReadLine()
                .Split()
                .Select(int.Parse)
                .ToArray();

            int[] bottlesCapacity = Console.ReadLine()
                .Split()
                .Select(int.Parse)
                .ToArray();

            int wastedWater = 0;

            Queue<int> cups = new Queue<int>(cupsCapacity);
            Stack<int> bottles = new Stack<int>(bottlesCapacity);
 
            while (cups.Count > 0 && bottles.Count > 0)
            {
                if (bottles.Peek() >= cups.Peek())
                {
                    wastedWater += bottles.Pop() - cups.Dequeue();
                }
                else if (bottles.Peek() < cups.Peek())
                {
                    int cupCapacity = cups.Peek();
                    while (cupCapacity > 0)
                    {
                        cupCapacity -= bottles.Peek();
                        if (cupCapacity <= 0)
                        {
                            wastedWater += Math.Abs(bottles.Peek() - cupCapacity - bottles.Pop());
                            cups.Dequeue();
                        }
                        else
                        {
                            bottles.Pop();
                        }
                    }
                    
                }
            }
            if (bottles.Count > 0)
            {
                Console.WriteLine($"Bottles: {string.Join(" ", bottles)}");
            }
            else if (cups.Count > 0)
            {
                Console.WriteLine($"Cups: {string.Join(" ", cups)}");
            }
            Console.WriteLine($"Wasted litters of water: {wastedWater}");
        }
    }
}
