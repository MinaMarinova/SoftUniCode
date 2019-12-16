using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleTextEditor
{
    class Program
    {
        static void Main(string[] args)
        {
            int N = int.Parse(Console.ReadLine());
            StringBuilder text = new StringBuilder();

            Stack<string> commandToback = new Stack<string>();


            for (int i = 0; i < N; i++)
            {
                string[] command = Console.ReadLine().Split();

                if (command[0] == "1")
                {
                    commandToback.Push(text.ToString());
                    text.Append(command[1]);

                }
                else if (command[0] == "2")
                {
                    commandToback.Push(text.ToString());
                    text.Remove(text.Length - int.Parse(command[1]), int.Parse(command[1]));

                }
                else if (command[0] == "3")
                {
                    int index = int.Parse(command[1]);
                    Console.WriteLine(text[index - 1]);
                }
                else if (command[0] == "4")
                {
                    text.Clear();
                    text.Append(commandToback.Pop());
                }

            }
        }
    }
}
