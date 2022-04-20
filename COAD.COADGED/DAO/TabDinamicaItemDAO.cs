using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Model.DTO.Custons;
using COAD.COADGED.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.SqlClient;
using System.Reflection;

namespace COAD.COADGED.DAO
{

    public class TabDinamicaItemDAO : AbstractGenericDao<TAB_DINAMICA_ITEM, TabDinamicaItemDTO, object>
    {
        public COADGEDEntities db { get { return GetDb<COADGEDEntities>(); } set { } } 
        public TabDinamicaItemDAO()
            : base()
        {
            SetProfileName("GED");
            db = GetDb<COADGEDEntities>(false);
        }
        private IQueryable<TAB_DINAMICA_ITEM> Listar(string _tdc_id = null, List<ParamConsultaDTO> p = null, Boolean palavrachave = false)
        {

            IQueryable<TAB_DINAMICA_ITEM> query = null;
            string _parametros = "";
    
            if (p != null && p.Count >0)
            {
                string[] _valor = new string[p.Count()];
           
                for(int i=0; i<=p.Count()-1; i++)
                {
                    if (_parametros != null && _parametros != "")
                        if (!palavrachave)
                            _parametros += " AND ";
                        else
                            _parametros += " OR ";

                    _parametros += p[i].campo + ".Contains(@"+i.ToString()+")";
                    _valor[i] = p[i].valor;
                    //_parametros += item.campo + ".Contains('" + item.valor + "')";
                }
                //--------------

                query = db.TAB_DINAMICA_ITEM.Where(_parametros, _valor);
                
                query = query.Where(x => x.TDC_ID == _tdc_id);

            }
            else
            {
                //---------------
                if (_tdc_id != null)
                {
                    query = db.TAB_DINAMICA_ITEM.Where(x => x.TDC_ID == _tdc_id);
                }
            }

            return query;
        }
        public IQueryable<TAB_DINAMICA_ITEM> BuscarItem(string _tdc_id = null, List<ParamConsultaDTO> p = null)
        {

            IQueryable<TAB_DINAMICA_ITEM> query = null;
            string _parametros = "";

            if (p != null && p.Count > 0)
            {
                string[] _valor = new string[p.Count()];

                for (int i = 0; i <= p.Count() - 1; i++)
                {
                    if (_parametros != null && _parametros != "")
                        _parametros += " AND ";
                        
                    _parametros += p[i].campo + " == @" + i.ToString() ;
                    _valor[i] = p[i].valor;
                    //_parametros += item.campo + ".Contains('" + item.valor + "')";
                }
                //--------------

                query = db.TAB_DINAMICA_ITEM.Where(_parametros, _valor);

                query = query.Where(x => x.TDC_ID == _tdc_id);

            }
            else
            {
                //---------------
                if (_tdc_id != null)
                {
                    query = db.TAB_DINAMICA_ITEM.Where(x => x.TDC_ID == _tdc_id);
                }
            }

            return query;
        }
        public IList<TabDinamicaItemDTO> ListarTabDinamica(string _tdc_id = null, List<ParamConsultaDTO> p = null)
        {
            IQueryable<TAB_DINAMICA_ITEM> query = this.Listar(_tdc_id, p);

            return ToDTO(query);
        }
        public Pagina<TabDinamicaItemDTO> ListarTabDinamicaPag(string _tdc_id = null, int pagina = 1, int registroPorPagina = 20, List<ParamConsultaDTO> p = null, Boolean palavrachave = false)
        {
            IQueryable<TAB_DINAMICA_ITEM> query = this.Listar(_tdc_id, p, palavrachave);

            return ToDTOPage(query, pagina, registroPorPagina);
        }
        public Pagina<TabDinamicaItemDTO> ListarTabDinamicaItemUF(string _tdc_id = null, string _uf = null, int pagina = 1, int registroPorPagina = 20 )
        {
            var query =  db.TAB_DINAMICA_ITEM.Where(x => x.TDC_ID == _tdc_id && x.TAB_STRING01.Contains(_uf));
  
            return ToDTOPage(query, pagina, registroPorPagina);
        }

    }
}
