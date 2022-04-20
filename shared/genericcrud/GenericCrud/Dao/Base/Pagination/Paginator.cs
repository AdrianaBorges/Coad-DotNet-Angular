using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coad.GenericCrud.Dao.Base.Pagination
{
    public class Paginator
    {
        public static Pagina<T> paginar<T>(IEnumerable<T> query, int pagina, int itensPorPagina)
        {
            int count = query.Count();

            Pagina<T> pageRequest = new Pagina<T>();
                        
            double valorDouble = ((double) count / (double) itensPorPagina); // número de páginas
            int paginas = Convert.ToInt32(Math.Ceiling(valorDouble));

            pagina = (pagina > paginas) ? paginas : pagina; // limita o numero da pagina requisitada para no máximo o numero de páginas
            
            pagina = (pagina < 1) ? 1 : pagina; // previne que o número da página fique negativo ou igual a 0
            int registro = (pagina - 1) * itensPorPagina; // calcula qual registo de inicio para a consulta
            query = query.Skip(registro); // seta o inicio do registro
            query = query.Take(itensPorPagina); // (verbo) pagina 

            pageRequest.itensPorPagina = itensPorPagina;
            pageRequest.numeroPaginas = paginas; // indica o número de páginas
            pageRequest.pagina = pagina;
            pageRequest.lista = query;
            pageRequest.numeroRegistros = count;
            return pageRequest;
        }

        public static int numeroPaginas<T>(IEnumerable<T> query, int itensPorPagina)
        {
            int count = query.Count();
            double valorDouble = ((double)count / (double)itensPorPagina);
            int paginas = Convert.ToInt32(Math.Ceiling(valorDouble));

            return paginas;
        }

    }
}