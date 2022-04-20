using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.PORTAL.Model.DTO.PortalCoad;
using COAD.PORTAL.Repositorios.Contexto;

namespace COAD.PORTAL.DAO.PortalCoad
{
    public class NoticiasConteudoDAO : AbstractGenericDao<noticias_conteudo, NoticiasConteudoDTO, int>
    {
        private coadEntities db { get; set; }

        public NoticiasConteudoDAO()
        {
            SetProfileName("portalCoad");
            //db = GetDb<COADEntities>(false);
        }

        public NoticiasConteudoDTO BuscarPorIdDaNoticia(int? idNoticia = null)
        {
            noticias_conteudo query = GetDbSet().Where(x => x.id_noticia == (int)idNoticia).FirstOrDefault();
            
            return ToDTO(query);
        }
    }
}
