using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Service.Base;
using COAD.PORTAL.DAO.PortalCoad;
using COAD.PORTAL.Model.DTO.PortalCoad;
using COAD.PORTAL.Repositorios.Contexto;

namespace COAD.PORTAL.Service.PortalCoad
{
    public class Noticias_ConteudoSRV : GenericService<noticias_conteudo, NoticiasConteudoDTO, int>
    {        
        private NoticiasConteudoDAO _dao = new NoticiasConteudoDAO();

        public Noticias_ConteudoSRV()
        {
            Dao = _dao;
        }

        public NoticiasConteudoDTO BuscarPorIdDaNoticia(int? idNoticia = null)
        {
            return _dao.BuscarPorIdDaNoticia(idNoticia);
        }
    }
}
