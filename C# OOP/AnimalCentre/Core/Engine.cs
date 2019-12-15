using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimalCentre.Core
{
    public class Engine
    {
        private AnimalCentre animalCentre;

        public Engine(AnimalCentre animalCentre)
        {
            this.animalCentre = animalCentre;
        }

        public void Run()
        {
            while (true)
            {
                string input = Console.ReadLine();

                if (input == "End")
                {
                    Console.WriteLine(this.animalCentre.GetSummary());
                    break;
                }

                string[] commandArgs = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                string commandName = commandArgs[0];
                string[] data = commandArgs.Skip(1).ToArray();
                

                try
                {
                    switch (commandName)
                    {
                    

                        case "RegisterAnimal":
                            string type = data[0];
                            string name = data[1];
                            int energy = int.Parse(data[2]);
                            int happiness = int.Parse(data[3]);
                            int procedureTime = int.Parse(data[4]);

                            Console.WriteLine(this.animalCentre.RegisterAnimal(type, name, energy, happiness, procedureTime));
                            break;

                        case "Chip":
                            name = data[0];
                            procedureTime = int.Parse(data[1]);

                            Console.WriteLine(this.animalCentre.Chip(name, procedureTime));
                            break;

                        case "Vaccinate":
                            name = data[0];
                            procedureTime = int.Parse(data[1]);

                            Console.WriteLine(this.animalCentre.Vaccinate(name, procedureTime));
                            break;

                        case "Fitness":
                            name = data[0];
                            procedureTime = int.Parse(data[1]);

                            Console.WriteLine(this.animalCentre.Fitness(name, procedureTime));
                            break;

                        case "Play":
                            name = data[0];
                            procedureTime = int.Parse(data[1]);

                            Console.WriteLine(this.animalCentre.Play(name, procedureTime));
                            break;

                        case "DentalCare":
                            name = data[0];
                            procedureTime = int.Parse(data[1]);

                            Console.WriteLine(this.animalCentre.DentalCare(name, procedureTime));
                            break;

                        case "NailTrim":
                            name = data[0];
                            procedureTime = int.Parse(data[1]);

                            Console.WriteLine(this.animalCentre.NailTrim(name, procedureTime));
                            break;
                        case "Adopt":
                            string animalName = data[0];
                            string owner = data[1];

                            Console.WriteLine(this.animalCentre.Adopt(animalName, owner));
                            break;

                        case "History":
                            string procedureType = data[0];
                            Console.WriteLine(this.animalCentre.History(procedureType));
                            break;

                        default:
                            break;
                    }
                }

                catch(Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"ArgumentException: {ex.InnerException.Message}");
                    }
                    else
                    {
                        if (ex.GetType().Name == "InvalidOperationException")
                        {
                            Console.WriteLine($"InvalidOperationException: {ex.Message}");
                        }
                        else
                        {
                            Console.WriteLine($"ArgumentException: {ex.Message}");
                        }
                    }
                }
            }
        }
    }
}
