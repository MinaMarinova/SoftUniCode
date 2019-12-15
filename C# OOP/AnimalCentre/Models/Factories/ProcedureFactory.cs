namespace AnimalCentre.Models.Factories
{
    using Models.Contracts;
    using System;
    using System.Linq;
    using System.Reflection;

    public class ProcedureFactory
    {
        public IProcedure Create(string type)
        {
            var typeProcedure = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .FirstOrDefault(x => !x.IsAbstract && x.Name == type);

            if (typeProcedure != null)
            {
                var instanceProcedure = Activator.CreateInstance(typeProcedure);
                return (IProcedure)instanceProcedure;
            }

            return null;
        }
    }
}
