using System.Xml.Linq;

namespace E_Commerce.Core.Entities.Basket_Module
{
    public class BasketItem
    {

       

        public int Id { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }

        public string Brand { get; set; }

        public string Type { get; set; }

        public  decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}