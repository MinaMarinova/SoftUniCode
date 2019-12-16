using System;
using System.Collections.Generic;
using System.Linq;

namespace FashionBoutique
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] clothes = Console.ReadLine()
                .Split(' ')
                .Select(int.Parse)
                .ToArray();

            int capacityRack = int.Parse(Console.ReadLine());

            int sum = 0;
            int rack = 1;

            Stack<int> clothesValue = new Stack<int>(clothes);

            if (clothesValue.Count == 0)
            {
                Console.WriteLine("0");
                return;
            }

            while (clothesValue.Count > 0)
            {
                if (sum + clothesValue.Peek() <= capacityRack)
                {
                    sum += clothesValue.Pop();
                }
                else if (sum + clothesValue.Peek() > capacityRack)
                {
                    rack++;
                    sum = clothesValue.Pop();
                }
                
            }

            Console.WriteLine(rack);
        }
    }
}
