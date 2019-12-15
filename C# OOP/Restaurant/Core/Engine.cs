namespace SoftUniRestaurant.Core
{
    using System;
    using System.Linq;

    public class Engine
    {
        private RestaurantController restaurantController;

        public Engine()
        {
            this.restaurantController = new RestaurantController();
        }

        public void Run()
        {
            while (true)
            {
                string input = Console.ReadLine();

                if (input == "END")
                {
                    Console.WriteLine(this.restaurantController.GetSummary());
                    break;
                }

                string[] commandArgs = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                string commandName = commandArgs[0];

                string[] data = commandArgs.Skip(1).ToArray();
                
                try
                {
                    switch (commandName)
                    {
                        case "AddFood":
                            string type = data[0];
                            string name = data[1];
                            decimal price = decimal.Parse(data[2]);

                            Console.WriteLine(this.restaurantController.AddFood(type, name, price));
                            break;

                        case "AddDrink":
                            type = data[0];
                            name = data[1];
                            int servingSize = int.Parse(data[2]);
                            string brand = data[3];

                            Console.WriteLine(this.restaurantController.AddDrink(type, name, servingSize, brand));
                            break;

                        case "AddTable":
                            type = data[0];
                            int tableNumber = int.Parse(data[1]);
                            int capacity = int.Parse(data[2]);

                            Console.WriteLine(this.restaurantController.AddTable(type, tableNumber, capacity));
                            break;

                        case "ReserveTable":
                            int numberOfPeople = int.Parse(data[0]);
                            Console.WriteLine(this.restaurantController.ReserveTable(numberOfPeople));
                            break;

                        case "OrderFood":
                            tableNumber = int.Parse(data[0]);
                            string foodName = data[1];

                            Console.WriteLine(this.restaurantController.OrderFood(tableNumber, foodName));
                            break;

                        case "OrderDrink":
                            tableNumber = int.Parse(data[0]);
                            string drinkName = data[1];
                            string drinkBrand = data[2];

                            Console.WriteLine(this.restaurantController.OrderDrink(tableNumber, drinkName, drinkBrand));
                            break;

                        case "LeaveTable":
                            tableNumber = int.Parse(data[0]);
                            Console.WriteLine(this.restaurantController.LeaveTable(tableNumber));
                            break;

                        case "GetFreeTablesInfo":

                            Console.WriteLine(this.restaurantController.GetFreeTablesInfo());
                            break;

                        case "GetOccupiedTablesInfo":

                            Console.WriteLine(this.restaurantController.GetOccupiedTablesInfo());
                            break;

                        default:
                            break;
                    }
                }

                catch(Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine(ex.InnerException.Message);
                    }

                    else
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

    }
}
