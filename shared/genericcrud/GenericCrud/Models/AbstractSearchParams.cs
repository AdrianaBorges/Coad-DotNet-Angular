using GenericCrud.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Models
{
    public abstract class AbstractSearchParams<TEntity, D> where TEntity : class
    {
        public AbstractSearchParams()
        {
            Pagina = 1;
            PageSize = 15;
        }

        public int Pagina { get; set; }
        public int PageSize { get; set; }

        public IList<QueryParam> GetParams()
        {
            return new List<QueryParam>();
        }
    }
}
