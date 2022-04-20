
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
using GenericCrud.Config.DataAttributes.Maps;
using COAD.CORPORATIVO.Model.Dto.Custons.Impostos;
using GenericCrud.Service;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("CFI_ID")]
    public class ConfigImpostoSRV : GenericService<CONFIG_IMPOSTO, ConfigImpostoDTO, int>
    {        
        private ConfigImpostoDAO _dao { get; set; }

        [ServiceProperty("CFI_ID", Name = "cfgImpostoImposto", PropertyName = "CONFIG_IMPOSTO_IMPOSTO")]
        public ConfigImpostoImpostoSRV _configImpostoImpostoSRV { get; set; }

        public ConfigImpostoSRV()
        {
            this._configImpostoImpostoSRV = new ConfigImpostoImpostoSRV();
            this.Dao = new ConfigImpostoDAO();
        }

        public ConfigImpostoSRV(ConfigImpostoDAO _dao)
        {
            this.Dao = _dao;
            this._dao = _dao;
        }

        public IList<ConfigImpostoDTO> ObterConfiguracaoPorRegras(
                RequisicaoConfigImpostoDTO requisicao)
        {
            var lstResult = _dao.ObterConfiguracaoPorRegras(requisicao);

            if(lstResult != null)
            {
                foreach(var conf in lstResult)
                {
                    _configImpostoImpostoSRV.PreencherImpostosDaConfiguracao(conf, requisicao.SobreTotal);
                }
            }

            return lstResult;
        }

        public IList<ConfigImpostoDTO> ListarConfigImpostoNotaFiscalConfig(int? nfcId, bool preencherImpostos = false)
        {
            var lstConfigImposto = _dao.ListarConfigImpostoNotaFiscalConfig(nfcId);

            if(preencherImpostos 
                && lstConfigImposto != null 
                && lstConfigImposto.Count > 0)
            {
                foreach(var config in lstConfigImposto)
                {
                    _configImpostoImpostoSRV.PreencherImpostosDaConfiguracao(config);
                }
            }

            return lstConfigImposto;
        }

        public void PreencherConfigImposto(NotaFiscalConfigDTO notaFiscalConfig, bool preencherImpostos = false)
        {
            if(notaFiscalConfig != null)
            {
                notaFiscalConfig.CONFIG_IMPOSTO = ListarConfigImpostoNotaFiscalConfig(notaFiscalConfig.NFC_ID, preencherImpostos);
            }
        }
        

        public void SalvarEExcluirConfigImposto(NotaFiscalConfigDTO notaFiscalConfig)
        {
            var lstConfigImposto = notaFiscalConfig.CONFIG_IMPOSTO;
            if (lstConfigImposto != null)
            {
                ExcluirConfigImposto(notaFiscalConfig);
                SalvarConfigImposto(lstConfigImposto, notaFiscalConfig);
            }

        }

        public void SalvarConfigImposto(IEnumerable<ConfigImpostoDTO> lstConfigImposto, NotaFiscalConfigDTO notaFiscalConfig)
        {
            CheckAndAssignKeyFromParentToChildsList(notaFiscalConfig, lstConfigImposto, "NFC_ID");
            _SalvarConfigImposto(lstConfigImposto);
        }

        public void _SalvarConfigImposto(IEnumerable<ConfigImpostoDTO> lstConfigImposto)
        {
            if (lstConfigImposto != null)
            {
                var lstConfigSalva = SaveOrUpdateAll(lstConfigImposto).ToList();

                var index = 0;
                foreach(var conf in lstConfigImposto)
                {
                    if(conf.CFI_ID == null && lstConfigSalva[index] != null)
                    {
                        conf.CFI_ID = lstConfigSalva[index].CFI_ID;
                    }
                    _configImpostoImpostoSRV.SalvarEExcluirConfigImpostoImposto(conf);
                    index++;
                }
            }

        }

        public void ExcluirConfigImposto(NotaFiscalConfigDTO notaFiscalConfig)
        {
            if (notaFiscalConfig.DATA_EXCLUSAO == null)
            {
                var nfcID = notaFiscalConfig.NFC_ID;
                var nfConfigBanco = ServiceFactory.RetornarServico<NotaFiscalConfigSRV>().FindByIdFullLoaded(nfcID, true);
                var lstConfigImposto = GetMissinList(notaFiscalConfig, nfConfigBanco, "CONFIG_IMPOSTO");
                DeletarConfigImposto(lstConfigImposto);
            }
        }

        public void DeletarConfigImposto(IEnumerable<ConfigImpostoDTO> lstConfigImposto)
        {
            if (lstConfigImposto != null)
            {
                foreach (var regTab in lstConfigImposto)
                {
                    regTab.DATA_EXCLUSAO = DateTime.Now;
                }

                SaveOrUpdateAll(lstConfigImposto);
            }
        }

        public ConfigImpostoDTO FindByIdFullLoaded(int? cfiId, bool trazConfigImpostoImposto = false)
        {
            var configImposto = FindById(cfiId);

            if(configImposto != null && trazConfigImpostoImposto)
            {
                _configImpostoImpostoSRV.PreencherImpostosDaConfiguracao(configImposto);
            }

            return configImposto;
        }

        public ICollection<ConfigImpostoDTO> ClonarConfigImposto(NotaFiscalConfigDTO configDTO)
        {
            ICollection<ConfigImpostoDTO> lstConfigImposto = new List<ConfigImpostoDTO>();

            if (configDTO != null)
            {
                if (configDTO.CONFIG_IMPOSTO != null)
                {
                    foreach (var configImposto in configDTO.CONFIG_IMPOSTO)
                    {
                        var configImpostoClone = new ConfigImpostoDTO()
                        {
                            CFI_CLIENTE_RETEM = configImposto.CFI_CLIENTE_RETEM,
                            CFI_DESC_REGRA = configImposto.CFI_DESC_REGRA,
                            CFI_EMPRESA_DO_SIMPLES = configImposto.CFI_EMPRESA_DO_SIMPLES,
                            CFI_QUALQUER_VALOR = configImposto.CFI_QUALQUER_VALOR,
                            CFI_VLR_DESCONTO_MIM = configImposto.CFI_VLR_DESCONTO_MIM,
                            TIPO_CLI_ID = configImposto.TIPO_CLI_ID,
                            TIPO_CLIENTE = configImposto.TIPO_CLIENTE,
                            CFI_CODIGO_TRIBUTACAO_MUNICIPIO = configImposto.CFI_CODIGO_TRIBUTACAO_MUNICIPIO,
                            
                        };

                        configImpostoClone.CONFIG_IMPOSTO_IMPOSTO = _configImpostoImpostoSRV
                            .ClonarConfigImpostoImposto(configImposto);

                        lstConfigImposto.Add(configImpostoClone);
                    }
                }
            }

            return lstConfigImposto;
        }
        

    }
}
