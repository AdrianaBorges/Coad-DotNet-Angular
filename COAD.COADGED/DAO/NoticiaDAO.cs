using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace COAD.COADGED.DAO
{
    public class NoticiaDAO : AbstractGenericDao<NOTICIA, NoticiaDTO, int>
    {
                public COADGEDEntities db { get { return GetDb<COADGEDEntities>(); } set { } } 
        public NoticiaDAO()
            : base()
        {
            SetProfileName( "GED" );
            db = GetDb<COADGEDEntities>(false);
           
        }
        private IQueryable<NOTICIA> Listar(string _manchete, int? _grandegrupo, int? _class)
        {
            var  query = (from n in db.NOTICIA
                          where ((_manchete == null || _manchete == "") || (n.NOT_MANCHETE.Contains(_manchete))) &&
                                ((_class == null || _class == 0) || (n.NGR_ID == _class)) &&
                                ((_grandegrupo == null || _grandegrupo == 0) || (n.TIT_ID == _grandegrupo))
                         select n);                 

            return query;
        }
        public Pagina<NoticiaDTO> ListarNoticias(string _manchete, int? _grandegrupo, int? _class, int _pagina = 1, int _registroPorPagina = 20)
        {
            IQueryable<NOTICIA> query = this.Listar(_manchete, _grandegrupo, _class);

            return ToDTOPage(query, _pagina, _registroPorPagina);

        }

        public Pagina<NoticiaDTO> TrazerNoticiasPorQuantidadeEArea(int _registroPorPagina = 20, int _area = 1)
        {
            LINHA_PRODUTO_REF lpr = new LINHA_PRODUTO_REF();
            lpr.LIN_PRO_ID = _area;
            //var query = (from n in db.NOTICIA
            //             where (n.NOT_DESTAQUE_HOME == true /*&& n.NOTICIA_GRUPO.LINHA_PRODUTO_REF.Contains(lpr)*/ &&
            //             n.DATA_PUBLICACAO != null)
            //             select n);

            var _usulogado = (from b in db.NOTICIA
                              join s in db.NOTICIA_GRUPO on b.NGR_ID equals s.NGR_ID
                              join c in db.NOTICIA_GRUPO_LINHA_PROD on b.NGR_ID equals c.NGR_ID
                              where c.LIN_PRO_ID == _area
                              select b).ToList();

            return ToDTOPage(_usulogado, 1, _registroPorPagina);
        }
        
    }
}
