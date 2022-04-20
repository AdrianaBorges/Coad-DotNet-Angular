using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{
    public class ImpostoInfoFaturaDAO : DAOAdapter<IMPOSTO_INFO_FATURA, ImpostoInfoFaturaDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public ImpostoInfoFaturaDAO()
        {
            db = GetDb<COADCORPEntities>();
        }

        public IList<ImpostoInfoFaturaDTO> ListByIffId(int? iffId)
        {
            var query = GetDbSet().Where(x => x.IFF_ID == iffId);
            return ToDTO(query);

        }


    }
}
