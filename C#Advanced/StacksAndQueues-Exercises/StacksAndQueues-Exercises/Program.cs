using System;
using System.Collections.Generic;
using System.Linq;

namespace ReverseNumbersWithStack
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] numbers = Console.ReadLine()
                .Split(' ')
                .Select(int.Parse)
                .ToArray();

            Stack<int> numbersInStack = new Stack<int>(numbers);
            
            
            while (numbersInStack.Count > 0)
            {
                Console.Write($"{numbersInStack.Pop()} ");
            }
            Console.WriteLine();
        }
    }
}
