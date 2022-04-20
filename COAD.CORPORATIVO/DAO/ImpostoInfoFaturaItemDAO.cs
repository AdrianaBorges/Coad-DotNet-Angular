

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
    public class ImpostoInfoFaturaItemDAO : AbstractGenericDao<IMPOSTO_INFO_FATURA_ITEM, ImpostoInfoFaturaItemDTO, Int32>
    {
        public CORPORATIVOContext db { get { return GetDb<CORPORATIVOContext>(); } set { } }

        public ImpostoInfoFaturaItemDAO()
        {
            db = GetDb<CORPORATIVOContext>(false);
        }

        public ICollection<ImpostoInfoFaturaItemDTO> ListarImpostoInfoFatura(int? ifiId)
        {
            var query = (from imIfFatItm in 
                             db.IMPOSTO_INFO_FATURA_ITEM
                         where imIfFatItm.IFI_ID == ifiId
                        select imIfFatItm);
            return ToDTO(query);
        }
    }
}
