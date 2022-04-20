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
    public class OcorrenciaRetornoDAO : AbstractGenericDao<OCORRENCIA_RETORNO, OcorrenciaRetornoDTO, string>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public OcorrenciaRetornoDAO()
        {
            db = GetDb<COADCORPEntities>(false);
        }
        public IList<OcorrenciaRetornoDTO> LerOcorrenciaRetorno(string _ban_id)
        {
            var query = (from o in db.OCORRENCIA_RETORNO
                         where o.BAN_ID == _ban_id
                        select o).ToList();

            return ToDTO(query);

        }

        public Pagina<OcorrenciaRetornoDTO> LerOcorrenciaRetorno(string bco = null, string cod = null, bool? baixa = null, bool? desaloca = null, bool? registra = null, int pagina = 1, int itensPorPagina = 10)
        {
            IQueryable<OCORRENCIA_RETORNO> query = GetDbSet();

            if (!String.IsNullOrWhiteSpace(bco))
            {
                query = query.Where(x => x.BAN_ID == bco);
            }
            if (!String.IsNullOrWhiteSpace(cod))
            {
                query = query.Where(x => x.OCT_CODIGO == cod);
            }
            if (baixa != null)
            {
                query = query.Where(x => x.OCT_BAIXAR_TITULO == baixa);
            }
            if (desaloca != null)
            {
                query = query.Where(x => x.OCT_DESALOCAR_TITULO == desaloca);
            }
            if (registra != null)
            {
                query = query.Where(x => x.OCT_REGISTRAR_TITULO == registra);
            }

            return ToDTOPage(query, pagina, itensPorPagina);
        }
    }
}
