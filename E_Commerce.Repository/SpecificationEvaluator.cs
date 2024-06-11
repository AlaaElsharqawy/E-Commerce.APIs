using E_Commerce.Core.Entities.Product_Module;
using E_Commerce.Core.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Repository
{
   public static class SpecificationEvaluator<T> where T : BaseEntity
    {

        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery,ISpecifications<T> spec)
        {
            var query = inputQuery;//_dbContext.Set<T>

            
            if (spec.Criteria is not null)
            {
                query=query.Where(spec.Criteria);//_dbContext.Set<T>.where(p=>p.Id)
            }

            if(spec.OrderBy is not null)
            {
                query=query.OrderBy(spec.OrderBy);
            }

            if(spec.OrderByDescending is not null)
            {
                query=query.OrderByDescending(spec.OrderByDescending);
            }


            if(spec.IsPaginationEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }


            query = spec.Includes.Aggregate(query,(CurrentQuery,IncludeExpression)=>CurrentQuery.Include(IncludeExpression));

            //_dbContext.Set<T>.where(p=>p.Id).Include(p=>p.productType).Include(p=>p.productBrand)
            return query;
        }


    }
}
