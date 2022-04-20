using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coad.GenericCrud.Dao.Base.Pagination
{
    public class Pagina<T>
    {
        public IEnumerable<T> lista {get; set;}
        public int pagina { get; set; }
        public int numeroPaginas { get; set; }
        public int itensPorPagina { get; set; }
        public int numeroRegistros { get; set; }

        public Pagina<D> Derivar<D>(IEnumerable<D> items)
        {
            return new Pagina<D>()
                {
                    lista = items,
                    pagina = pagina,
                    numeroPaginas = numeroPaginas,
                    itensPorPagina = itensPorPagina,
                    numeroRegistros = numeroRegistros
                };
            
        }
              
    }
}