namespace SoftUniRestaurant.Core
{
    using Models.Foods.Contracts;
    using Models.Drinks.Contracts;
    using System;
    using System.Collections.Generic;
    using Models.Tables.Contracts;
    using Models.Factories;
    using System.Linq;
    using System.Text;

    public class RestaurantController
    {
        private List<IFood> menu;
        private List<IDrink> drinks;
        private List<ITable> tables;

        private FoodFactory foodFactory;
        private DrinkFactory drinkFactory;
        private TableFactory tableFactory;

        private decimal totalIncome;

        public RestaurantController()
        {
            this.menu = new List<IFood>();
            this.drinks = new List<IDrink>();
            this.tables = new List<ITable>();
            this.foodFactory = new FoodFactory();
            this.drinkFactory = new DrinkFactory();
            this.tableFactory = new TableFactory();
        }


        public string AddFood(string type, string name, decimal price)
        {
            IFood food = this.foodFactory.Create(type, name, price);
            this.menu.Add(food);

            return $"Added {name} ({type}) with price {price:f2} to the pool";
        }

        public string AddDrink(string type, string name, int servingSize, string brand)
        {
            IDrink drink = this.drinkFactory.Create(type, name, servingSize, brand);
            this.drinks.Add(drink);

            return $"Added {name} ({brand}) to the drink pool";
        }

        public string AddTable(string type, int tableNumber, int capacity)
        {
            ITable table = this.tableFactory.Create(type, tableNumber, capacity);
            this.tables.Add(table);

            return $"Added table number {tableNumber} in the restaurant";
        }

        public string ReserveTable(int numberOfPeople)
        {
            ITable table = this.tables.FirstOrDefault(t => !t.IsReserved && t.Capacity >= numberOfPeople);

            if (table == null)
            {
                return $"No available table for {numberOfPeople} people";
            }

            else
            {
                table.Reserve(numberOfPeople);

                return $"Table {table.TableNumber} has been reserved for {numberOfPeople} people";
            }
        }

        public string OrderFood(int tableNumber, string foodName)
        {
            ITable table = this.tables.FirstOrDefault(t => t.TableNumber == tableNumber);

            if (table == null)
            {
                return $"Could not find table with {tableNumber}";
            }

            IFood food = this.menu.FirstOrDefault(f => f.Name == foodName);

            if (food == null)
            {
                return $"No {foodName} in the menu";
            }

            table.OrderFood(food);

            return $"Table {tableNumber} ordered {foodName}";
        }

        public string OrderDrink(int tableNumber, string drinkName, string drinkBrand)
        {
            ITable table = this.tables.FirstOrDefault(t => t.TableNumber == tableNumber);

            if (table == null)
            {
                return $"Could not find table with {tableNumber}";
            }

            IDrink drink = this.drinks.FirstOrDefault(d => d.Name == drinkName && d.Brand == drinkBrand);

            if (drink == null)
            {
                return $"There is no {drinkName} {drinkBrand} available";
            }

            table.OrderDrink(drink);
            return $"Table {tableNumber} ordered {drinkName} {drinkBrand}";
        }

        public string LeaveTable(int tableNumber)
        {
            ITable table = this.tables.FirstOrDefault(t => t.TableNumber == tableNumber);

            if (table != null)
            {
                decimal bill = table.GetBill();
                table.Clear();

                totalIncome += bill;
                return $"Table: {tableNumber}" + Environment.NewLine + $"Bill: {bill:f2}";
            }

            return null;
        }

        public string GetFreeTablesInfo()
        {
            var freeTables = this.tables.Where(t => !t.IsReserved);
            
            StringBuilder sb = new StringBuilder();

            foreach (var table in freeTables)
            {
                sb.AppendLine(table.GetFreeTableInfo());
            }

            return sb.ToString().TrimEnd();
        }

        public string GetOccupiedTablesInfo()
        {
            var occupiedTables = this.tables.Where(t => t.IsReserved);

            StringBuilder sb = new StringBuilder();

            foreach (var table in occupiedTables)
            {
                sb.AppendLine(table.GetOccupiedTableInfo());
            }

            return sb.ToString().TrimEnd();
        }

        public string GetSummary()
        {
            return $"Total income: {totalIncome:f2}lv";
        }
    }
}
