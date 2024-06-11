using E_Commerce.Core.Entities.Order_Module;
using E_Commerce.Core.Entities.Product_Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Repository.Data.Contexts
{
   public static class DataContextSeed
    {


        public static async Task SeedAsync(StoreContext _context)
        {
            #region Seeding productBrand 

            if(!_context.Set<ProductBrand>().Any()) 
            {

               // 1)read data from files
                var BrandsData =await File.ReadAllTextAsync(@"..\E_Commerce.Repository\Data\DataSeed\brands.json");
                //2)convert data to c# object

                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData);

                //3) insert data into DB

                if(Brands is not null&& Brands.Any())
                {
                    foreach(var Brand in Brands)
                    {
                      await  _context.Set<ProductBrand>().AddRangeAsync(Brand);
                    }
                   await _context.SaveChangesAsync();
                }
            
            }



            #endregion



            #region Seeding ProductTypes

            if (!_context.Set<ProductType>().Any())
            {

                // 1)read data from files
                var TypesData = await File.ReadAllTextAsync(@"..\E_Commerce.Repository\Data\DataSeed\types.json");
                //2)convert data to c# object

                var Types = JsonSerializer.Deserialize<List<ProductType>>(TypesData);

                //3) insert data into DB

                if (Types is not null && Types.Any())
                {
                    foreach (var Type in Types)
                    {
                        await _context.Set<ProductType>().AddRangeAsync(Type);
                    }
                    await _context.SaveChangesAsync();
                }

            }



            #endregion



            #region Seeding Product

            if (!_context.Set<Product>().Any())
            {

                // 1)read data from files
                var ProductsData = await File.ReadAllTextAsync(@"..\E_Commerce.Repository\Data\DataSeed\products.json");
                //2)convert data to c# object

                var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData);

                //3) insert data into DB

                if (Products is not null && Products.Any())
                {
                    foreach (var Product in Products)
                    {
                        await _context.Set<Product>().AddRangeAsync(Product);
                    }
                    await _context.SaveChangesAsync();
                }

            }


            #endregion

            #region Seeding DeliveryMethod

            if (!_context.Set<DeliveryMethod>().Any())
            {

                // 1)read data from files
                var DeliveryMethodsData = await File.ReadAllTextAsync(@"..\E_Commerce.Repository\Data\DataSeed\delivery.json");
                //2)convert data to c# object

                var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodsData);

                //3) insert data into DB

                if (DeliveryMethods is not null && DeliveryMethods.Any())
                {
                    foreach (var DeliveryMethod in DeliveryMethods)
                    {
                        await _context.Set<DeliveryMethod>().AddRangeAsync(DeliveryMethod);
                    }
                    await _context.SaveChangesAsync();
                }

            }
            #endregion




        }

    }
}
