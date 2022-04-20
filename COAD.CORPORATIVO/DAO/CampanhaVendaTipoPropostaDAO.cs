using Coad.GenericCrud.Dao.Base.Pagination;
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
    public class CampanhaVendaTipoPropostaDAO : DAOAdapter<CAMPANHA_VENDA_TIPO_PROPOSTA, CampanhaVendaTipoPropostaDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public CampanhaVendaTipoPropostaDAO()
        {
            db = GetDb<COADCORPEntities>();
        }
        
        public ICollection<CampanhaVendaTipoPropostaDTO> ListarCampanhaTipoProposta(int? cveId)
        {
            var query = (from
                            camTpPro in db.CAMPANHA_VENDA_TIPO_PROPOSTA
                         where
                            camTpPro.CVE_ID == cveId
                         select camTpPro);

            return ToDTO(query);
        }

    }
}
