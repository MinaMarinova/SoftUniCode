namespace SoftUniRestaurant.Models.Tables
{
    using Drinks.Contracts;
    using Foods.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Tables.Contracts;

    public abstract class Table : ITable
    {
        private ICollection<IFood> foodOrders;
        private ICollection<IDrink> drinkOrders;
        private int capacity;
        private int numberOfPeople;

        protected Table(int tableNumber, int capacity, decimal pricePerPerson)
        {
            this.TableNumber = tableNumber;
            this.Capacity = capacity;
            this.PricePerPerson = pricePerPerson;
            this.foodOrders = new List<IFood>();
            this.drinkOrders = new List<IDrink>();
            this.IsReserved = false;
        }

        public int TableNumber { get; protected set; }
        
        public int Capacity
        {
            get => this.capacity;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Capacity has to be greater than 0");
                }

                this.capacity = value;
            }
        }

        public int NumberOfPeople
        {
            get => this.numberOfPeople;
            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Cannot place zero or less people!");
                }

                this.numberOfPeople = value;
            }
        }

        public decimal PricePerPerson { get; protected set; }

        public bool IsReserved { get; set; }

        public decimal Price => this.foodOrders.Select(f => f.Price).Sum() + this.drinkOrders.Select(d => d.Price).Sum() + (this.PricePerPerson * this.NumberOfPeople);


        public void Clear()
        {
            this.foodOrders.Clear();
            this.drinkOrders.Clear();
            this.IsReserved = false;
            
        }

        public decimal GetBill()
        {
            return Price;
        }

        public string GetFreeTableInfo()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Table: {this.TableNumber}");
            sb.AppendLine($"Type: {this.GetType().Name}");
            sb.AppendLine($"Capacity: {this.Capacity}");
            sb.AppendLine($"Price per Person: {this.PricePerPerson}");

            return sb.ToString().TrimEnd();
        }

        public string GetOccupiedTableInfo()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Table: {this.TableNumber}");
            sb.AppendLine($"Type: {this.GetType().Name}");
            sb.AppendLine($"Number of people: {this.NumberOfPeople}");
            if (this.foodOrders.Count == 0)
            {
                sb.AppendLine("Food orders: None");
            }
            else
            {
                sb.AppendLine($"Food orders: {this.foodOrders.Count}");
                foreach (var food in foodOrders)
                {
                    sb.AppendLine(food.ToString());
                }
            }
            if (this.drinkOrders.Count == 0)
            {
                sb.AppendLine("Drink orders: None");
            }
            else
            {
                sb.AppendLine($"Drink orders: {this.drinkOrders.Count}");
                foreach (var drink in drinkOrders)
                {
                    sb.AppendLine(drink.ToString());
                }
            }

            return sb.ToString().TrimEnd();
        }

        public void OrderDrink(IDrink drink)
        {
            this.drinkOrders.Add(drink);
        }

        public void OrderFood(IFood food)
        {
            this.foodOrders.Add(food);
        }

        public void Reserve(int numberOfPeople)
        {
            this.NumberOfPeople = numberOfPeople;
            this.IsReserved = true;
        }
    }
}
