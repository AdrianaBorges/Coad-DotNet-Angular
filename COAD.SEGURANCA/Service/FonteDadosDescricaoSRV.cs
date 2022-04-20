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
    [ServiceConfig("FDD_ID")]
    public class FonteDadosDescricaoSRV : GenericService<FONTE_DADOS_DESCRICAO, FonteDadosDescricaoDTO, int>
    {
        private FonteDadosDescricaoDAO _dao { get; set; }

        public FonteDadosDescricaoSRV(FonteDadosDescricaoDAO _dao) 
        {
            this._dao = _dao;
            this.Dao = _dao;
        }

        public FonteDadosDescricaoSRV()
        {
            this._dao = new FonteDadosDescricaoDAO();
            this.Dao = _dao;
        }

        public IList<FonteDadosDescricaoDTO> ListarFonteDadosDescricaoDaFonteDeDados(int? fdaId)
        {
            return _dao.ListarFonteDadosDescricaoDaFonteDeDados(fdaId);
        }

        public void PreencherFonteDadosDescricaoNaFonteDados(FonteDadosTemplateDTO template)
        {
            if(template != null && template.FDA_ID != null)
            {
                template.FONTE_DADOS_DESCRICAO = ListarFonteDadosDescricaoDaFonteDeDados(template.FDA_ID);
            }
        }
    }
}
