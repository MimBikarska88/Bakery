namespace Bakery.Models.Tables
{
    public class OutsideTable : Table
    {
        private const decimal InitialPricePerPerson = (decimal)3.50;

        public OutsideTable(int tableNumber, int capacity) : base(tableNumber, capacity,InitialPricePerPerson)
        {
        }
    }
}