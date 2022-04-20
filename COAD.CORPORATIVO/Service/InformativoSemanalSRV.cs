
using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Repositorios.Base;
using GenericCrud.Config.DataAttributes;
using Coad.GenericCrud.Dao.Base.Pagination;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("INF_ANO", "INF_REMESSA", "INF_ENVIO")]
    public class InformativoSemanalSRV : GenericService<INFORMATIVO_SEMANAL, InformativoSemanalDTO, object>
    {
        public InformativoSemanalDAO _dao = new InformativoSemanalDAO();

        public InformativoSemanalSRV()
        {
            this.Dao = _dao;
        }

        public Pagina<InformativoSemanalDTO> Buscar(int _tipo, DateTime _dataini, DateTime _datafim, int pagina, int linhasPorPaginas)
        {
            return _dao.Buscar(_tipo, _dataini, _datafim, pagina, linhasPorPaginas);
        }
    }
}
