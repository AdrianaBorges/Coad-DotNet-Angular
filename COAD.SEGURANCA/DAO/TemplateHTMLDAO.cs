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
using COAD.SEGURANCA.Model.Dto;
using COAD.SEGURANCA.Model.Dto.Custons.Pesquisas;

namespace COAD.SEGURANCA.DAO
{
    public class TemplateHTMLDAO : DAOAdapter<TEMPLATE_HTML, TemplateHTMLDTO, int>
    {
        public COADSYSEntities db { get { return GetDb<COADSYSEntities>(); } set { } }

        public TemplateHTMLDAO()
        {
            SetProfileName("coadsys");
        }

        public Pagina<TemplateHTMLDTO> PesquisarTemplatesHTML(PesquisaTemplatesDTO pesquisaDTO)
        {
            int? tplId = pesquisaDTO.tplId;
            string descricao = pesquisaDTO.descricao;
            Nullable<bool> layout = pesquisaDTO.layout;
            Nullable<int> tgrId = pesquisaDTO.tgrId;
            int pagina = pesquisaDTO.pagina;
            int registrosPorPagina = pesquisaDTO.registrosPorPagina;

            var query = (from tpl in db.TEMPLATE_HTML
                         where
                            (tpl.DATA_EXCLUIR == null) &&
                            (tplId == null || tpl.TPL_ID == tplId) &&
                            (descricao == null || tpl.TPL_DESCRICAO.Contains(descricao)) &&
                            (layout == null || tpl.LAYOUT == layout) &&
                            (tgrId == null || tpl.TGR_ID == tgrId)
                         select tpl);
            return ToDTOPage(query, pagina, registrosPorPagina);
        }

        public Pagina<TemplateHTMLDTO> PesquisarTemplatesLayout(
            int pagina = 0,
            int registrosPorPagina = 10)
        {
            var query = (from tpl in db.TEMPLATE_HTML
                         where
                            tpl.DATA_EXCLUIR == null &&
                            tpl.LAYOUT == true
                         select tpl);
            return ToDTOPage(query, pagina, registrosPorPagina);
        }

        public TemplateHTMLDTO RetornarTemplatePorFuncionalidade(int? fsiId)
        {
            var query = (from 
                            templ in db.TEMPLATE_HTML join 
                            funSisRef in db.FUNCIONALIDADE_SISTEMA on templ.TPL_ID equals funSisRef.TPL_ID
                         where funSisRef.FSI_ID == fsiId
                         select templ)
                         .FirstOrDefault();
            return ToDTO(query);
        }
    }
}
