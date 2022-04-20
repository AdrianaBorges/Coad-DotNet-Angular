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
using System.Configuration;
using System.Web.Configuration;

namespace COAD.CORPORATIVO.DAO
{
    public class OcorrenciaErroDAO : AbstractGenericDao<OCORRENCIA_ERRO, OcorrenciaErroDTO, string>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public OcorrenciaErroDAO()
        {
            db = GetDb<COADCORPEntities>(false);
        }

        public Pagina<OcorrenciaErroDTO> LerOcorrenciaErro(string bco=null, string cod=null, string codRet=null, int pagina = 1, int itensPorPagina = 10)
        {
            IQueryable<OCORRENCIA_ERRO> query = GetDbSet();

            if (!String.IsNullOrWhiteSpace(bco))
            {
                query = query.Where(x => x.BAN_ID == bco);
            }
            if (!String.IsNullOrWhiteSpace(cod))
            {
                query = query.Where(x => x.OCE_CODIGO == cod);
            }
            if (!String.IsNullOrWhiteSpace(codRet))
            {
                query = query.Where(x => x.OCT_CODIGO == codRet);
            }
            
            return ToDTOPage(query, pagina, itensPorPagina);
        }
    }
}
