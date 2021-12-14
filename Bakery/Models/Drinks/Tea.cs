using System.Diagnostics;

namespace Bakery.Models.Drinks
{
    public class Tea : Drink
    {
        private const decimal TeaPrice = (decimal)2.50;
        public Tea(string name, int portion, string brand) : base(name, portion, TeaPrice, brand)
        {
        }
    }
}