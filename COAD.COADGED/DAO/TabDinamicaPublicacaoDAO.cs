using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.DAO
{
    public class TabDinamicaPublicacaoDAO : AbstractGenericDao<TAB_DINAMICA_PUBLICACAO, TabDinamicaPublicacaoDTO, int>
    {
                public COADGEDEntities db { get { return GetDb<COADGEDEntities>(); } set { } } 
        public TabDinamicaPublicacaoDAO() : base()
        {
            SetProfileName( "GED" );
            db = GetDb<COADGEDEntities>(false);
        }
        private IQueryable<TAB_DINAMICA_PUBLICACAO> ListarPublicacao(string _tdc_id = null, DateTime? _dtini = null, DateTime? _dtfim = null)
        {
            IQueryable<TAB_DINAMICA_PUBLICACAO> query = null;

            if (_tdc_id != null)
                query = db.TAB_DINAMICA_PUBLICACAO.Where(x => x.TDC_ID == _tdc_id);

            if (_dtini != null)
                if (_tdc_id != null)
                    query = query.Where(x => x.TPU_DATA_APROV >= _dtini && x.TPU_DATA_APROV < _dtfim);
                else
                    query = db.TAB_DINAMICA_PUBLICACAO.Where(x => x.TPU_DATA_APROV >= _dtini && x.TPU_DATA_APROV < _dtfim);

               
            return query;
        }
        private IQueryable<TAB_DINAMICA_PUBLICACAO> ListarAprovacao(string _tdc_id = null, DateTime? _dtini = null, DateTime? _dtfim = null)
        {
            IQueryable<TAB_DINAMICA_PUBLICACAO> query = null;

            if (_tdc_id != null)
                query = db.TAB_DINAMICA_PUBLICACAO.Where(x => x.TDC_ID == _tdc_id);

            if (_dtini != null)
                if (_tdc_id != null)
                    query = query.Where(x => x.TPU_DATA_APROV >= _dtini && x.TPU_DATA_APROV < _dtfim);
                else
                    query = db.TAB_DINAMICA_PUBLICACAO.Where(x => x.TPU_DATA_APROV >= _dtini && x.TPU_DATA_APROV < _dtfim);

            return query;
        }
        public IList<TabDinamicaPublicacaoDTO> ListarPorPublicacao(string _tdc_id = null, DateTime? _dtini = null, DateTime? _dtfim = null )
        {
            IQueryable<TAB_DINAMICA_PUBLICACAO> query = this.ListarPublicacao(_tdc_id, _dtini, _dtfim);

            return ToDTO(query);
        }
        public Pagina<TabDinamicaPublicacaoDTO> ListarPorPublicacao(string _tdc_id = null, DateTime? _dtini = null, DateTime? _dtfim = null, int pagina = 1, int registroPorPagina = 20)
        {
            IQueryable<TAB_DINAMICA_PUBLICACAO> query = this.ListarPublicacao(_tdc_id, _dtini, _dtfim);

            return ToDTOPage(query, pagina, registroPorPagina);
        }
        public IList<TabDinamicaPublicacaoDTO> ListarPorAprovacao(string _tdc_id = null, DateTime? _dtini = null, DateTime? _dtfim = null)
        {
            IQueryable<TAB_DINAMICA_PUBLICACAO> query = this.ListarAprovacao(_tdc_id, _dtini, _dtfim);

            return ToDTO(query);
        }
        public Pagina<TabDinamicaPublicacaoDTO> ListarPorAprovacao(string _tdc_id = null, DateTime? _dtini = null, DateTime? _dtfim = null, int pagina = 1, int registroPorPagina = 20)
        {
            IQueryable<TAB_DINAMICA_PUBLICACAO> query = this.ListarAprovacao(_tdc_id, _dtini, _dtfim);

            return ToDTOPage(query, pagina, registroPorPagina);
        }
        public TabDinamicaPublicacaoDTO FindLastPubReg(string _tdc_id)
        {
            TAB_DINAMICA_PUBLICACAO query = null;

            query = db.TAB_DINAMICA_PUBLICACAO.Where(x => x.TDC_ID == _tdc_id && x.TPU_DATA_APROV == null).FirstOrDefault();

            return ToDTO(query);
        }
    }
}
