using System;
using System.Collections.Generic;

namespace Wardrobe
{
    class Program
    {
        static void Main(string[] args)
        {
            int lines = int.Parse(Console.ReadLine());

            Dictionary<string, Dictionary<string, int>> wardrobe = new Dictionary<string, Dictionary<string, int>>();

            for (int i = 0; i < lines; i++)
            {
                string[] input = Console.ReadLine()
                    .Split(" -> ");

                var colour = input[0];
                string[] clothes = input[1].Split(',');
                if (!wardrobe.ContainsKey(colour))
                    {
                        wardrobe[colour] = new Dictionary<string, int>();
                    }

                foreach (var item in clothes)
                {
                    if (!wardrobe[colour].ContainsKey(item))
                    {
                        wardrobe[colour][item] = 0;
                    }
                    wardrobe[colour][item]++;
                }
            }
            string[] command = Console.ReadLine()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            string colourToFind = command[0];
            string itemToFind = command[1];

            foreach(var kvp in wardrobe)
            {
                    string colour = kvp.Key;
                    Dictionary<string, int> items = kvp.Value;

                    Console.WriteLine($"{colour} clothes:");

                if (colour == colourToFind)
                {
                    foreach (var item in items)
                    {
                        var cloth = item.Key;
                        var quantity = item.Value;

                        if (cloth == itemToFind)
                        {
                            Console.WriteLine($"* {cloth} - {quantity} (found!)");
                        }
                        else
                        {
                            Console.WriteLine($"* {cloth} - {quantity}");
                        }
                    }
                }

                else
                {
                    string colourNoMatch = kvp.Key;
                    Dictionary<string, int> itemsNoMatch = kvp.Value;
                    

                    foreach (var item in itemsNoMatch)
                    {
                        var cloth = item.Key;
                        var quantity = item.Value;

                        Console.WriteLine($"* {cloth} - {quantity}");

                    }
                }
            }
        }
    }
}
