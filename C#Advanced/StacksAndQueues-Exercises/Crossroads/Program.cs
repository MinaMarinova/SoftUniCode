using System;
using System.Collections.Generic;
using System.Linq;

namespace Crossroads
{
    class Program
    {
        static void Main(string[] args)
        {
            int durationGreenLight = int.Parse(Console.ReadLine());
            int durationFreeWindow = int.Parse(Console.ReadLine());

            int greenLightRefreshed = durationGreenLight;

            Queue<string> cars = new Queue<string>();
            int count = 0;

           

            while (true)
            {
                string command = Console.ReadLine();

                if (command == "END")
                {
                    Console.WriteLine("Everyone is safe.");
                    Console.WriteLine($"{count} total cars passed the crossroads.");
                    break;
                }

                else if (command == "green")
                {
                    durationGreenLight = greenLightRefreshed;
                    while (true)
                    {
                        if (cars.Any())
                        {
                            string currentCar = cars.Peek();

                            if (durationGreenLight > currentCar.Length)
                            {
                                cars.Dequeue();
                                count++;
                                durationGreenLight -= currentCar.Length;
                            }

                            else if (durationGreenLight == currentCar.Length)
                            {
                                cars.Dequeue();
                                count++;
                                break;
                            }

                            else
                            {
                                currentCar = currentCar.Substring(durationGreenLight, currentCar.Length - durationGreenLight);
                                if (durationFreeWindow < currentCar.Length)
                                {
                                    Console.WriteLine("A crash happened!");
                                    Console.WriteLine($"{cars.Peek()} was hit at {currentCar[durationFreeWindow]}.");
                                    return;
                                }
                                else
                                {
                                    cars.Dequeue();
                                    count++;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            break;

                        }

                    }
                }
                else
                {
                    cars.Enqueue(command);
                }
            }
        }
    }
}
