using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bakery.Models.BakedFoods;
using Bakery.Models.BakedFoods.Contracts;
using Bakery.Models.Drinks;
using Bakery.Models.Drinks.Contracts;
using Bakery.Models.Tables;
using Bakery.Models.Tables.Contracts;
using Bakery.Utilities.Messages;

namespace Bakery.Core.Contracts
{
    public class Controller : IController
    {
        private List<IBakedFood> BakedFoods;
        private List<IDrink> Drinks;
        private List<ITable> Tables;
        private decimal totalIncome;
        
        public Controller()
        {
            this.BakedFoods = new List<IBakedFood>();
            this.Drinks = new List<IDrink>();
            this.Tables = new List<ITable>();
            
        }
        public string AddFood(string type, string name, decimal price)
        {
            if (type == "Bread")
            {
               BakedFoods.Add(new Bread(name,price));
            }else if (type == "Cake")
            {
                BakedFoods.Add(new Cake(name,price));
            }
            return string.Format(OutputMessages.FoodAdded, name, type);
        }

        public string AddDrink(string type, string name, int portion, string brand)
        {
            if (type == "Tea")
            {
                Drinks.Add(new Tea(name,portion,brand));
            }else if (type == "Water")
            {
                Drinks.Add(new Water(name,portion,brand));
            }

            return string.Format(OutputMessages.DrinkAdded, name, brand);
        }

        public string AddTable(string type, int tableNumber, int capacity)
        {
            if (type == "OutsideTable")
            {
                Tables.Add(new OutsideTable(tableNumber,capacity));
            }else if (type == "InsideTable")
            {
                Tables.Add(new InsideTable(tableNumber,capacity));
            }

            return string.Format(OutputMessages.TableAdded, tableNumber);
        }

        public string ReserveTable(int numberOfPeople)
        {
            var table = Tables.FirstOrDefault(table =>
                table.IsReserved == false && table.Capacity >= table.NumberOfPeople);
            if (table == null)
            {
                return string.Format(OutputMessages.ReservationNotPossible, numberOfPeople);
            }
            else
            {
                table.Reserve(numberOfPeople);
                return string.Format(OutputMessages.TableReserved, table.TableNumber, numberOfPeople);
            }
        }

        public string OrderFood(int tableNumber, string foodName)
        {
            var table = Tables.FirstOrDefault(table => table.TableNumber == tableNumber);
            var order = BakedFoods.FirstOrDefault(food => food.Name == foodName);
            if (table == null)
            {
                return string.Format(OutputMessages.WrongTableNumber, tableNumber);
            }

            if (order == null)
            {
                return string.Format(OutputMessages.NonExistentFood, foodName);
            }
            table.OrderFood(order);
            return string.Format(OutputMessages.FoodOrderSuccessful, tableNumber, foodName);
        }

        public string OrderDrink(int tableNumber, string drinkName, string drinkBrand)
        {
            var table = Tables.FirstOrDefault(table => table.TableNumber == tableNumber);
            var order = Drinks.FirstOrDefault(drink => drink.Name == drinkName && drink.Brand == drinkBrand);
            if (table == null)
            {
                return string.Format(OutputMessages.WrongTableNumber, tableNumber);
            }

            if (order == null)
            {
                return string.Format(OutputMessages.NonExistentDrink, drinkName, drinkBrand);
            }
            table.OrderDrink(order);
            return string.Format(OutputMessages.FoodOrderSuccessful, tableNumber, drinkName + " " + drinkBrand);
        }

        public string LeaveTable(int tableNumber)
        {
            var table = Tables.FirstOrDefault(table => table.TableNumber == tableNumber);
            if (table == null)
            {
                return string.Format(OutputMessages.WrongTableNumber, tableNumber);
            }

            var total = table.Price + table.GetBill();
            totalIncome += total;
            table.Clear();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"Table number: {tableNumber}" + Environment.NewLine);
            stringBuilder.Append($"Bill: {total:F2}" + Environment.NewLine);
            return stringBuilder.ToString().TrimEnd();
        }

        public string GetFreeTablesInfo()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var table in Tables.Where(table => !table.IsReserved))
            {
                stringBuilder.Append(table.GetFreeTableInfo()+Environment.NewLine);
            }

            return stringBuilder.ToString().TrimEnd();
        }

        public string GetTotalIncome()
        {
            return string.Format(OutputMessages.TotalIncome,totalIncome);
        }
    }
}