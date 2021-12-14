namespace Bakery.Models.Drinks
{
    public class Water : Drink
    {
        private const decimal WaterPrice = (decimal)1.50;
        public Water(string name, int portion, string brand) : base(name, portion, WaterPrice, brand)
        {
        }
    }
}