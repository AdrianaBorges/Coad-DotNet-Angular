using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using COAD.COADGED.DAO;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Service
{
    [ServiceConfig("TDC_ID", "TCI_ID")]
    public class TabDinamicaConfigItemSRV : GenericService<TAB_DINAMICA_CONFIG_ITEM, TabDinamicaConfigItemDTO, object>
    {
        private TabDinamicaConfigItemDAO _dao = new TabDinamicaConfigItemDAO();

        public TabDinamicaConfigItemSRV()
        {
            Dao = _dao;
        }
        public IList<TabDinamicaConfigItemDTO> ListarTabDinamica(string _tdc_id = null, string _tci_nome_campo = null, string _tci_nome_campodb = null)
        {
            return _dao.ListarTabDinamica(_tdc_id, _tci_nome_campo, _tci_nome_campodb );
        }
        public Pagina<TabDinamicaConfigItemDTO> ListarTabDinamicaPag(string _tdc_id = null, string _tci_nome_campo = null, string _tci_nome_campodb = null, int pagina = 1, int registroPorPagina = 7)
        {
            return _dao.ListarTabDinamicaPag(_tdc_id, _tci_nome_campo, _tci_nome_campodb, pagina, registroPorPagina);
        }
    }
}
