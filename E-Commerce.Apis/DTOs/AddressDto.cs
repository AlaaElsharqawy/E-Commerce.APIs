﻿using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Apis.DTOs
{
    public class AddressDto
    {

        //public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string City { get; set; }
        [Required]

        public string Street { get; set; }
        [Required]
        public string Country { get; set; }
    }
}
