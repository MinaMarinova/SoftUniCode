namespace SoftUniRestaurant.Models.Factories
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Tables.Contracts;

    public class TableFactory
    {
        public ITable Create(string type, int tableNumber, int capacity)
        {
            var typeTable = Assembly
                .GetCallingAssembly()
                .GetTypes()
                .FirstOrDefault(t => !t.IsAbstract && t.Name == type + "Table");

            if (typeTable != null)
            {
                var instanceTable = Activator.CreateInstance(typeTable, tableNumber, capacity);
                return (ITable)instanceTable;
            }

            return null;
        }
    }
}
