using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Model.DTO.Custons;
using COAD.COADGED.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.DAO
{
    public class TabDinamicaDAO : AbstractGenericDao<TAB_DINAMICA, TabDinamicaDTO, string>
    {
                public COADGEDEntities db { get { return GetDb<COADGEDEntities>(); } set { } } 
        public TabDinamicaDAO() : base()
        {
            SetProfileName( "GED" );

            db = GetDb<COADGEDEntities>(false);
        }

        public IList<TabDinamicaDTO> ListarTabelas(string _tab_descricao = null)
        {
            IQueryable<TAB_DINAMICA> query = (from a in db.TAB_DINAMICA
                                              join c in db.TAB_DINAMICA_CONFIG on a.TDC_ID equals c.TDC_ID
                                              where (c.TDC_TIPO == 1)
                                              select a).AsQueryable();   


            return ToDTO(query);
        }
        public IList<TabDinamicaDTO> ListarSimulador(string _tab_descricao = null)
        {
            IQueryable<TAB_DINAMICA> query = (from a in db.TAB_DINAMICA
                                              join c in db.TAB_DINAMICA_CONFIG on a.TDC_ID equals c.TDC_ID
                                              where (c.TDC_TIPO == 2)
                                              select a).AsQueryable();


            return ToDTO(query);
        }
        private IQueryable<TAB_DINAMICA> Listar(string _tdc_id = null, string _tab_descricao = null)
        {
            IQueryable<TAB_DINAMICA> query = null;

            if (_tdc_id != null)
            {
                query = db.TAB_DINAMICA.Where(x => x.TDC_ID == _tdc_id).OrderBy(x => x.TAB_DATA_ALTERA).OrderBy(x => x.TAB_DATA_INCLUSAO);
            }
            else
            {
                query = db.TAB_DINAMICA.Where(x => x.TAB_DESCRICAO.Contains(_tab_descricao)).OrderBy(x => x.TAB_DATA_ALTERA).OrderBy(x => x.TAB_DATA_INCLUSAO);
            }
           
            return query;
        }
        public IList<TabDinamicaDTO> ListarTabDinamica(string _tdc_id = null, string _tab_descricao = null)
        {
            IQueryable<TAB_DINAMICA> query = this.Listar(_tdc_id, _tab_descricao); 

            return ToDTO(query);
        }
        public Pagina<TabDinamicaDTO> ListarTabDinamicaPag(string _tdc_id = null, string _tab_descricao = null, int pagina = 1, int registroPorPagina = 20)
        {
            IQueryable<TAB_DINAMICA> query = this.Listar(_tdc_id, _tab_descricao);

            return ToDTOPage(query, pagina, registroPorPagina);
        } 

        public IList<TabelaArvoreDTO> ListarTabelaTipi()
        {
            List<TabelaArvoreDTO> _lista = new  List<TabelaArvoreDTO>();

            List<decimal?> selecionados = new List<decimal?>();

            var query = (from tp in db.TABELA_TIPI_VW
                         join tp2 in db.TABELA_TIPI_VW on tp.id_item_tabela_tipi_pai equals tp2.id
                         where tp2.id_item_tabela_tipi_pai == null
                         select tp).AsQueryable();

            foreach (var item in query)
            {

                TabelaArvoreDTO nivel0 = new TabelaArvoreDTO();
                nivel0.ID = item.id;
                nivel0.CODIGO = item.ncm;
                nivel0.DESCRICAO = item.descricao;
                nivel0.ID_NODE = item.id_item_tabela_tipi_pai;
                nivel0.CODAUX = item.codigo_cest;

                _lista.Add(nivel0);
            }


            query = (from tp in db.TABELA_TIPI_VW
                         where tp.id_item_tabela_tipi_pai != null
                         select tp).OrderBy(x => x.id_item_tabela_tipi_pai);

            foreach (var item in query)
            {

                if (item.id_item_tabela_tipi_pai != null)
                {
                    for (var ind = 0; ind <= _lista.Count - 1; ind++)
                    {
                        if (item.id_item_tabela_tipi_pai == _lista[ind].ID)
                        {
                            TabelaArvoreDTO nivel1 = new TabelaArvoreDTO();

                            nivel1.ID = item.id;
                            nivel1.CODIGO = item.ncm;
                            nivel1.DESCRICAO = item.descricao;
                            nivel1.ID_NODE = item.id_item_tabela_tipi_pai;
                            nivel1.CODAUX = item.codigo_cest;

                            _lista[ind].ITEM.Add(nivel1);
                        }
                        else
                        {

                            foreach (var item_nivel2 in _lista[ind].ITEM)
                            {

                                if (item.id_item_tabela_tipi_pai == item_nivel2.ID)
                                {
                                    TabelaArvoreDTO nivel2 = new TabelaArvoreDTO();

                                    nivel2.ID = item.id;
                                    nivel2.CODIGO = item.ncm;
                                    nivel2.DESCRICAO = item.descricao;
                                    nivel2.ID_NODE = item.id_item_tabela_tipi_pai;
                                    nivel2.CODAUX = item.codigo_cest;

                                    item_nivel2.ITEM.Add(nivel2);
                                }
                            }
                        }

                    }
                }
                
            }


            return _lista;
        }
        public IList<TabelaArvoreDTO> ListarTabelaTipi(string _ncm = null, string _cest = null)
        {

            List<TabelaArvoreDTO> _lista = new List<TabelaArvoreDTO>();

            List<TABELA_TIPI_VW> query = (from tp in db.TABELA_TIPI_VW
                                          where (_ncm == null || (_ncm != null && _ncm == tp.ncm))
                                          select tp).ToList();

            foreach (var item in query)
            {
                TabelaArvoreDTO nivel0 = new TabelaArvoreDTO();
                nivel0.ID = item.id;
                nivel0.CODIGO = item.ncm;
                nivel0.DESCRICAO = item.descricao;
                nivel0.ID_NODE = item.id_item_tabela_tipi_pai;
                nivel0.CODAUX = item.codigo_cest;
                
                _lista.Add(nivel0);

            }

            return _lista;
        }
        public IList<TabelaArvoreDTO> ListarTabelaCest()
        {
            List<TabelaArvoreDTO> _lista = new  List<TabelaArvoreDTO>();

            var query = (from tp in db.TABELA_CEST_VW
                         join tp2 in db.TABELA_CEST_VW on tp.id_item_cest_pai equals tp2.id
                        where tp2.id_item_cest_pai == null
                       select tp).AsQueryable();

            foreach (var item in query)
            {

                TabelaArvoreDTO nivel0 = new TabelaArvoreDTO();
                nivel0.ID = item.id;
                nivel0.CODIGO = item.codigo_cest;
                nivel0.DESCRICAO = item.descricao;
                nivel0.ID_NODE = item.id_item_cest_pai;
                nivel0.CODAUX = item.ncm;

                _lista.Add(nivel0);
            }


            query = (from tp in db.TABELA_CEST_VW
                     where tp.id_item_cest_pai != null
                    select tp).OrderBy(x => x.id_item_cest_pai);

            foreach (var item in query)
            {

                if (item.id_item_cest_pai != null)
                {
                    for (var ind = 0; ind <= _lista.Count - 1; ind++)
                    {
                        if (item.id_item_cest_pai == _lista[ind].ID)
                        {
                            TabelaArvoreDTO nivel1 = new TabelaArvoreDTO();

                            nivel1.ID = item.id;
                            nivel1.CODIGO = item.codigo_cest;
                            nivel1.DESCRICAO = item.descricao;
                            nivel1.ID_NODE = item.id_item_cest_pai;
                            nivel1.CODAUX = item.ncm;

                            _lista[ind].ITEM.Add(nivel1);
                        }
                        else
                        {

                            foreach (var item_nivel2 in _lista[ind].ITEM)
                            {

                                if (item.id_item_cest_pai == item_nivel2.ID)
                                {
                                    TabelaArvoreDTO nivel2 = new TabelaArvoreDTO();

                                    nivel2.ID = item.id;
                                    nivel2.CODIGO = item.codigo_cest;
                                    nivel2.DESCRICAO = item.descricao;
                                    nivel2.ID_NODE = item.id_item_cest_pai;
                                    nivel2.CODAUX = item.ncm;

                                    item_nivel2.ITEM.Add(nivel2);
                                }
                            }
                        }

                    }
                }
                
            }


            return _lista;
        }
        public IList<TabelaArvoreDTO> ListarTabelaCest(string _ncm = null, string _cest = null)
        {

            List<TabelaArvoreDTO> _lista = new List<TabelaArvoreDTO>();

            List<TABELA_CEST_VW> query = (from tp in db.TABELA_CEST_VW
                                          where (_cest == null || (_cest != null && _cest == tp.codigo_cest))
                                         select tp).ToList();

            foreach (var item in query)
            {
                TabelaArvoreDTO nivel0 = new TabelaArvoreDTO();
                nivel0.ID = item.id;
                nivel0.CODIGO = item.codigo_cest;
                nivel0.DESCRICAO = item.descricao;
                nivel0.ID_NODE = item.id_item_cest_pai;
                nivel0.CODAUX = item.ncm;
                
                _lista.Add(nivel0);

            }

            return _lista;
        } 
        

    }
}
