namespace MortalEngines.Entities.Factories
{
    using MortalEngines.Entities.Contracts;
    using System;
    using System.Linq;
    using System.Reflection;

    public class MachineFactory
    {
        public IMachine CreateMachine(string type, string name, double attackPoints, double defensePoints)
        {
            var typeMachine = Assembly
                .GetCallingAssembly()
                .GetTypes()
                .FirstOrDefault(m => m.Name == type);

            var instanceMachine = Activator.CreateInstance(typeMachine, name, attackPoints, defensePoints);

            return (IMachine)instanceMachine;
        }
    }
}
