using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Repositorios.Base;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("REM_ID")]
    public class ParcelasRemessaSRV : ServiceAdapter<PARCELAS_REMESSA, ParcelasRemessaDTO, int>
    {
        private ParcelasRemessaDAO _dao = new ParcelasRemessaDAO();

        public ParcelasRemessaSRV()
        {
            SetDao(_dao);
        }
        public IList<ParcelasRemessaDTO> Listar(string status = null)
        {
            return _dao.Listar(status);
        }

        public IList<ParcelasRemessaDTO> ListarRemessa(DateTime _dtini, DateTime _dtfinal, int _emp_id, string _ban_id = null)
        {
            return _dao.ListarRemessa(_dtini, _dtfinal, _emp_id, _ban_id);
        }
        public Pagina<ParcelasRemessaDTO> ListarRemessa(DateTime _dtini, DateTime _dtfinal, int _emp_id, string _ban_id = null, int pagina = 1, int itensPorPagina = 10)
        {
            return _dao.ListarRemessa(_dtini, _dtfinal, _emp_id, _ban_id, pagina, itensPorPagina);
        }

    }

}
