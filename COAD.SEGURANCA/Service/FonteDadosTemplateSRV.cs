using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using COAD.SEGURANCA.Model;
using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.Config.DataAttributes;
using COAD.SEGURANCA.DAO;
using COAD.CORPORATIVO.Model;
using COAD.SEGURANCA.Model.Dto;

namespace COAD.SEGURANCA.Service
{
    [ServiceConfig("FDA_ID")]
    public class FonteDadosTemplateSRV : GenericService<FONTE_DADOS_TEMPLATE, FonteDadosTemplateDTO, int>
    {
        private FonteDadosTemplateDAO _dao { get; set; }

        public FonteDadosTemplateSRV(FonteDadosTemplateDAO _dao) 
        {
            this._dao = _dao;
            this.Dao = _dao;
        }

        public FonteDadosTemplateSRV()
        {
            this._dao = new FonteDadosTemplateDAO();
            this.Dao = _dao;
        }

        public Pagina<FonteDadosTemplateDTO> PesquisarFonteDadosTemplate(int? fdaId, string descricao, int pagina = 1, int registrosPorPagina = 6)
        {
            return _dao.PesquisarFonteDadosTemplate(fdaId, descricao, pagina, registrosPorPagina);
        }
    }
}
