namespace SoftUniRestaurant.Models.Factories
{
    using Drinks.Contracts;
    using System;
    using System.Linq;
    using System.Reflection;

    public class DrinkFactory
    {
        public IDrink Create(string type, string name, int servingSize, string brand)
        {
            var typeDrink = Assembly
                .GetCallingAssembly()
                .GetTypes()
                .FirstOrDefault(d => !d.IsAbstract && d.Name == type);

            if (typeDrink != null)
            {
                var instanceDrink = Activator.CreateInstance(typeDrink, name, servingSize, brand);

                return (IDrink)instanceDrink;
            }
            return null;
        }
    }
}
