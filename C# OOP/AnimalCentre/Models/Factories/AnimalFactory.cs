namespace AnimalCentre.Models.Factories
{
    using AnimalCentre.Models.Contracts;
    using System;
    using System.Linq;
    using System.Reflection;

    public class AnimalFactory
    {
        public IAnimal Create(string type, string name, int energy, int happiness, int procedureTime)
        {
            var animalType = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .FirstOrDefault(x => !x.IsAbstract && x.Name == type);
            
            if (animalType != null)
            {
                var instanceAnimal = Activator.CreateInstance(animalType, name, energy, happiness, procedureTime);
                return (IAnimal)instanceAnimal;
            }

            return null;
        }
    }
}
