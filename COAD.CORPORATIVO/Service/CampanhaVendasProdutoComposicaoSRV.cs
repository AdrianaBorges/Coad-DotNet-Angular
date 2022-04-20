

using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Repositorios.Base;
using System.Transactions;
using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.Config.DataAttributes;
using System.Web;
using System.IO;
using GenericCrud.Service;
using GenericCrud.Validations;
using Coad.GenericCrud.Exceptions;
using COAD.CORPORATIVO.Repository.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;

namespace COAD.CORPORATIVO.Service
{ 
	[ServiceConfig("CVE_ID", "CMP_ID")]
	public class CampanhaVendasProdutoComposicaoSRV : GenericService<CAMPANHA_VENDAS_PRODUTO_COMPOSICAO, CampanhaVendasProdutoComposicaoDTO, Int32>
	{

        public CampanhaVendasProdutoComposicaoDAO _dao { get; set; }

        public CampanhaVendasProdutoComposicaoSRV(CampanhaVendasProdutoComposicaoDAO _dao)
        {
			this._dao = _dao;
			this.Dao = _dao;
        }


        public void SalvarEExcluirCampanhaVendaProdutoComposicao(CampanhaVendaDTO campanha)
        {
            ChecarExcluirCampanhaVendaProdutoComposicao(campanha);

            var lstCampVendaProdComp = campanha.CAMPANHA_VENDAS_PRODUTO_COMPOSICAO;

            if (lstCampVendaProdComp != null)
            {
                SalvarCampanhaVendaProdutoComposicao(campanha, lstCampVendaProdComp.AsQueryable());
            }
        }

        public void SalvarCampanhaVendaProdutoComposicao(CampanhaVendaDTO campanha, IQueryable<CampanhaVendasProdutoComposicaoDTO> lstCampVendaProdComp)
        {
            if (lstCampVendaProdComp != null)
            {
                foreach (var camVProdCmp in lstCampVendaProdComp)
                {
                    if (camVProdCmp.CVE_ID == null && campanha.CVE_ID != null)
                    {
                        camVProdCmp.CVE_ID = campanha.CVE_ID;
                    }

                    if (camVProdCmp.CMP_ID == null && camVProdCmp.PRODUTO_COMPOSICAO != null)
                    {
                        camVProdCmp.CMP_ID = camVProdCmp.PRODUTO_COMPOSICAO.CMP_ID;
                    }
                }
                SaveOrUpdateNonIdentityKeyEntity(lstCampVendaProdComp);
            }
        }

        public void ChecarExcluirCampanhaVendaProdutoComposicao(CampanhaVendaDTO campanha)
        {
            CampanhaVendaDTO campanhaDoBanco = ServiceFactory.RetornarServico<CampanhaVendaSRV>().FindByIdFullLoaded(campanha.CVE_ID, false, false, true);
            ExcluirList<CampanhaVendaDTO>(campanha, campanhaDoBanco, "CAMPANHA_VENDAS_PRODUTO_COMPOSICAO");

        }

        /// <summary>
        /// Lista os CampanhaVendasProdutoComposicao por Código de Campanha de Vendas
        /// </summary>
        /// <returns></returns>
        public ICollection<CampanhaVendasProdutoComposicaoDTO> ListarCampVendasProdutoCmpPorCampanha(int? cveId)
        {
            return _dao.ListarCampVendasProdutoCmpPorCampanha(cveId);
        }

        public void PreencherCampanhaVendasProdutoComposicao(CampanhaVendaDTO campanha)
        {
            if(campanha != null && campanha.CVE_ID != null)
            {
                campanha.CAMPANHA_VENDAS_PRODUTO_COMPOSICAO = ListarCampVendasProdutoCmpPorCampanha(campanha.CVE_ID);

                if(campanha.CAMPANHA_VENDAS_PRODUTO_COMPOSICAO != null)
                {
                    var cmpSRV = ServiceFactory.RetornarServico<ProdutoComposicaoSRV>();
                    foreach(var cvCmp in campanha.CAMPANHA_VENDAS_PRODUTO_COMPOSICAO)
                    {
                        if(cvCmp.PRODUTO_COMPOSICAO != null)
                        {
                            cmpSRV.PreencherEmpresa(cvCmp.PRODUTO_COMPOSICAO);
                        }
                    }
                }
            }
        }

    }
}
