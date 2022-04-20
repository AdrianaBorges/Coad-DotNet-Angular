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
    public class CoObrigacoesSRV : GenericService<CO_OBRIGACOES, CoObrigacoesDTO, string>
    {
        private CoObrigacoesDAO _dao = new CoObrigacoesDAO();

        public CoObrigacoesSRV()
        {
            Dao = _dao;
           
        }

        public CoObrigacoesDTO DTOObrigacao(long id)
        {
            return _dao.ToDTO(_dao.FindById(id));
        }

    }
}
