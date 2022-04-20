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

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("TPG_ID", "CVE_ID")]
    public class TipoPagamentoCampanhaVendaSRV : GenericService<TIPO_PAGAMENTO_CAMPANHA_VENDA, TipoPagamentoCampanhaVendaDTO, int>
    {
        private TipoPagamentoCampanhaVendaDAO _dao;

        public TipoPagamentoCampanhaVendaSRV()
        {
            this._dao = new TipoPagamentoCampanhaVendaDAO();
            this.Dao = _dao;
        }

        public TipoPagamentoCampanhaVendaSRV(TipoPagamentoCampanhaVendaDAO _dao)
        {
            this._dao = _dao;
            this.Dao = _dao;
        }

        public ICollection<TipoPagamentoCampanhaVendaDTO> ListarTipoPagamentoCampanhaVenda(int? cveId)
        {
            return _dao.ListarTipoPagamentoCampanhaVenda(cveId);
        }

        public void PreencherCampanhaTipoPagamento(CampanhaVendaDTO campanhaVenda)
        {
            if(campanhaVenda != null)
            {
                campanhaVenda.TIPO_PAGAMENTO_CAMPANHA_VENDA = ListarTipoPagamentoCampanhaVenda(campanhaVenda.CVE_ID);
            }
        }

        
        public void SalvarEExcluirTipoPagamentoCampanhaVenda(CampanhaVendaDTO campanha)
        {
            ChecarExcluirTipoPagamentoCampanhaVenda(campanha);
            
            var lstVendaTipoPagamento = campanha.TIPO_PAGAMENTO_CAMPANHA_VENDA;

            if (lstVendaTipoPagamento != null)
            {
                SalvarTipoPagamentoCampanhaVenda(campanha, lstVendaTipoPagamento.AsQueryable());
            }
        }

        public void SalvarTipoPagamentoCampanhaVenda(CampanhaVendaDTO campanha, IQueryable<TipoPagamentoCampanhaVendaDTO> tipoPagamentoCampanhaVenda)
        {
            if (tipoPagamentoCampanhaVenda != null)
            {
                foreach(var tpCamV in tipoPagamentoCampanhaVenda)
                {
                    if(tpCamV.CVE_ID == null && campanha.CVE_ID != null)
                    {
                        tpCamV.CVE_ID = campanha.CVE_ID;                        
                    }

                    if(tpCamV.TPG_ID == null && tpCamV.TIPO_PAGAMENTO != null)
                    {
                        tpCamV.TPG_ID = tpCamV.TIPO_PAGAMENTO.TPG_ID;
                    }
                }
                SaveOrUpdateNonIdentityKeyEntity(tipoPagamentoCampanhaVenda);
            }
        }

        public void ChecarExcluirTipoPagamentoCampanhaVenda(CampanhaVendaDTO campanha)
        {
            CampanhaVendaDTO campanhaDoBanco = ServiceFactory.RetornarServico<CampanhaVendaSRV>().FindByIdFullLoaded(campanha.CVE_ID, false, true);
            ExcluirList<CampanhaVendaDTO>(campanha, campanhaDoBanco, "TIPO_PAGAMENTO_CAMPANHA_VENDA");

        }

    }
}
