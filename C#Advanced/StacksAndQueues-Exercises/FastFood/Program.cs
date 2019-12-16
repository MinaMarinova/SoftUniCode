using System;
using System.Collections.Generic;
using System.Linq;

namespace FastFood
{
    class Program
    {
        static void Main(string[] args)
        {
            int foodQuantity = int.Parse(Console.ReadLine());

            int[] orders = Console.ReadLine()
                .Split(' ')
                .Select(int.Parse)
                .ToArray();

            int biggestOrder = int.MinValue;
            Queue<int> ordersInQueue = new Queue<int>(orders);

            for (int i = 0; i < orders.Length; i++)
            {
                if (orders[i] >= biggestOrder)
                {
                    biggestOrder = orders[i];
                }
            }
            Console.WriteLine(biggestOrder);

            while(ordersInQueue.Count > 0)
            {
                if (ordersInQueue.Peek() <= foodQuantity)
                {
                    foodQuantity -= ordersInQueue.Peek();
                    ordersInQueue.Dequeue();
                }
                else
                {
                    Console.Write("Orders left: ");
                    while (ordersInQueue.Count > 0)
                    {
                        Console.Write($"{ordersInQueue.Dequeue()} ");
                    }
                    Console.WriteLine();
                    return;
                }
            }

            Console.WriteLine("Orders complete");
        }
    }
}
