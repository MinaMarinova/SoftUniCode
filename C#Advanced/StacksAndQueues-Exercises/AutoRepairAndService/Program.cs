using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoRepairAndService
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] inputCars = Console.ReadLine().Split();

            Queue<string> waitingVehicles = new Queue<string>(inputCars);

            Stack<string> servedVehigles = new Stack<string>();

            while (true)
            {
                string command = Console.ReadLine();

                if (command == "End")
                {
                    if (waitingVehicles.Count > 0)
                    {
                        Console.Write("Vehicles for service: ");
                        Console.WriteLine(string.Join(", ", waitingVehicles));
                    }
                    Console.Write("Served vehicles: ");
                    Console.WriteLine(string.Join(", ", servedVehigles));
                    break;
                }
                else if (command == "Service" && waitingVehicles.Count > 0)
                {
                    servedVehigles.Push(waitingVehicles.Dequeue());
                    Console.WriteLine($"Vehicle {servedVehigles.Peek()} got served.");
                }
                else if (command.Contains("CarInfo"))
                {
                    string[] carInfo = command.Split('-');
                    if (waitingVehicles.Contains(carInfo[1]))
                    {
                        Console.WriteLine("Still waiting for service.");
                    }
                    else
                    {
                        Console.WriteLine("Served.");
                    }
                }
                else if (command == "History")
                {
                    Console.WriteLine(string.Join(", ", servedVehigles));
                }
            }
        }
    }
}
