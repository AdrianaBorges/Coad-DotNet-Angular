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
    public class Noticias_BuscaPortalSRV : GenericService<noticias_busca, NoticiasPortalBuscaDTO, int>
    {
        private NoticiasBuscaPortalDAO _dao = new NoticiasBuscaPortalDAO();

        public Noticias_BuscaPortalSRV()
        {
            Dao = _dao;
        }

        public NoticiasPortalBuscaDTO BuscarPorIdNoticia(int identificador)
        {
            return _dao.BuscarPorIdNoticia(identificador);
        }

        public List<NoticiasPortalBuscaDTO> NoticiasEmOrdemDescendente(int pagina = 0, int itensPorPagina = 10)
        {
            return _dao.NoticiasEmOrdemDescendente(pagina, itensPorPagina);
        }

        public List<NoticiasPortalBuscaDTO> NoticiasPorTipoEmOrdemDescendente(string tipo, int pagina = 0, int itensPorPagina = 10)
        {
            return _dao.NoticiasPorTipoEmOrdemDescendente(tipo, pagina, itensPorPagina);
        }

        public NoticiasPortalBuscaDTO UltimaNoticia()
        {
            return _dao.UltimaNoticia();
        }

        public List<NoticiasPortalBuscaDTO> BuscarNoticiasEmLotePorIDs(List<int> meus_ids)
        {
            return _dao.BuscarNoticiasEmLotePorIDs(meus_ids);
        }

        public List<NoticiasPortalBuscaDTO> BuscaNoticiasPalavrasChaveVerbeteTexto(string texto, int pagina = 0, int itensPorPagina = 10)
        {
            return _dao.BuscaNoticiasPalavrasChaveVerbeteTexto(texto, pagina, itensPorPagina);
        }

        public List<NoticiasPortalBuscaDTO> BuscaNoticiasPalavrasChaveTexto(string palavra, int pagina = 0, int itensPorPagina = 10)
        {
            return _dao.BuscaNoticiasPalavrasChaveTexto(palavra, pagina, itensPorPagina);
        }

        public List<NoticiasPortalBuscaDTO> BuscaNoticiasPalavrasChaveVerbete(string palavra, int pagina = 0, int itensPorPagina = 10)
        {
            return _dao.BuscaNoticiasPalavrasChaveVerbete(palavra, pagina, itensPorPagina);
        }

        public List<NoticiasPortalBuscaDTO> BuscaNoticiasPorGrupo(string id_grupo, int pagina = 0, int itensPorPagina = 10)
        {
            return _dao.BuscaNoticiasPorGrupo(id_grupo, pagina, itensPorPagina);
        }
    }
}
