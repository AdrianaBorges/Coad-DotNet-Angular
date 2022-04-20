using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using COAD.COADGED.DAO;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Model.DTO.Custons;
using COAD.COADGED.Repositorios.Contexto;
using COAD.SEGURANCA.Repositorios.Base;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace COAD.COADGED.Service
{

    public class TabDinamicaSRV : GenericService<TAB_DINAMICA, TabDinamicaDTO, string>
    {
        private TabDinamicaDAO _dao = new TabDinamicaDAO();

        public TabDinamicaSRV()
        {
            Dao = _dao;
        }
        
        public IList<TabDinamicaDTO> ListarTabelas(string _tab_descricao = null)
        {
            return _dao.ListarTabelas();
        }
        public IList<TabDinamicaDTO> ListarSimulador(string _tab_descricao = null)
        {
            return _dao.ListarSimulador();
        }

        public void BuscarUltimosSimuladoresTrabalhistasPorData(DateTime dataParametro)
        {
            throw new NotImplementedException();
        }

        public IList<TabDinamicaDTO> ListarTabDinamica(string _tdc_id = null, string _tab_descricao = null)
        {
            return _dao.ListarTabDinamica(_tdc_id, _tab_descricao);
        }
        public Pagina<TabDinamicaDTO> ListarTabDinamicaPag(string _tdc_id = null, string _tab_descricao = null, int pagina = 1, int registroPorPagina = 20)
        {
            return _dao.ListarTabDinamicaPag(_tdc_id, _tab_descricao, pagina, registroPorPagina);
        }
        public IList<TabelaArvoreDTO> ListarTabelaTipi()
        {
            return _dao.ListarTabelaTipi();
        }
        public IList<TabelaArvoreDTO> ListarTabelaTipi(string _ncm = null, string _cest = null)
        {
            return _dao.ListarTabelaTipi(_ncm, _cest);
        }
        public IList<TabelaArvoreDTO> ListarTabelaCest()
        {
            return _dao.ListarTabelaCest();
        }
        public IList<TabelaArvoreDTO> ListarTabelaCest(string _ncm = null, string _cest = null)
        {
            return _dao.ListarTabelaCest(_ncm, _cest);
        } 

    }
}
