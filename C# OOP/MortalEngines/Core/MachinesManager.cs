namespace MortalEngines.Core
{
    using Contracts;
    using MortalEngines.Common;
    using MortalEngines.Entities;
    using MortalEngines.Entities.Contracts;
    using MortalEngines.Entities.Factories;
    using System.Collections.Generic;
    using System.Linq;

    public class MachinesManager : IMachinesManager
    {
        private PilotFactory pilotFactory;
        private MachineFactory machineFactory;

        private List<IMachine> machines;
        private List<IPilot> pilots;
        
        public MachinesManager()
        {
            this.machines = new List<IMachine>();
            this.pilots = new List<IPilot>();

            this.pilotFactory = new PilotFactory();
            this.machineFactory = new MachineFactory();
            
        }

        public string HirePilot(string name)
        {
            if (this.pilots.Any(p => p.Name == name))
            {
                return $"Pilot {name} is hired already";
            }

            else
            {
                IPilot pilot = this.pilotFactory.CreatePilot("Pilot", name);
                this.pilots.Add(pilot);
                return $"Pilot {name} hired";
            }
        }

        public string ManufactureTank(string name, double attackPoints, double defensePoints)
        {
            string type = "Tank";

            if (this.machines.Any(m => m.Name == name))
            {
                return $"Machine {name} is manufactured already";
            }

            IMachine machine = this.machineFactory.CreateMachine(type, name, attackPoints, defensePoints);
            this.machines.Add(machine);
            return $"Tank {name} manufactured - attack: {machine.AttackPoints:F2}; defense: {machine.DefensePoints:F2}";
        }

        public string ManufactureFighter(string name, double attackPoints, double defensePoints)
        {
            string type = "Fighter";

            if (this.machines.Any(m => m.Name == name))
            {
                return $"Machine {name} is manufactured already";
            }

            IMachine machine = this.machineFactory.CreateMachine(type, name, attackPoints, defensePoints);
            this.machines.Add(machine);
            return $"Fighter {name} manufactured - attack: {machine.AttackPoints:F2}; defense: {machine.DefensePoints:F2}; aggressive: ON";
        }

        public string EngageMachine(string selectedPilotName, string selectedMachineName)
        {
            IPilot pilot = this.pilots.FirstOrDefault(p => p.Name == selectedPilotName);
            IMachine machine = this.machines.FirstOrDefault(m => m.Name == selectedMachineName);

            if (pilot == null)
            {
                return $"Pilot {selectedPilotName} could not be found";
            }

            if (machine == null)
            {
                return $"Machine {selectedMachineName} could not be found";
            }

            // ToDo: CouldCheckForPilotNotName
            if (machine.Pilot != null)
            {
                return $"Machine {selectedMachineName} is already occupied";
            }


            pilot.AddMachine(machine);
            machine.Pilot = pilot;
            return $"Pilot {selectedPilotName} engaged machine {selectedMachineName}";

        }

        public string AttackMachines(string attackingMachineName, string defendingMachineName)
        {
            IMachine attackingMachine = this.machines.FirstOrDefault(m => m.Name == attackingMachineName);
            IMachine defendingMachine = this.machines.FirstOrDefault(m => m.Name == defendingMachineName);

            if (attackingMachine == null)
            {
                return $"Machine {attackingMachineName} could not be found";
            }

            if (defendingMachine == null)
            {
                return $"Machine {defendingMachineName} could not be found";
            }

            if (attackingMachine.HealthPoints == 0)
            {
                return $"Dead machine {attackingMachineName} cannot attack or be attacked";
            }

            if (defendingMachine.HealthPoints == 0)
            {
                return $"Dead machine {defendingMachineName} cannot attack or be attacked";
            }

            attackingMachine.Attack(defendingMachine);

            return $"Machine {defendingMachineName} was attacked by machine {attackingMachineName} - current health: {defendingMachine.HealthPoints:F2}";

        }

        public string PilotReport(string pilotReporting)
        {
            IPilot pilot = this.pilots.FirstOrDefault(p => p.Name == pilotReporting);

            if (pilot != null)
            {
                return pilot.Report();
            }

            return null;
        }

        public string MachineReport(string machineName)
        {
            IMachine machine = this.machines.FirstOrDefault(m => m.Name == machineName);

            if (machine != null)
            {
                return machine.ToString();
            }

            return null;
        }

        public string ToggleFighterAggressiveMode(string fighterName)
        {
            Fighter fighter = (Fighter)this.machines.FirstOrDefault(m => m.Name == fighterName);

            if (fighter == null)
            {
                return $"Machine {fighterName} could not be found";
            }

            fighter.ToggleAggressiveMode();
            return $"Fighter {fighterName} toggled aggressive mode";
        }

        public string ToggleTankDefenseMode(string tankName)
        {
            Tank machine = (Tank)this.machines.FirstOrDefault(m => m.Name == tankName);
            
            if (machine == null)
            {
                return $"Machine {tankName} could not be found";
            }

            machine.ToggleDefenseMode();
            return $"Tank {tankName} toggled defense mode";
        }
    }
}