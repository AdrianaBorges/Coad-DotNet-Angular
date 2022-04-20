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
    [ServiceConfig("CVE_ID", "TPP_ID")]
    public class CampanhaVendaTipoPropostaSRV : GenericService<CAMPANHA_VENDA_TIPO_PROPOSTA, CampanhaVendaTipoPropostaDTO, int>
    {
        private CampanhaVendaTipoPropostaDAO _dao;

        public CampanhaVendaTipoPropostaSRV()
        {
            this._dao = new CampanhaVendaTipoPropostaDAO();
            this.Dao = _dao;
        }

        public CampanhaVendaTipoPropostaSRV(CampanhaVendaTipoPropostaDAO _dao)
        {
            this._dao = _dao;
            this.Dao = _dao;
        }

        public ICollection<CampanhaVendaTipoPropostaDTO> ListarCampanhaTipoProposta(int? cveId)
        {
            return _dao.ListarCampanhaTipoProposta(cveId);
        }

        public void PreencherCampanhaTipoProposta(CampanhaVendaDTO campanhaVenda)
        {
            if(campanhaVenda != null)
            {
                campanhaVenda.CAMPANHA_VENDA_TIPO_PROPOSTA = ListarCampanhaTipoProposta(campanhaVenda.CVE_ID);
            }
        }


        /// <summary>
        /// Atualiza os emails e remove dos bancos os que foram excluidos da lista
        /// </summary>
        /// <param name="cliente"></param>
        public void SalvarEExcluirCampanhaVendaTipoProposta(CampanhaVendaDTO campanha)
        {
            ChecarExcluirCampanhaVendaTipoProposta(campanha);

            // AssinaturaDTO clientes = _assinaturaSRV.ExtrairAssinaturaCliente(cliente, PROD_ID);
            //var lstTelefone = ExtrairEmailDaAssinaturaCliente(cliente, PROD_ID);
            var lstVendaTipoProposta = campanha.CAMPANHA_VENDA_TIPO_PROPOSTA;

            if (lstVendaTipoProposta != null)
            {
                SalvarCampanhaTipoProposta(campanha, lstVendaTipoProposta.AsQueryable());
            }
        }
        /// <summary>
        /// Salva os email de um determinado prospect
        /// </summary>
        /// <param name="CLI_ID">Id inserido no telefone antes de salvar</param>
        /// <param name="campanhaVendaTipoProposta">Os telefones para serem salvos</param>
        /// <param name="atualizar">true = atualizar, false = incluir</param>
        public void SalvarCampanhaTipoProposta(CampanhaVendaDTO campanha, IQueryable<CampanhaVendaTipoPropostaDTO> campanhaVendaTipoProposta)
        {
            if (campanhaVendaTipoProposta != null)
            {
                foreach(var capaVen in campanhaVendaTipoProposta)
                {
                    if(capaVen.CVE_ID == null && campanha.CVE_ID != null)
                    {
                        capaVen.CVE_ID = campanha.CVE_ID;                        
                    }

                    if(capaVen.TPP_ID != null && capaVen.TIPO_PROPOSTA != null)
                    {
                        capaVen.TPP_ID = capaVen.TIPO_PROPOSTA.TPP_ID;
                    }
                }
                SaveOrUpdateNonIdentityKeyEntity(campanhaVendaTipoProposta);
            }
        }

        public void ChecarExcluirCampanhaVendaTipoProposta(CampanhaVendaDTO campanha)
        {
            CampanhaVendaDTO campanhaDoBanco = ServiceFactory.RetornarServico<CampanhaVendaSRV>().FindByIdFullLoaded(campanha.CVE_ID, true);
            ExcluirList<CampanhaVendaDTO>(campanha, campanhaDoBanco, "CAMPANHA_VENDA_TIPO_PROPOSTA");

        }

    }
}
