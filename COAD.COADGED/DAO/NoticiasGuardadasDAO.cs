using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;

namespace COAD.COADGED.DAO
{
    public class NoticiasGuardadasDAO : AbstractGenericDao<NOTICIAS_GUARDADAS, NoticiasGuardadasDTO, int>
    {
                public COADGEDEntities db { get { return GetDb<COADGEDEntities>(); } set { } } 
        public NoticiasGuardadasDAO()
            : base()
        {
            SetProfileName( "GED" );
            db = GetDb<COADGEDEntities>(false);
        }

        public IList<NoticiasGuardadasDTO> BuscarNoticiasSalvas(int idCliente)
        {
            IQueryable<NOTICIAS_GUARDADAS> query = GetDbSet();

            query = query.Where(x => x.ID_CLIENTE == idCliente);

            //query = query.Where(p => p.DATA_EXCLUSAO == null); ALT: 23/06/2015 - o campo de exclusão lógica não foi criado...

            return ToDTO(query);
        }

        internal NoticiasGuardadasDTO BuscarNoticiaPorIdClienteIdNoticia(int idCliente, int idNoticia)
        {
            NOTICIAS_GUARDADAS query = GetDbSet().Where(x => x.ID_CLIENTE == idCliente && x.ID_NOTICIA == idNoticia).FirstOrDefault();

            return ToDTO(query);
        }

        internal void ExcluirNoticiaPorIdClienteIdNoticia(int idCliente, int idNoticia)
        {
            using (var ctx = new COADGEDEntities())
            {
                var x = (from y in ctx.NOTICIAS_GUARDADAS
                         where y.ID_CLIENTE.Equals(idCliente) && y.ID_NOTICIA.Equals(idNoticia) select y).FirstOrDefault();
                ctx.NOTICIAS_GUARDADAS.Remove(x);
                ctx.SaveChanges();
            }
        }
    }
}
