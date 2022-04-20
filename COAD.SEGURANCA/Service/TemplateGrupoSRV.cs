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
using COAD.SEGURANCA.Model.Dto;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("TGR_ID")]
    public class TemplateGrupoSRV : GenericService<TEMPLATE_GRUPO, TemplateGrupoDTO, int>
    {
        private TemplateGrupoDAO _dao { get; set; }

        public TemplateGrupoSRV(TemplateGrupoDAO _dao)
        {
            this._dao = _dao;
            this.Dao = _dao;
        }

        public TemplateGrupoSRV()
        {
            this._dao = new TemplateGrupoDAO();
            this.Dao = _dao;
        }
    }
}
