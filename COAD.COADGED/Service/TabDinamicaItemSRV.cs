using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using COAD.COADGED.DAO;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Model.DTO.Custons;
using COAD.COADGED.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Service
{
    public class TabDinamicaItemSRV : GenericService<TAB_DINAMICA_ITEM, TabDinamicaItemDTO, object>
    {
        private TabDinamicaItemDAO _dao = new TabDinamicaItemDAO();

        public TabDinamicaItemSRV()
        {
            Dao = _dao;
        }
        public IList<TabDinamicaItemDTO> ListarTabDinamica(string _tdc_id = null, string _tab_descricao = null, List<ParamConsultaDTO> p = null)
        {
            return _dao.ListarTabDinamica(_tdc_id, p);
        }
        public Pagina<TabDinamicaItemDTO> ListarTabDinamicaPag(string _tdc_id = null, int pagina = 1, int registroPorPagina = 20, List<ParamConsultaDTO> p = null, Boolean palavrachave = false)
        {
            return _dao.ListarTabDinamicaPag(_tdc_id, pagina, registroPorPagina, p, palavrachave);
        }
        public IQueryable<TAB_DINAMICA_ITEM> BuscarItem(string _tdc_id = null, List<ParamConsultaDTO> p = null)
        {
            return _dao.BuscarItem(_tdc_id, p);
        }
        public Pagina<TabDinamicaItemDTO> ListarTabDinamicaItemUF(string _tdc_id = null, string _uf = null, int pagina = 1, int registroPorPagina = 20)
        {
            return _dao.ListarTabDinamicaItemUF(_tdc_id, _uf, pagina, registroPorPagina);
        }
    }

}
