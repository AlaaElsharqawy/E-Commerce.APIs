using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Apis.DTOs
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        [Required]

        public string Brand { get; set; }
        [Required]

        public string Type { get; set; }
        [Required]
        [Range(1, double.MaxValue,ErrorMessage ="Price Can Not Be Zero")]
        public decimal Price { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity Must be One Item At Least ")]
        public int Quantity { get; set; }
    }
}