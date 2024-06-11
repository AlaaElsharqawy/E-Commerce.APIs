using E_Commerce.Core.Entities.Product_Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Specifications
{
    public interface ISpecifications<T> where T : BaseEntity
    {

        //Signiture for property Criteria

        public Expression<Func<T, bool>> Criteria { get; set; }


        //signiture for property List<Includes>

        public List< Expression<Func<T, object>>> Includes { get; set; }


        //signiture for property OrderBy
        public Expression<Func<T,object>> OrderBy { get; set; }


        //signiture for property OrderByDescending
        public Expression<Func<T, object>> OrderByDescending { get; set; }



        //signiture for property Skip
        public int Skip { get; set; }



        //signiture for property Take
        public int Take { get; set; }


        public bool IsPaginationEnabled { get; set; }

        

    }
}
