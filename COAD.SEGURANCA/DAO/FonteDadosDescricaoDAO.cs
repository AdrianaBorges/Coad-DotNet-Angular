
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using Coad.GenericCrud.Repositorios.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.Model;
using COAD.SEGURANCA.Model.Dto;

namespace COAD.SEGURANCA.DAO
{
    public class FonteDadosDescricaoDAO : DAOAdapter<FONTE_DADOS_DESCRICAO, FonteDadosDescricaoDTO, int>
    {
        public COADSYSEntities db { get { return GetDb<COADSYSEntities>(); } set { } }
        
        public FonteDadosDescricaoDAO()
        {
            SetProfileName("coadsys");            
        }        

        public IList<FonteDadosDescricaoDTO> ListarFonteDadosDescricaoDaFonteDeDados(int? fdaId)
        {
            var query = (from fDadosDesc in db.FONTE_DADOS_DESCRICAO
                         where fDadosDesc.FDA_ID == fdaId
                         select fDadosDesc);
            return ToDTO(query);
        }
    }
}
