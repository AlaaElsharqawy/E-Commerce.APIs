using E_Commerce.Core.Entities.Product_Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Specifications
{
    public class BaseSpecifications<T> : ISpecifications<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get ; set ; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDescending { get; set; }
        public int Skip { get; set; }

        public int Take { get; set; }
        public bool IsPaginationEnabled { get; set; }


       
     


        public BaseSpecifications()
        {
            
        }

        public BaseSpecifications(Expression<Func<T, bool>> criteria)
        {
            Criteria= criteria;
        }


        public void ApplyOrderBy(Expression<Func<T, object>> OrderbyExpression)
        {
            OrderBy = OrderbyExpression;
        }

        public void ApplyOrderByDescending(Expression<Func<T, object>> OrderbyDescendingExpression)
        {
            OrderByDescending = OrderbyDescendingExpression;
        }

        //set Pagination
        public void ApplyPagination(int pageSize, int pageIndex )
        {
            IsPaginationEnabled = true;
            Skip = pageSize * (pageIndex - 1);
            Take = pageSize;
        }

    }
}
