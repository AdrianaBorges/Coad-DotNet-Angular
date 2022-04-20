
using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Repositorios.Base;
using GenericCrud.Config.DataAttributes;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("CMP_ID", "ARE_CONS_ID", "TIT_ID")]
    public class AreaConsultoriaCursoSRV : GenericService<AREA_CONSULTORIA_CURSO, AreaConsultoriaCursoDTO, int>
    {
        
        public AreaConsultoriaCursoDAO _dao = new AreaConsultoriaCursoDAO();

        public AreaConsultoriaCursoSRV()
        {
            this.Dao = _dao;
        }        
    }
}
