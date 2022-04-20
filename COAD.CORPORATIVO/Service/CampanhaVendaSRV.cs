using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Coad.GenericCrud.Service.Base;
using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto.Custons.Pesquisas;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.Model.Dto.Custons.Validacoes;
using GenericCrud.Service;
using COAD.CORPORATIVO.Model.Dto.Custons.Historicos;
using COAD.CORPORATIVO.Service.Custons;
using GenericCrud.Config.DataAttributes;
using System.Transactions;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("CVE_ID")]
    public class CampanhaVendaSRV : GenericService<CAMPANHA_VENDA, CampanhaVendaDTO, int>
    {
        private CampanhaVendaDAO _dao;
        public CampanhaVendaTipoPropostaSRV _campanhaVendaTipoProposta { get; set; }
        public TipoPagamentoCampanhaVendaSRV _tipoPagamentoCampanhaVenda { get; set; }
        public CampanhaVendasProdutoComposicaoSRV _campanhaVendasProdutoComposicaoSRV { get; set; }

        public CampanhaVendaSRV()
        {
            this._dao = new CampanhaVendaDAO();
            this.Dao = _dao;
        }

        public CampanhaVendaSRV(CampanhaVendaDAO _dao)
        {
            this._dao = _dao;
            this.Dao = _dao;
        }

        public Pagina<CampanhaVendaDTO> PesquisarCampanhaVenda(PesquisaCampanhaVendaDTO pesquisa)
        {
            var pagina = _dao.PesquisarCampanhaVenda(pesquisa);
            foreach(var caVe in pagina.lista)
            {
                _campanhaVendaTipoProposta.PreencherCampanhaTipoProposta(caVe);
                _tipoPagamentoCampanhaVenda.PreencherCampanhaTipoPagamento(caVe);
            }

            return pagina;
        }

        public CampanhaVendaDTO FindByIdFullLoaded(int? cveId, 
            bool trazCampanhaVendaTipoProposta = false, 
            bool trazTipoPagamentoCampanhaVenda = false,
            bool trazCampanhaVendasProdCmp = false)
        {
            var campanhaVenda = FindById(cveId);
            if(campanhaVenda != null)
            {
                if(trazCampanhaVendaTipoProposta)
                {
                    _campanhaVendaTipoProposta.PreencherCampanhaTipoProposta(campanhaVenda);
                }

                if(trazTipoPagamentoCampanhaVenda)
                {
                    _tipoPagamentoCampanhaVenda.PreencherCampanhaTipoPagamento(campanhaVenda);
                }

                if (trazCampanhaVendasProdCmp)
                {
                    _campanhaVendasProdutoComposicaoSRV.PreencherCampanhaVendasProdutoComposicao(campanhaVenda);
                }

            }
            return campanhaVenda;
        }

        public CampanhaVendaDTO SalvarCampanhaVenda(CampanhaVendaDTO campanhaVenda)
        {
            CampanhaVendaDTO campanha = campanhaVenda;

            using(var scope = new TransactionScope())
            {
                var campanhaSalva = SaveOrUpdate(campanhaVenda);

                if (campanhaVenda.CVE_ID == null)
                    campanhaVenda.CVE_ID = campanhaSalva.CVE_ID;

                _campanhaVendaTipoProposta.SalvarEExcluirCampanhaVendaTipoProposta(campanhaVenda);
                _tipoPagamentoCampanhaVenda.SalvarEExcluirTipoPagamentoCampanhaVenda(campanhaVenda);
                _campanhaVendasProdutoComposicaoSRV.SalvarEExcluirCampanhaVendaProdutoComposicao(campanha);
                scope.Complete();
            }

            return campanha;
        }

        public void PausarOuAtivarCampanhaVenda(int? cveId)
        {
            var campanhaVenda = FindById(cveId);
            if(campanhaVenda != null)
            {
                if (campanhaVenda.CVE_CAMPANHA_ATIVA == null ||
                    campanhaVenda.CVE_CAMPANHA_ATIVA == false)
                    campanhaVenda.CVE_CAMPANHA_ATIVA = true;
                else
                {
                    campanhaVenda.CVE_CAMPANHA_ATIVA = false;
                }

                Merge(campanhaVenda);
            }
        }

        public void ExcluirCampanhaVenda(int? cveId)
        {
            var campanhaVenda = FindById(cveId);
            if (campanhaVenda != null)
            {
                campanhaVenda.DATA_EXCLUSAO = DateTime.Now;
                Merge(campanhaVenda);
            }
        }

        public IList<CampanhaVendaDTO> BuscarCampanhaVenda(DateTime data, int? tppId, int? tpgId, int? numParcela, int? cmpId = null)
        {
            return _dao.BuscarCampanhaVenda(data, tppId, tpgId, numParcela, cmpId);
        }
        
    }
}
