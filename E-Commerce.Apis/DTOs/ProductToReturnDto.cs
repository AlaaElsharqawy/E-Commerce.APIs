﻿using E_Commerce.Core.Entities.Product_Module;

namespace E_Commerce.Apis.DTOs
{
    public class ProductToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string PictureUrl { get; set; }


        public decimal Price { get; set; }


        public int ProductBrandId { get; set; }//FK
        public string ProductBrand { get; set; }


        public int ProductTypeId { get; set; }//FK
        public string ProductType { get; set; }
    }
}
