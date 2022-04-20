using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.LEGADO.Dao;
using COAD.CORPORATIVO.LEGADO.Model.Dto;
using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Service
{
    public class AgendaSRV : GenericService<AGENDA, AgendaDTO, object>
    {
        private AgendaDAO _dao = new AgendaDAO();

        public AgendaSRV()
        {
            Dao = _dao;
        }
        public Pagina<AgendaDTO> BuscarPorAssinatura(string _assinatura = null, int pagina = 1, int registroPorPagina = 10)
        {
            return _dao.BuscarPorAssinatura(_assinatura, pagina, registroPorPagina);
        }
        public IList<AgendaDTO> ListarPorAssinatura(string _assinatura)
        {
            return _dao.ListarPorAssinatura(_assinatura);
        }
        public Pagina<AgendaDTO> BuscarPorCliente(string _assinatura = null, int pagina = 1, int registroPorPagina = 10)
        {
            return _dao.BuscarPorCliente(_assinatura, pagina, registroPorPagina);
        }

        public IList<AgendaDTO> BuscarPorCliente(string _cli_id)
        {
            return _dao.BuscarPorCliente(_cli_id);
        }
        

    }
}
