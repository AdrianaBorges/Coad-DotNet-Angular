using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{
    public class ContaRefDAO : AbstractGenericDao<CONTA_REF, ContaRefDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public ContaRefDAO()
        {
            db = GetDb<COADCORPEntities>();
        }

        public Pagina<ContaRefDTO> Conta(int? empresa = null, string banco=null, string agencia=null, string conta=null, string tipo=null, int pagina = 1, int itensPorPagina = 10)
        {
            IQueryable<CONTA_REF> query = GetDbSet();

            return ToDTOPage(query, pagina, itensPorPagina);
        }
    }
}
