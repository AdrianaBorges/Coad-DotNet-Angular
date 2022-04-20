

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Dao.Base;
using COAD.CORPORATIVO.Repository.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;

namespace COAD.CORPORATIVO.DAO
{
    public class CnabConfigArquivoDAO : AbstractGenericDao<CNAB_CONFIG_ARQUIVO, CnabConfigArquivoDTO, Int32>
    {
        public CORPORATIVOContext db { get { return GetDb<CORPORATIVOContext>(); } set { } }

        public CnabConfigArquivoDAO()
        {
            db = GetDb<CORPORATIVOContext>(false);
        }

        public IList<CnabConfigArquivoDTO> ListarCnabsConfigArquivoDaConfig(int? cncId)
        {
            var query = (from cnb in db.CNAB_CONFIG_ARQUIVO
                         where
                            cnb.DATA_EXCLUSAO == null &&
                            cnb.CNC_ID == cncId
                         select cnb);
            return ToDTO(query);
        }


    }
}
