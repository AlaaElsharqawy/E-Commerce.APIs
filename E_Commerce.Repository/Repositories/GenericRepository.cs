using E_Commerce.Core.Entities.Product_Module;
using E_Commerce.Core.Interfaces.Repositories;
using E_Commerce.Core.Specifications;
using E_Commerce.Repository.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _context;

        public GenericRepository(StoreContext context)
        {
            _context = context;
        }

        #region Get Product WithOutSpec

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            //if (typeof(T) == typeof(Product))
            //{
            //    return (IReadOnlyList<T>)await _context.products.Include(p => p.ProductBrand).Include(p => p.ProductType).ToListAsync();
            //}

            return await _context.Set<T>().ToListAsync();


        }

     

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }


        #endregion



        #region Get Product With Spec


        // _context.products.Include(p => p.ProductBrand).Include(p => p.ProductType).ToListAsync();
    
    public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }



        public async Task<T> GetEntityWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }



        private IQueryable<T> ApplySpecification(ISpecifications<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>(), spec);
        }

        public async Task<int> GetCountWithSpecAsync(ISpecifications<T> spec)
        {
           return await ApplySpecification(spec).CountAsync();
        }

        public  async Task AddAsync(T item)
        { 
       await  _context.Set<T>().AddAsync(item);
        }

        public void Delete(T item)
        {
           _context.Set<T>().Remove(item);
        }

        public void Update(T item)
        {
            _context.Set<T>().Update(item);
        }



        #endregion
    }
}
