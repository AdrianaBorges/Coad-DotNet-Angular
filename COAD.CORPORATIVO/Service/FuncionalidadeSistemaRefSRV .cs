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
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Model.Dto;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("FSI_ID")]
    public class TemplateHTMLFuncionalidadeSistemaRefSRV : GenericService<FUNCIONALIDADE_SISTEMA_REF, FuncionalidadeSistemaRefDTO, int>
    {
        private FuncionalidadeSistemaRefDAO _dao { get; set; }

        public TemplateHTMLFuncionalidadeSistemaRefSRV(FuncionalidadeSistemaRefDAO _dao)
        {
            this._dao = _dao;
            this.Dao = _dao;
        }

        public TemplateHTMLFuncionalidadeSistemaRefSRV()
        {
            this._dao = new FuncionalidadeSistemaRefDAO();
            this.Dao = _dao;
        }
    }
}
