
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
using GenericCrud.Service;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("CFI_ID", "IMP_ID")]
    public class ConfigImpostoImpostoSRV : GenericService<CONFIG_IMPOSTO_IMPOSTO, ConfigImpostoImpostoDTO, int>
    {        
        public ConfigImpostoImpostoDAO _dao { get; set; }

        public ConfigImpostoImpostoSRV()
        {
            this._dao = new ConfigImpostoImpostoDAO();
            this.Dao = _dao;
        }

        public ConfigImpostoImpostoSRV(ConfigImpostoImpostoDAO _dao)
        {
            this._dao = _dao;
            this.Dao = _dao;
        }

        public ICollection<ConfigImpostoImpostoDTO> ListarImpostosDaConfiguracao(int? cfiId, bool? sobreTotal = null)
        {
            return _dao.ListarImpostosDaConfiguracao(cfiId, sobreTotal);
        }

        public void PreencherImpostosDaConfiguracao(ConfigImpostoDTO config, bool? sobreTotal = null)
        {
            if(config != null && config.CFI_ID != null)
            {
                config.CONFIG_IMPOSTO_IMPOSTO = ListarImpostosDaConfiguracao(config.CFI_ID, sobreTotal);
            }
        }

        
        public void SalvarEExcluirConfigImpostoImposto(ConfigImpostoDTO configImposto)
        {
            ExcluirConfigImpostoImposto(configImposto);
            
            var lstConfigImpostoImposto = configImposto.CONFIG_IMPOSTO_IMPOSTO;

            if (lstConfigImpostoImposto != null)
            {
                SalvarConfigImpostoImposto(configImposto, lstConfigImpostoImposto.AsQueryable());
            }
        }


        public void ExcluirConfigImpostoImposto(ConfigImpostoDTO configImposto)
        {
            var cfiId = (int)configImposto.CFI_ID;

            ConfigImpostoDTO configImpostoBanco = ServiceFactory.RetornarServico<ConfigImpostoSRV>().FindByIdFullLoaded(cfiId, true);
            ExcluirList<ConfigImpostoDTO>(configImposto, configImpostoBanco, "CONFIG_IMPOSTO_IMPOSTO");

        }

        public void SalvarConfigImpostoImposto(ConfigImpostoDTO configImposto, IQueryable<ConfigImpostoImpostoDTO> lstConfigImpostoImposto)
        {
            if (lstConfigImpostoImposto != null)
            {
                CheckAndAssignKeyFromParentToChildsList(configImposto, lstConfigImpostoImposto, "CFI_ID");
                SaveOrUpdateNonIdentityKeyEntity(lstConfigImpostoImposto);
            }
        }

        public ICollection<ConfigImpostoImpostoDTO> ClonarConfigImpostoImposto(ConfigImpostoDTO configImposto)
        {
            ICollection<ConfigImpostoImpostoDTO> lstConfigImpostoImposto = new List<ConfigImpostoImpostoDTO>();

            if(configImposto != null 
                && configImposto.CONFIG_IMPOSTO_IMPOSTO != null)
            {
                foreach(var confImpostoImposto in configImposto.CONFIG_IMPOSTO_IMPOSTO)
                {
                    lstConfigImpostoImposto.Add(new ConfigImpostoImpostoDTO()
                    {
                        CII_ALIQUOTA = confImpostoImposto.CII_ALIQUOTA,
                        DATETIME = DateTime.Now,
                        IMP_ID = confImpostoImposto.IMP_ID
                    });
                }
            }

            return lstConfigImpostoImposto;
        }
    }
}
