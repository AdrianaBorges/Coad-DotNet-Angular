using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using COAD.COADGED.DAO;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions; 

namespace COAD.COADGED.Service
{
    public class NoticiaSRV : GenericService<NOTICIA, NoticiaDTO, int>
    {
        private NoticiaDAO _dao = new NoticiaDAO();

        public NoticiaSRV()
        {
            Dao = _dao;
        }

        //public string TrazerNoticiasPorQuantidadeEArea(int qte, int area)
        //{
        //    return null;
        //}


        public Pagina<NoticiaDTO> ListarNoticias(string _manchete, int? _grandegrupo, int? _class, int _pagina = 1, int _registroPorPagina = 20)
        {
            return _dao.ListarNoticias(_manchete, _grandegrupo, _class, _pagina, _registroPorPagina);
        }
        public void Publicar(NoticiaDTO _noticia)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                _noticia.USU_LOGIN_PUB = SessionContext.autenticado.USU_LOGIN;
                _noticia.DATA_PUBLICACAO = DateTime.Now;
                this.Merge(_noticia, "NOT_ID");
                scope.Complete();
            }
        }
        public void RemoverPublicacao(NoticiaDTO _noticia)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                _noticia.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;
                _noticia.DATA_ALTERA = DateTime.Now;
                _noticia.USU_LOGIN_PUB = null;
                _noticia.DATA_PUBLICACAO = null;
                this.Merge(_noticia, "NOT_ID");
                scope.Complete();
            }
        }
        public void SalvarNoticiaMateria(NoticiaDTO _noticia)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                _noticia.DATA_CADASTRO = DateTime.Now;
                _noticia.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;
                _noticia.USU_LOGIN_PUB = null;
                _noticia.DATA_PUBLICACAO = null;

                this.Save(_noticia);

                scope.Complete();
            }
        }
        
		public Pagina<NoticiaDTO> TrazerNoticiasPorQuantidadeEArea(int _qte, int _area)
        {
            return _dao.TrazerNoticiasPorQuantidadeEArea(_qte, _area);
        }
        
        
    }
}
