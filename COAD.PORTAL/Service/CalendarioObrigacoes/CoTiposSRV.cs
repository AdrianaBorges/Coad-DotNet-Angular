using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using COAD.PORTAL.DAO.CalendarioObrigacoes;
using COAD.PORTAL.Model.DTO.CalendarioObrigacoes;
using COAD.PORTAL.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PORTAL.Service.CalendarioObrigacoes
{
    public class CoTiposSRV : GenericService<CO_TIPOS, CoTiposDTO, string>
    {
        private CoTiposDAO _dao = new CoTiposDAO();

        public CoTiposSRV()
        {
            Dao = _dao;
           
        }
    }
}
