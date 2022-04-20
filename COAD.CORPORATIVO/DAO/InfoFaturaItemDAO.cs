

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
    public class InfoFaturaItemDAO : AbstractGenericDao<INFO_FATURA_ITEM, InfoFaturaItemDTO, Int32>
    {
        public CORPORATIVOContext db { get { return GetDb<CORPORATIVOContext>(); } set { } }

        public InfoFaturaItemDAO()
        {
            db = GetDb<CORPORATIVOContext>(false);
        }

        public IList<InfoFaturaItemDTO> ListarInfoFaturaItemDaInfoFatura(int? iffId)
        {
            var query = (from
                            infFatItm in db.INFO_FATURA_ITEM
                         where infFatItm.IFF_ID == iffId
                         select
                            infFatItm);

            return ToDTO(query);
        }

        public InfoFaturaItemDTO ListarInfoFaturaItemPorConfig(int? iffId, int? nfcId)
        {
            var query = (from 
                            infFaturaItm in db.INFO_FATURA_ITEM
                        where 
                            infFaturaItm.IFF_ID == iffId &&
                            infFaturaItm.NFC_ID == nfcId
                        select
                            infFaturaItm);
            return ToDTO(query.FirstOrDefault());
        }

        public InfoFaturaItemDTO ListarInfoFaturaPorItem(int? iffId)
        {

            var query = (from
                            infFaturaItm in db.INFO_FATURA_ITEM
                         where
                             infFaturaItm.IFF_ID == iffId &&
                             infFaturaItm.NCT_ID == 1
                         select
                             infFaturaItm);
            return ToDTO(query.FirstOrDefault());
        }

    }
}
