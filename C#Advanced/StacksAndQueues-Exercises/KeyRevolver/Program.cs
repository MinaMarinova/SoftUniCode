using System;
using System.Collections.Generic;
using System.Linq;

namespace KeyRevolver
{
    class Program
    {
        static void Main(string[] args)
        {
            int pricePerBullet = int.Parse(Console.ReadLine());
            int sizeGunBarrel = int.Parse(Console.ReadLine());

            int[] bullets = Console.ReadLine()
                .Split()
                .Select(int.Parse)
                .ToArray();

            int[] locks = Console.ReadLine()
                .Split()
                .Select(int.Parse)
                .ToArray();

            int valueIntelligence = int.Parse(Console.ReadLine());

            Stack<int> bulletsInStack = new Stack<int>(bullets);
            Queue<int> locksInQueue = new Queue<int>(locks);

            int counter = 0;
            int costBullets = 0;

            for (int i = 1; i <= bulletsInStack.Count; i++)
            {
                if (locksInQueue.Any())
                {
                    counter++;
                    if (bulletsInStack.Peek() <= locksInQueue.Peek())
                    {
                        Console.WriteLine("Bang!");
                        bulletsInStack.Pop();
                        locksInQueue.Dequeue();
                        costBullets += pricePerBullet;
                        i--;
                    }
                    else if (bulletsInStack.Peek() > locksInQueue.Peek())
                    {
                        Console.WriteLine("Ping!");
                        bulletsInStack.Pop();
                        costBullets += pricePerBullet;
                        i--;
                    }

                    if (counter == sizeGunBarrel && bulletsInStack.Any())
                    {
                        Console.WriteLine("Reloading!");
                        counter = 0;
                    }
                    
                }
                else
                {
                    continue;
                }
            }
            
            if (locksInQueue.Any())
            {
                Console.WriteLine($"Couldn't get through. Locks left: {locksInQueue.Count}");
            }
            else
            {
                Console.WriteLine($"{bulletsInStack.Count} bullets left. Earned ${valueIntelligence - costBullets}");
            }
        }
    }
}
