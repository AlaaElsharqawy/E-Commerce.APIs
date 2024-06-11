using E_Commerce.Core.Entities.Product_Module;
using E_Commerce.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Interfaces.Repositories
{
   public interface IGenericRepository<T> where T : BaseEntity
    {

        #region Get Product WithOut Spec
        Task<IReadOnlyList<T>> GetAllAsync();

        Task<T> GetByIdAsync(int id);
        #endregion

        #region Get Product With Spec

        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T>spec);

        Task<T> GetEntityWithSpecAsync(ISpecifications<T> spec);
         Task<int> GetCountWithSpecAsync(ISpecifications<T> spec );


        #endregion

        Task AddAsync(T item);
        void Delete (T item);
        void Update(T item);

    }
}
