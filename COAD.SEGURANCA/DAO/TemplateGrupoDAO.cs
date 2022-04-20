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

namespace COAD.SEGURANCA.DAO
{
    public class TemplateGrupoDAO : DAOAdapter<TEMPLATE_GRUPO, TemplateGrupoDTO, int>
    {
        public COADSYSEntities db { get { return GetDb<COADSYSEntities>(); } set { } }
        
        public TemplateGrupoDAO()
        {
            SetProfileName("coadsys");            
        }        
    }
}
