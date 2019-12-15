using System;
using System.Linq;

namespace MortalEngines.Core
{
    public class Engine
    {
        private MachinesManager machinesManager;

        public Engine()
        {
            this.machinesManager = new MachinesManager();
        }

        public void Run()
        {
            while (true)
            {
                string command = Console.ReadLine();

                if (command == "Quit")
                {
                    break;
                }

                string[] commandArgs = command.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                string commandName = commandArgs[0];
                string[] data = commandArgs.Skip(1).ToArray();

                try
                {
                    if (commandName == "HirePilot")
                    {
                        string name = data[0];
                        Console.WriteLine(this.machinesManager.HirePilot(name));
                    }

                    else if (commandName == "PilotReport")
                    {
                        string name = data[0];
                        Console.WriteLine(this.machinesManager.PilotReport(name));
                    }

                    else if (commandName == "ManufactureTank")
                    {
                        string name = data[0];
                        double attack = double.Parse(data[1]);
                        double defense = double.Parse(data[2]);

                        Console.WriteLine(this.machinesManager.ManufactureTank(name, attack, defense));
                    }

                    else if (commandName == "ManufactureFighter")
                    {
                        string name = data[0];
                        double attack = double.Parse(data[1]);
                        double defense = double.Parse(data[2]);

                        Console.WriteLine(this.machinesManager.ManufactureFighter(name, attack, defense));
                    }

                    else if (commandName == "MachineReport")
                    {
                        string name = data[0];
                        Console.WriteLine(this.machinesManager.MachineReport(name));
                    }

                    else if (commandName == "AggressiveMode")
                    {
                        string name = data[0];
                        Console.WriteLine(this.machinesManager.ToggleFighterAggressiveMode(name));
                    }

                    else if (commandName == "DefenseMode")
                    {
                        string name = data[0];
                        Console.WriteLine(this.machinesManager.ToggleTankDefenseMode(name));
                    }

                    else if (commandName == "Engage")
                    {
                        string pilotName = data[0];
                        string machineName = data[1];

                        Console.WriteLine(this.machinesManager.EngageMachine(pilotName, machineName));
                    }

                    else if (commandName == "Attack")
                    {
                        string attachingMachineName = data[0];
                        string defendingMachineName = data[1];

                        Console.WriteLine(this.machinesManager.AttackMachines(attachingMachineName, defendingMachineName));
                    }
                }

                catch(Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine("Error:" + ex.InnerException.Message);
                    }

                    else
                    {
                        Console.WriteLine("Error:" + ex.Message);
                    }
                }
            }
        }
    }
}
