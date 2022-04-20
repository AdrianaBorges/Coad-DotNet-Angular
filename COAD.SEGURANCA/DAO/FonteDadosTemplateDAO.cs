
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
    public class FonteDadosTemplateDAO : DAOAdapter<FONTE_DADOS_TEMPLATE, FonteDadosTemplateDTO, int>
    {
        public COADSYSEntities db { get { return GetDb<COADSYSEntities>(); } set { } }
        
        public FonteDadosTemplateDAO()
        {
            SetProfileName("coadsys");
        }

        public Pagina<FonteDadosTemplateDTO> PesquisarFonteDadosTemplate(int? fdaId, string descricao, int pagina = 1, int registrosPorPagina = 6)
        {
            var query = (from fDados in db.FONTE_DADOS_TEMPLATE
                         where 
                            (fdaId == null || fDados.FDA_ID == fdaId) &&
                            (descricao == null || fDados.FDA_DESCRICAO.Contains(descricao))
                         select fDados);

            return ToDTOPage(query, pagina, registrosPorPagina);
        }

        //public IList<FonteDadosTemplateDTO> ListarFonteDadosTemplateDaFonteDeDados(int? fdaId)
        //{
        //    var query = (from fDadosDesc in db.FONTE_DADOS_DESCRICAO
        //                 where fDadosDesc.FDA_ID == fdaId
        //                 select fDadosDesc);
        //    return ToDTO(query);
        //}
    }
}
