using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Dao.Base;
using COAD.PORTAL.Model.DTO.PortalCoad;
using COAD.PORTAL.Repositorios.Contexto;

namespace COAD.PORTAL.DAO.DAOPortalCoad
{
    public class NoticiaDAO : AbstractGenericDao<noticias, NoticiasPortalDTO, string>
    {
        private coadEntities db { get; set; }

        public NoticiaDAO()
        {
            SetProfileName("portalCoad");
            //db = GetDb<COADEntities>(false);
        }

        public void NoticiasPorID(int id)
        {
            //Noticias.id = NoticiasConteudo.id_noticia
            //var dbset = GetDbSet();
            //var resultadoPesquisa = (from noticiaConteudo in db.noticias_conteudo 
            //                        join noticia in db.noticias on noticia.id equals noticiaConteudo.id 
            //    select new { ProductName = prod.Name, Category = category.Name }
            //    );
        }
    }
}
