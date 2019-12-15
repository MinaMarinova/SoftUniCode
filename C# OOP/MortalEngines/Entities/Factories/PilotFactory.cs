namespace MortalEngines.Entities.Factories
{
    using Contracts;
    using System;
    using System.Linq;
    using System.Reflection;

    public class PilotFactory
    {
        public IPilot CreatePilot(string type, string name)
        {
            var typePilot = Assembly
                .GetCallingAssembly()
                .GetTypes()
                .FirstOrDefault(p => p.Name == type);

            var instancePilot = Activator.CreateInstance(typePilot, name);

            return (IPilot)instancePilot;
        }
    }
}
