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
    public class VW_NoticiasSRV : GenericService<VW_BuscarNoticias, VW_NoticiasDTO, int>
    {
        private VW_NoticiasDAO _dao = new VW_NoticiasDAO();

        public VW_NoticiasSRV()
        {
            Dao = _dao;
        }
                
        public List<VW_NoticiasDTO> NoticiasEmOrdemDescendente(int pagina = 0, int itensPorPagina = 10)
        {
            return _dao.NoticiasEmOrdemDescendente(pagina, itensPorPagina);
        }

        public VW_NoticiasDTO UltimaNoticia()
        {
            return _dao.UltimaNoticia();
        }

        public VW_NoticiasDTO BuscarNoticiaPorId(int id)
        {
            return _dao.BuscarNoticiaPorId(id);
        }

        public Pagina<VW_NoticiasDTO> NoticiasFiltro(string titulo, string texto, string descricao, int pagina = 1, int nLinha = 7)
        {
            return _dao.NoticiasFiltro(titulo, texto, descricao, pagina, nLinha);
        }
    }
}
