using System;
using System.Collections.Generic;

namespace CountSymbols
{
    class Program
    {
        static void Main(string[] args)
        {
            string text = Console.ReadLine();

            SortedDictionary<char, int> symbols = new SortedDictionary<char, int>();

            foreach (var symbol in text)
            {
                if (!symbols.ContainsKey(symbol))
                {
                    symbols[symbol] = 0;
                }
                symbols[symbol]++;
            }

            foreach (var item in symbols)
            {
                var symbol = item.Key;
                var appearance = item.Value;

                Console.WriteLine($"{symbol}: {appearance} time/s");
            }
        }
    }
}
