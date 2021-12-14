using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bakery.Models.BakedFoods.Contracts;
using Bakery.Models.Drinks;
using Bakery.Models.Drinks.Contracts;
using Bakery.Models.Tables.Contracts;
using Bakery.Utilities.Messages;

namespace Bakery.Models.Tables
{
    public abstract class Table : ITable
    {
        private readonly List<IDrink> DrinkOrders;
        private readonly List<IBakedFood> FoodOrders;
        protected int capacity;

        protected Table(int tableNumber, int capacity, decimal price)
        {
            DrinkOrders = new List<IDrink>();
            FoodOrders = new List<IBakedFood>();
            this.Capacity = capacity;
            this.TableNumber = tableNumber;
            this.PricePerPerson = price;
        }
        public int TableNumber { get; }

        public int Capacity
        {
            get => capacity;
            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentException(ExceptionMessages.InvalidTableCapacity);
                }

                capacity = value;
            }
        }
        public int NumberOfPeople { get; private set; }
        public decimal PricePerPerson { get; }
        public bool IsReserved => NumberOfPeople > 0;

        public decimal Price
        {
            get => NumberOfPeople * PricePerPerson;
        }
        public void Reserve(int numberOfPeople)
        {
            if (numberOfPeople<=0)
            {
                throw new ArgumentException(ExceptionMessages.InvalidNumberOfPeople);
            }

            NumberOfPeople = numberOfPeople;
        }

        public void OrderFood(IBakedFood food)
        {
           FoodOrders.Add(food);
        }

        public void OrderDrink(IDrink drink)
        {
            DrinkOrders.Add(drink);
        }

        public decimal GetBill()
        {
            decimal sum = 0;
            if (FoodOrders.Count > 0)
            {
                sum += this.FoodOrders.Select(food => food.Price).Sum();
            }

            if (DrinkOrders.Count > 0)
            {
                sum  += this.DrinkOrders.Select(drink => drink.Price).Sum();
            }

            return sum;
        }

        public void Clear()
        {
            this.FoodOrders.Clear();
            this.DrinkOrders.Clear();
            this.NumberOfPeople = 0;
        }

        public string GetFreeTableInfo()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"Table: {this.TableNumber}"+Environment.NewLine);
            stringBuilder.Append($"Type: {this.GetType().Name}" + Environment.NewLine);
            stringBuilder.Append($"Capacity: {this.Capacity}" + Environment.NewLine);
            stringBuilder.Append($"Price per Person: {this.PricePerPerson:F2}" + Environment.NewLine);
            return stringBuilder.ToString().TrimEnd();
        }
    }
}