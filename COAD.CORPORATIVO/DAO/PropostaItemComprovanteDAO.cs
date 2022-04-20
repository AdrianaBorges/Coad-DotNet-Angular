using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.CORPORATIVO.Model.Dto.Custons;
using GenericCrud.IOCContainer.Proxies;
using System.Data.Objects;
using Coad.GenericCrud.Dao.Base.Pagination;
namespace COAD.CORPORATIVO.DAO
{
    public class PropostaItemComprovanteDAO : DAOAdapter<PROPOSTA_ITEM_COMPROVANTE, PropostaItemComprovanteDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public PropostaItemComprovanteDAO()
        {
            //db = GetDb<COADCORPEntities>();
        }

        public IList<PropostaItemComprovanteDTO> ListarPropostaItemComprovante(int? ppiId = null, int? ipeId = null)
        {
            if(ppiId == null && ipeId == null)
            {
                throw new ArgumentNullException(" Não é possível listar os comprovantes. O parâmetro ppiId e ipeId não foram informados. Informe pelo menos um deles.");
            }
            var query = (from prItm in db.PROPOSTA_ITEM_COMPROVANTE 
                         where 
                             (ppiId == null || prItm.PPI_ID == ppiId) &&
                             (ipeId == null || prItm.IPE_ID == ipeId) &&
                                prItm.DATA_EXCLUSAO == null
                        select prItm);

            return ToDTO(query);
        }
        
    }
}