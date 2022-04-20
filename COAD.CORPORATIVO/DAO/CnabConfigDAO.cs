

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Dao.Base;
using COAD.CORPORATIVO.Repository.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.Model.Dto.Custons.Pesquisas;

namespace COAD.CORPORATIVO.DAO
{
    public class CnabConfigDAO : AbstractGenericDao<CNAB_CONFIG, CnabConfigDTO, Int32>
    {
        public CORPORATIVOContext db { get { return GetDb<CORPORATIVOContext>(); } set { } }

        public CnabConfigDAO()
        {
            db = GetDb<CORPORATIVOContext>(false);
        }

        public Pagina<CnabConfigDTO> PesquisarCnabConfig(PesquisaCnabConfigDTO pesquisa)
        {
            var empId = pesquisa.empId;
            var banId = pesquisa.banId;
            var tipoRegistro = pesquisa.tipoRegistro;

            var query = (from cnabConfig
                            in db.CNAB_CONFIG
                         where
                            cnabConfig.DATA_EXCLUSAO == null &&
                             (empId == null || cnabConfig.EMP_ID == empId) &&
                             (banId == null || cnabConfig.BAN_ID == banId) &&
                             (tipoRegistro == null || cnabConfig.CNC_TIPO_REGISTRO == tipoRegistro)
                         select cnabConfig);

            return ToDTOPage(query, pesquisa.pagina, pesquisa.registrosPorPagina);
        }

        
    }
}
