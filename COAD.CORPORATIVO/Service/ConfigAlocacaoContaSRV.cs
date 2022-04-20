
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
using Coad.GenericCrud.Repositorios.Base;
using GenericCrud.Config.DataAttributes;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Service;

namespace COAD.CORPORATIVO.Service
{
    public enum TipoAmbientePagamento
    {
        /// <summary>
        /// Todo processo de pagamento fica a cargo das funcionalidades do sistema
        /// </summary>
        INTERNO = 1,

        /// <summary>
        /// Parte do processo de pagamento será utilizando um Gateway
        /// </summary>
        GATEWAY_PAGAMENTO = 2
    }

    [ServiceConfig("CFA_ID", "CTA_ID", "TCC_ID")]
    public class ConfigAlocacaoContaSRV : GenericService<CONFIG_ALOCACAO_CONTA, ConfigAlocacaoContaDTO, int>
    {
        public ConfigAlocacaoContaDAO _dao; //= new ConfigAlocacaoContaDAO();
        public ContaSRV _contaSRV {get; set;} //= new ContaSRV();

        public ConfigAlocacaoContaSRV()
        {
            this.Dao = new ConfigAlocacaoContaDAO();
            this._contaSRV = new ContaSRV();
        }
        public ConfigAlocacaoContaSRV(ConfigAlocacaoContaDAO _dao)
        {
            this._dao = _dao;
            this.Dao = _dao;
        }


        [Obsolete("Ainda não está sendo utilizado. Poderá ser usado no futuro. Ao invéz desse método, utilize o [RetornarPorTipo]")]
        public ContaDTO RetornaPorConfiguracao(RegiaoDTO regiao, int? EMP_ID, int? TCC_ID)
        {
            if (regiao != null)
            {
                var rgId = regiao.RG_ID;

                if (EMP_ID == null)
                {
                    EMP_ID = regiao.EMP_ID;
                }

                var contaId = _dao.RetornarCtaIdDeAcordoComConfiguracao(rgId, EMP_ID, TCC_ID);
                var conta = _contaSRV.FindById(contaId);

                if (conta != null && conta.DATA_EXCLUSAO == null)
                    return conta;                
            }

            return _contaSRV.FindById(1);
        }

        /// <summary>
        /// Use esse método provisóriamente em subtituição ao [RetornaPorConfiguracao]
        /// </summary>
        /// <param name="tipoPagamento"></param>
        /// <returns></returns>
        public ContaDTO RetornaPorTipo(TipoAmbientePagamento tipoPagamento)
        {
            int IdConta = (tipoPagamento.Equals(TipoAmbientePagamento.INTERNO)) ? 1 : 61;
            return _contaSRV.FindById(IdConta);
        }
    }
}
