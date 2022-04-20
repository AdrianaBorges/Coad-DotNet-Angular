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
    public class TabDinamicaConfigItemDAO : AbstractGenericDao<TAB_DINAMICA_CONFIG_ITEM, TabDinamicaConfigItemDTO, object>
    {
        public COADGEDEntities db { get { return GetDb<COADGEDEntities>(); } set { } } 
        public TabDinamicaConfigItemDAO() : base()
        {
            SetProfileName( "GED" );
            db = GetDb<COADGEDEntities>(false);
        }
        private IQueryable<TAB_DINAMICA_CONFIG_ITEM> Listar(string _tdc_id = null, string _tci_nome_campo = null, string _tci_nome_campodb = null)
        {
            IQueryable<TAB_DINAMICA_CONFIG_ITEM> query = null;

            if (_tdc_id != null)
            {
                query = db.TAB_DINAMICA_CONFIG_ITEM.Where(x => x.TDC_ID == _tdc_id);
            }
            if (_tci_nome_campo != null)
            {
                query = query.Where(x => x.TCI_NOME_CAMPO.Contains(_tci_nome_campo));
            }
            if (_tci_nome_campodb != null)
            {
                query = query.Where(x => x.TCI_NOME_CAMPODB.Contains(_tci_nome_campodb));
            }

            return query;
        }
        public IList<TabDinamicaConfigItemDTO> ListarTabDinamica(string _tdc_id = null, string _tci_nome_campo = null, string _tci_nome_campodb = null)
        {
            IQueryable<TAB_DINAMICA_CONFIG_ITEM> query = this.Listar(_tdc_id, _tci_nome_campo, _tci_nome_campodb);

            return ToDTO(query);
        }
        public Pagina<TabDinamicaConfigItemDTO> ListarTabDinamicaPag(string _tdc_id = null, string _tci_nome_campo = null, string _tci_nome_campodb = null,int pagina = 1, int registroPorPagina = 7)
        {
            IQueryable<TAB_DINAMICA_CONFIG_ITEM> query = this.Listar(_tdc_id, _tci_nome_campo, _tci_nome_campodb);

            return ToDTOPage(query, pagina, registroPorPagina);
        } 

    }
}
