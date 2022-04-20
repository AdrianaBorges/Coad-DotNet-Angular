using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Models.Filtros
{
    public class RequisicaoPaginacao
    {
        public RequisicaoPaginacao()
        {
            pagina = 1;
            registrosPorPagina = 15;
        }

        public FiltroOrdenacao ordenacao { get; set; }
        public int pagina { get; set; }
        public int registrosPorPagina { get; set; }
    }
}
