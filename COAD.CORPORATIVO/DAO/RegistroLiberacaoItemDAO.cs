using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons.Pesquisas;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{
    public class RegistroLiberacaoItemDAO : DAOAdapter<REGISTRO_LIBERACAO_ITEM, RegistroLiberacaoItemDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public RegistroLiberacaoItemDAO()
        {
            db = GetDb<COADCORPEntities>();
        }
        
        public IList<RegistroLiberacaoItemDTO> ListarRegistroItemPorRegistroAtivo(int? rliId)
        {
            var query = (from reItm in db.REGISTRO_LIBERACAO_ITEM
                         where  
                            reItm.RLI_ID == rliId &&
                            reItm.RIT_DATA_ACAO == null &&
                            reItm.RIT_LIBERADO == null
                         select reItm);
            return ToDTO(query);
        }

        public IList<RegistroLiberacaoItemDTO> ListarRegistroItemPorRegistro(int? rliId)
        {
            var query = (from reItm in db.REGISTRO_LIBERACAO_ITEM
                         where reItm.RLI_ID == rliId
                         select reItm);
            return ToDTO(query);
        }
    }
}
