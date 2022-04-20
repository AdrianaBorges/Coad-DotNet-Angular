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
    public class NoticiaDAO : AbstractGenericDao<noticias, NoticiasPortalDTO, int>
    {
        private coadEntities db { get; set; }

        public NoticiaDAO()
        {
            SetProfileName("portalCoad");
            //db = GetDb<COADEntities>(false);
        }
        
        public List<NoticiasPortalDTO> NoticiasEmOrdemDescendente(int pagina = 0, int itensPorPagina = 10)
        {
            //List<noticias> query = GetDbSet().SqlQuery("SELECT noticias.* FROM noticias INNER JOIN noticias_conteudo AS nc ON noticias.id = nc.id_noticia WHERE publicar = 's' AND id_prod <> 19 GROUP BY nc.verbete ORDER BY id_noticia DESC LIMIT " + pagina + "," + itensPorPagina + "").ToList();
            List<noticias> query = GetDbSet().SqlQuery("SELECT noticias.* FROM noticias INNER JOIN noticias_conteudo AS nc ON noticias.id = nc.id_noticia WHERE publicar = 's' AND id_prod = 18 ORDER BY id_noticia DESC LIMIT " + pagina + "," + itensPorPagina + "").ToList();
            List<NoticiasPortalDTO> noticiasDTO = new List<NoticiasPortalDTO>();
            if (query != null)
            {
                foreach (var noticia in query)
                {
                    noticiasDTO.Add(ToDTO(noticia));
                }
            }
            return noticiasDTO;
        }

        public List<NoticiasPortalDTO> NoticiasPorTipoEmOrdemDescendente(string tipo, int pagina = 0, int itensPorPagina = 10)
        {
            List<noticias> query;

            if(int.Parse(tipo) == 18)
                query = GetDbSet().SqlQuery("SELECT * FROM noticias WHERE id_prod = '18' AND id_tipo <> '23' AND publicar = 's' ORDER BY data_cadastro DESC LIMIT " + pagina + "," + itensPorPagina + "").ToList();
            else
                query = GetDbSet().SqlQuery("SELECT * FROM noticias WHERE id_prod = '18' AND id_tipo = '23' AND publicar = 's' ORDER BY data_cadastro DESC LIMIT " + pagina + "," + itensPorPagina + "").ToList();

            List<NoticiasPortalDTO> noticiasDTO = new List<NoticiasPortalDTO>();
            if (query != null)
            {
                foreach (var noticia in query)
                {
                    noticiasDTO.Add(ToDTO(noticia));
                }
            }
            return noticiasDTO;
        }

        public NoticiasPortalDTO UltimaNoticia()
        {
            noticias query = GetDbSet().OrderByDescending(x => x.data_cadastro).Where(x => x.publicar == "s" && x.id_prod == 18).FirstOrDefault();
            return ToDTO(query);
        }

        public List<NoticiasPortalDTO> BuscarNoticiasEmLotePorIDs(List<int> meus_ids)
        {
            string clausula_ids = "";
            bool primeiro_id = true;
            foreach(int id in meus_ids){
                if (primeiro_id)
                {
                    clausula_ids += id + "' ";
                    primeiro_id = false;
                }
                else
                    clausula_ids += "OR id = '" + id + "' ";
            }

            List<noticias> query = GetDbSet().SqlQuery("SELECT noticias.* FROM noticias WHERE id = '" + clausula_ids + "AND publicar = 's' ORDER BY data_cadastro").ToList();

            List<NoticiasPortalDTO> noticiasDTO = new List<NoticiasPortalDTO>();
            if (query != null)
            {
                foreach (var noticia in query)
                {
                    noticiasDTO.Add(ToDTO(noticia));
                }
            }
            return noticiasDTO;
        }
        
        public List<NoticiasPortalDTO> BuscaNoticiasPalavrasChaveVerbeteTexto(string texto, int pagina = 0, int itensPorPagina = 10)
        {
            List<noticias> query = GetDbSet().SqlQuery("SELECT noticias.* FROM noticias INNER JOIN noticias_busca AS nb ON noticias.id = nb.id_noticia WHERE (nb.verbete LIKE '%" + texto + "%' OR nb.texto LIKE '%" + texto + "%') AND nb.publicar = 's' AND nb.id_prod = 18 GROUP BY nb.verbete ORDER BY id_noticia DESC LIMIT " + pagina + "," + itensPorPagina + "").ToList();
            List<NoticiasPortalDTO> noticiasDTO = new List<NoticiasPortalDTO>();
            if (query != null)
            {
                foreach (var noticia in query)
                {
                    noticiasDTO.Add(ToDTO(noticia));
                }
            }
            return noticiasDTO;
        }

        public List<NoticiasPortalDTO> BuscaNoticiasPalavrasChaveTexto(string palavra, int pagina = 0, int itensPorPagina = 10)
        {
            List<noticias> query = GetDbSet().SqlQuery("SELECT noticias.* FROM noticias INNER JOIN noticias_busca AS nb ON noticias.id = nb.id_noticia WHERE nb.texto LIKE '%" + palavra + "%' AND nb.publicar = 's' AND nb.id_prod = 18 GROUP BY nb.verbete ORDER BY id_noticia DESC LIMIT " + pagina + "," + itensPorPagina + "").ToList();
            List<NoticiasPortalDTO> noticiasDTO = new List<NoticiasPortalDTO>();
            if (query != null)
            {
                foreach (var noticia in query)
                {
                    noticiasDTO.Add(ToDTO(noticia));
                }
            }
            return noticiasDTO;
        }

        public List<NoticiasPortalDTO> BuscaNoticiasPalavrasChaveVerbete(string palavra, int pagina = 0, int itensPorPagina = 10)
        {
            List<noticias> query = GetDbSet().SqlQuery("SELECT noticias.* FROM noticias INNER JOIN noticias_busca AS nb ON noticias.id = nb.id_noticia WHERE nb.verbete LIKE '%" + palavra + "%' AND nb.publicar = 's' AND nb.id_prod = 18 GROUP BY nb.verbete ORDER BY id_noticia DESC LIMIT " + pagina + "," + itensPorPagina + "").ToList();
            List<NoticiasPortalDTO> noticiasDTO = new List<NoticiasPortalDTO>();
            if (query != null)
            {
                foreach (var noticia in query)
                {
                    noticiasDTO.Add(ToDTO(noticia));
                }
            }
            return noticiasDTO;
        }
    }
}
