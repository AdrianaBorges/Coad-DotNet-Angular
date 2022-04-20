using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using COAD.PORTAL.DAO.PortalCoad;
using COAD.PORTAL.Model.DTO.PortalCoad;
using COAD.PORTAL.Repositorios.Contexto;

namespace COAD.PORTAL.Service.PortalCoad
{
    public class NoticiasSRV : GenericService<noticias, NoticiasPortalDTO, int>
    {
        private NoticiaDAO _dao = new NoticiaDAO();

        public NoticiasSRV()
        {
            Dao = _dao;
        }

        public List<NoticiasPortalDTO> NoticiasEmOrdemDescendente(int pagina = 0, int itensPorPagina = 10)
        {
            return _dao.NoticiasEmOrdemDescendente(pagina, itensPorPagina);
        }

        public List<NoticiasPortalDTO> NoticiasPorTipoEmOrdemDescendente(string tipo, int pagina = 0, int itensPorPagina = 10)
        {
            return _dao.NoticiasPorTipoEmOrdemDescendente(tipo, pagina, itensPorPagina);
        }

        public NoticiasPortalDTO UltimaNoticia()
        {
            return _dao.UltimaNoticia();
        }

        public List<NoticiasPortalDTO> BuscarNoticiasEmLotePorIDs(List<int> meus_ids)
        {
            return _dao.BuscarNoticiasEmLotePorIDs(meus_ids);
        }

        public List<NoticiasPortalDTO> BuscaNoticiasPalavrasChaveVerbeteTexto(string texto,  int pagina = 0, int itensPorPagina = 10)
        {
            return _dao.BuscaNoticiasPalavrasChaveVerbeteTexto(texto, pagina, itensPorPagina);
        }

        public List<NoticiasPortalDTO> BuscaNoticiasPalavrasChaveTexto(string palavra, int pagina = 0, int itensPorPagina = 10)
        {
            return _dao.BuscaNoticiasPalavrasChaveTexto(palavra, pagina, itensPorPagina);
        }

        public List<NoticiasPortalDTO> BuscaNoticiasPalavrasChaveVerbete(string palavra, int pagina = 0, int itensPorPagina = 10)
        {
            return _dao.BuscaNoticiasPalavrasChaveVerbete(palavra, pagina, itensPorPagina);
        }
    }
}
