using E_Commerce.Core.Entities.Product_Module;
using E_Commerce.Core.Interfaces;
using E_Commerce.Core.Interfaces.Repositories;
using E_Commerce.Repository.Data.Contexts;
using E_Commerce.Repository.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _dbContext;
        private readonly Hashtable _hashTableRepositories;

        public UnitOfWork(StoreContext dbContext)
        {
            _dbContext = dbContext;
            _hashTableRepositories = new Hashtable();
        }





        public async Task<int> CompleteAsync()
        {
          return await _dbContext.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
          await _dbContext.DisposeAsync();
        }

        //Repositories store in Dictionary or HashTable
        //HashTable (Key Value Pairs) Key =>product ,Value=> new GenericRepository<Product>();
        //Type for Key And Value is Object
        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var Key=typeof(TEntity).Name;
            if(! _hashTableRepositories.ContainsKey(Key) )
            {
                var Repository = new GenericRepository<TEntity>(_dbContext);
                _hashTableRepositories.Add(Key, Repository);

            }
            return _hashTableRepositories[Key] as IGenericRepository<TEntity>;

           
           


           
        }
    }
}
