using E_Commerce.Core.Entities.Order_Module;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Apis.DTOs
{
    public class OrderDto
    {
        [Required]
      public  string basketId {  get; set; }
      public    int deliveryMethodId { get; set; }
       public   AddressDto ShippingAddress { get; set; }
    }
}
