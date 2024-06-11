using E_Commerce.Core.Entities.Product_Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Specifications
{
   public class ProductWithFiltrationForCountAsync:BaseSpecifications<Product>
    {
        public ProductWithFiltrationForCountAsync(ProductSpecParams @params) :base(p=>
        (string.IsNullOrEmpty(@params.Search)||p.Name.ToLower().Contains(@params.Search))&&
        (!@params.BrandId.HasValue || p.ProductBrandId == @params.BrandId) &&
         (!@params.TypeId.HasValue || p.ProductTypeId == @params.TypeId))
        {
            
        }

    }
}
