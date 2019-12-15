namespace SoftUniRestaurant.Models.Factories
{
    using Foods.Contracts;
    using System;
    using System.Linq;
    using System.Reflection;

    public class FoodFactory
    {
        public IFood Create(string type, string name, decimal price)
        {
            var typeFood = Assembly
                .GetCallingAssembly()
                .GetTypes()
                .FirstOrDefault(f => !f.IsAbstract && f.Name == type);

            if (typeFood != null)
            {
                var instanceFood = Activator.CreateInstance(typeFood, name, price);
                return (IFood)instanceFood;
            }

            return null;
        }
    }
}
