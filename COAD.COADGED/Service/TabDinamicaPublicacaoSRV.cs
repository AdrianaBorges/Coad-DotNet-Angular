using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using COAD.COADGED.DAO;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Service
{
    public class TabDinamicaPublicacaoSRV : GenericService<TAB_DINAMICA_PUBLICACAO, TabDinamicaPublicacaoDTO, int>
    {
        private TabDinamicaPublicacaoDAO _dao = new TabDinamicaPublicacaoDAO();

        public TabDinamicaPublicacaoSRV()
        {
            Dao = _dao;
        }
        public IList<TabDinamicaPublicacaoDTO> ListarPorPublicacao(string _tdc_id = null, DateTime? _dtini = null, DateTime? _dtfim = null)
        {
            return _dao.ListarPorPublicacao(_tdc_id, _dtini, _dtfim);
        }
        public Pagina<TabDinamicaPublicacaoDTO> ListarPorPublicacao(string _tdc_id = null, DateTime? _dtini = null, DateTime? _dtfim = null, int pagina = 1, int registroPorPagina = 20)
        {
            return _dao.ListarPorPublicacao(_tdc_id, _dtini, _dtfim, pagina,registroPorPagina);
        }
        public IList<TabDinamicaPublicacaoDTO> ListarPorAprovacao(string _tdc_id = null, DateTime? _dtini = null, DateTime? _dtfim = null)
        {
            return _dao.ListarPorAprovacao(_tdc_id,_dtini, _dtfim);
        }
        public Pagina<TabDinamicaPublicacaoDTO> ListarPorAprovacao(string _tdc_id = null, DateTime? _dtini = null, DateTime? _dtfim = null, int pagina = 1, int registroPorPagina = 20)
        {
            return _dao.ListarPorAprovacao(_tdc_id, _dtini, _dtfim, pagina, registroPorPagina);
        }
        public TabDinamicaPublicacaoDTO FindLastPubReg(string _tdc_id)
        {
            return _dao.FindLastPubReg(_tdc_id);
        }

    }
}
