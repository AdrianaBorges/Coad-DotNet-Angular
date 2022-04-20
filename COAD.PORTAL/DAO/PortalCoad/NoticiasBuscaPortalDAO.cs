using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Dao.Base;
using COAD.PORTAL.Model.DTO.PortalCoad;
using COAD.PORTAL.Repositorios.Contexto;

namespace COAD.PORTAL.DAO.PortalCoad
{
    public class NoticiasBuscaPortalDAO : AbstractGenericDao<noticias_busca, NoticiasPortalBuscaDTO, int>
    {
        private coadEntities db { get; set; }

        public NoticiasBuscaPortalDAO()
        {
            SetProfileName("portalCoad");
        }


        public NoticiasPortalBuscaDTO BuscarPorIdNoticia(int identificador)
        {
            noticias_busca query = GetDbSet().Where(x => x.id_noticia == identificador).FirstOrDefault();
            return ToDTO(query);
        }

        public List<NoticiasPortalBuscaDTO> NoticiasEmOrdemDescendente(int pagina = 0, int itensPorPagina = 10)
        {
            List<noticias_busca> query = GetDbSet().SqlQuery("SELECT * FROM noticias_busca WHERE publicar = 's' AND id_prod = 18 ORDER BY id_noticia DESC LIMIT " + pagina + "," + itensPorPagina + "").ToList();
            List<NoticiasPortalBuscaDTO> noticiasDTO = new List<NoticiasPortalBuscaDTO>();
            if (query != null)
            {
                foreach (var noticia in query)
                {
                    noticiasDTO.Add(ToDTO(noticia));
                }
            }
            return noticiasDTO;
        }

        public List<NoticiasPortalBuscaDTO> NoticiasPorTipoEmOrdemDescendente(string tipo, int pagina = 0, int itensPorPagina = 10)
        {
            List<noticias_busca> query;

            if (int.Parse(tipo) == 18)
                query = GetDbSet().SqlQuery("SELECT * FROM noticias_busca WHERE id_prod = '18' AND id_tipo <> '23' AND publicar = 's' ORDER BY data_cadastro DESC LIMIT " + pagina + "," + itensPorPagina + "").ToList();
            else
                query = GetDbSet().SqlQuery("SELECT * FROM noticias_busca WHERE id_prod = '18' AND id_tipo = '23' AND publicar = 's' ORDER BY data_cadastro DESC LIMIT " + pagina + "," + itensPorPagina + "").ToList();

            List<NoticiasPortalBuscaDTO> noticiasDTO = new List<NoticiasPortalBuscaDTO>();
            if (query != null)
            {
                foreach (var noticia in query)
                {
                    noticiasDTO.Add(ToDTO(noticia));
                }
            }
            return noticiasDTO;
        }

        public NoticiasPortalBuscaDTO UltimaNoticia()
        {
            noticias_busca query = GetDbSet().OrderByDescending(x => x.data_cadastro).Where(x => x.publicar == "s" && x.id_prod == 18).FirstOrDefault();
            return ToDTO(query);
        }

        public List<NoticiasPortalBuscaDTO> BuscarNoticiasEmLotePorIDs(List<int> meus_ids)
        {
            string clausula_ids = "";
            bool primeiro_id = true;
            foreach (int id in meus_ids)
            {
                if (primeiro_id)
                {
                    clausula_ids += id + "' ";
                    primeiro_id = false;
                }
                else
                    clausula_ids += "OR id_noticia = '" + id + "' ";
            }

            List<noticias_busca> query = GetDbSet().SqlQuery("SELECT * FROM noticias_busca WHERE id_noticia = '" + clausula_ids + "AND publicar = 's' ORDER BY data_cadastro").ToList();

            List<NoticiasPortalBuscaDTO> noticiasDTO = new List<NoticiasPortalBuscaDTO>();
            if (query != null)
            {
                foreach (var noticia in query)
                {
                    noticiasDTO.Add(ToDTO(noticia));
                }
            }
            return noticiasDTO;
        }

        public List<NoticiasPortalBuscaDTO> BuscaNoticiasPalavrasChaveVerbeteTexto(string texto, int pagina = 0, int itensPorPagina = 10)
        {
            List<noticias_busca> query = GetDbSet().SqlQuery("SELECT * FROM noticias_busca WHERE (verbete LIKE '%" + texto + "%' OR texto LIKE '%" + texto + "%') AND publicar = 's' AND id_prod = 18 GROUP BY verbete ORDER BY id_noticia DESC LIMIT " + pagina + "," + itensPorPagina + "").ToList();
            List<NoticiasPortalBuscaDTO> noticiasDTO = new List<NoticiasPortalBuscaDTO>();
            if (query != null)
            {
                foreach (var noticia in query)
                {
                    noticiasDTO.Add(ToDTO(noticia));
                }
            }
            return noticiasDTO;
        }

        public List<NoticiasPortalBuscaDTO> BuscaNoticiasPalavrasChaveTexto(string palavra, int pagina = 0, int itensPorPagina = 10)
        {
            List<noticias_busca> query = GetDbSet().SqlQuery("SELECT * FROM noticias_busca WHERE texto LIKE '%" + palavra + "%' AND publicar = 's' AND id_prod = 18 GROUP BY verbete ORDER BY id_noticia DESC LIMIT " + pagina + "," + itensPorPagina + "").ToList();
            List<NoticiasPortalBuscaDTO> noticiasDTO = new List<NoticiasPortalBuscaDTO>();
            if (query != null)
            {
                foreach (var noticia in query)
                {
                    noticiasDTO.Add(ToDTO(noticia));
                }
            }
            return noticiasDTO;
        }

        public List<NoticiasPortalBuscaDTO> BuscaNoticiasPalavrasChaveVerbete(string palavra, int pagina = 0, int itensPorPagina = 10)
        {
            List<noticias_busca> query = GetDbSet().SqlQuery("SELECT * FROM noticias_busca WHERE verbete LIKE '%" + palavra + "%' AND publicar = 's' AND id_prod = 18 GROUP BY verbete ORDER BY id_noticia DESC LIMIT " + pagina + "," + itensPorPagina + "").ToList();
            List<NoticiasPortalBuscaDTO> noticiasDTO = new List<NoticiasPortalBuscaDTO>();
            if (query != null)
            {
                foreach (var noticia in query)
                {
                    noticiasDTO.Add(ToDTO(noticia));
                }
            }
            return noticiasDTO;
        }

        public List<NoticiasPortalBuscaDTO> BuscaNoticiasPorGrupo(string id_grupo, int pagina = 0, int itensPorPagina = 10)
        {
            List<noticias_busca> query = GetDbSet().SqlQuery("SELECT * FROM noticias_busca WHERE id_grupo = '" + id_grupo + "' AND publicar = 's' AND id_prod = 18 ORDER BY id_noticia DESC LIMIT " + pagina + "," + itensPorPagina + "").ToList();
            List<NoticiasPortalBuscaDTO> noticiasDTO = new List<NoticiasPortalBuscaDTO>();
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
