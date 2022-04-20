using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{
    public class InformativoSemanalDAO : DAOAdapter<INFORMATIVO_SEMANAL, InformativoSemanalDTO, object>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public InformativoSemanalDAO()
        {
            db = GetDb<COADCORPEntities>();
        }

        public Pagina<InformativoSemanalDTO> Buscar(int _tipo, DateTime _dataini, DateTime _datafim, int pagina, int linhasPorPaginas)
        {
            
            var _dtini = new DateTime(_dataini.Year, _dataini.Month, _dataini.Day);
            var _dtfim = new DateTime(_datafim.Year, _datafim.Month, _datafim.Day);
            _dtfim = _dtfim.AddDays(1);

            var _query = (from i in db.INFORMATIVO_SEMANAL
                         where (i.INF_DATA >= _dtini && i.INF_DATA < _dtfim) &&
                               (_tipo == 0 || (_tipo !=0 && i.INF_ENVIO == _tipo))
                        select i).OrderByDescending(x => x.INF_DATA).ToList();
                        
            return ToDTOPage(_query, pagina, linhasPorPaginas);
        }
    }
}
