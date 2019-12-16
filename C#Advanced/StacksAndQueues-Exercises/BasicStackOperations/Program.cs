using System;
using System.Collections.Generic;
using System.Linq;

namespace BasicStackOperations
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] numbers = Console.ReadLine()
                .Split(' ')
                .Select(int.Parse)
                .ToArray();

            int countNumbers = numbers[0];
            int numbersToPop = numbers[1];
            int checkNumber = numbers[2];

            int smallestNumber = int.MaxValue;

            int[] numbersToPush = Console.ReadLine()
                .Split(' ')
                .Select(int.Parse)
                .ToArray();

            Stack<int> numbersInStack = new Stack<int>(numbersToPush);

            for (int i = 0; i < numbersToPop; i++)
            {
                numbersInStack.Pop();
            }

            if (numbersInStack.Count == 0)
            {
                Console.WriteLine("0");
                return;
            }

            while (numbersInStack.Count > 0)
            {
                int currentNumber = numbersInStack.Pop();
                if (currentNumber == checkNumber)
                {
                    Console.WriteLine("true");
                    return;
                }
                else
                {
                    if (currentNumber <= smallestNumber)
                    {
                        smallestNumber = currentNumber;
                    }
                }
            }
            Console.WriteLine(smallestNumber);
        }
    }
}
