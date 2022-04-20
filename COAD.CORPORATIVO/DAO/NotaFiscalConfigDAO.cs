

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

namespace COAD.CORPORATIVO.DAO
{
    public class NotaFiscalConfigDAO : AbstractGenericDao<NOTA_FISCAL_CONFIG, NotaFiscalConfigDTO, Int32>
    {
        public CORPORATIVOContext db { get { return GetDb<CORPORATIVOContext>(); } set { } }

        public NotaFiscalConfigDAO()
        {
            db = GetDb<CORPORATIVOContext>(false);
        }

        public IList<NotaFiscalConfigDTO> ListarNotaFiscalConfig(int? cmpId, int? nctId = null, bool faturado100PorCento = false)
        {
            var query = (from nfc in
                             db.NOTA_FISCAL_CONFIG
                         where
                            nfc.CMP_ID == cmpId &&
                            (nctId == null || nfc.NCT_ID == nctId) &&
                            (faturado100PorCento == false || nfc.NFC_APLICAR_100_POR_CENTO_FAT == faturado100PorCento) &&
                            nfc.DATA_EXCLUSAO == null
                         select nfc);
            return ToDTO(query);
        }

        public bool ChecarEmitirNotaFiscal(int? cmpId, int? nctId)
        {
            var query = (from nfc in
                             db.NOTA_FISCAL_CONFIG
                         where
                            nfc.CMP_ID == cmpId &&
                            (nctId == null || nfc.NCT_ID == nctId) &&
                            nfc.DATA_EXCLUSAO == null
                         select nfc);
            return (query.Count() > 0);
        }
        
    }
}
