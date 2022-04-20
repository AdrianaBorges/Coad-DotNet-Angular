

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
	[ServiceConfig("CCA_ID")]
	public class CnabConfigArquivoSRV : GenericService<CNAB_CONFIG_ARQUIVO, CnabConfigArquivoDTO, Int32>
	{

        public CnabConfigArquivoDAO _dao { get; set; }
        public CnabSRV _cnabSRV { get; set; }
        public CnabConfigArquivoSRV(CnabConfigArquivoDAO _dao)
        {
			this._dao = _dao;
			this.Dao = _dao;
        }

        public IList<CnabConfigArquivoDTO> ListarCnabsConfigArquivoDaConfig(int? cncId)
        {
            return _dao.ListarCnabsConfigArquivoDaConfig(cncId);
        }


        public void PreencherCnabsNaConfig(CnabConfigDTO cnabConfig, bool trazCnabs = false)
        {
            if (cnabConfig != null)
            {
                cnabConfig.CNAB_CONFIG_ARQUIVO = ListarCnabsConfigArquivoDaConfig(cnabConfig.CNC_ID);

                if (trazCnabs && cnabConfig.CNAB_CONFIG_ARQUIVO != null)
                {
                    foreach(var cnabConfigArq in cnabConfig.CNAB_CONFIG_ARQUIVO)
                    {
                        _cnabSRV.PreencherCnabsNaConfig(cnabConfigArq);
                    }
                }
            }
        }

        public void SalvarEExcluirCnab(CnabConfigDTO cnabConfig)
        {
            var cnabsConfigArq = cnabConfig.CNAB_CONFIG_ARQUIVO;
            if (cnabsConfigArq != null)
            {
                ExcluirConfigCnabArquivo(cnabConfig);
                SalvarCnabConfigArquivo(cnabsConfigArq, cnabConfig);
            }

        }

        public void SalvarCnabConfigArquivo(IEnumerable<CnabConfigArquivoDTO> lstCnabConfigArq, CnabConfigDTO cnabConfig)
        {
            CheckAndAssignKeyFromParentToChildsList(cnabConfig, lstCnabConfigArq, "CNC_ID");

            var lstCnabConfigArqSalvo = SaveOrUpdateAll(lstCnabConfigArq).ToList();

            var index = 0;

            foreach (var cnabConfigArq in lstCnabConfigArq)
            {
                if (cnabConfigArq.CCA_ID == null && lstCnabConfigArqSalvo[index] != null)
                {
                    cnabConfigArq.CCA_ID = lstCnabConfigArqSalvo[index].CCA_ID;
                }

                _cnabSRV.SalvarEExcluirCnab(cnabConfigArq);
                index++;
            }

        }


        public void ExcluirConfigCnabArquivo(CnabConfigDTO cnabConfig)
        {
            if (cnabConfig.DATA_EXCLUSAO == null)
            {
                var cncId = cnabConfig.CNC_ID;
                var nfConfigBanco = ServiceFactory.RetornarServico<CnabConfigSRV>().FindByIdFullLoaded(cncId, true);
                var lstConfigImposto = GetMissinList(cnabConfig, nfConfigBanco, "CNAB_CONFIG_ARQUIVO");
                DeletarCnab(lstConfigImposto);
            }
        }

        public void DeletarCnab(IEnumerable<CnabConfigArquivoDTO> cnabsConfigArq)
        {
            if (cnabsConfigArq != null)
            {
                foreach (var regTab in cnabsConfigArq)
                {
                    regTab.DATA_EXCLUSAO = DateTime.Now;
                }

                SaveOrUpdateAll(cnabsConfigArq);
            }
        }

        public CnabConfigArquivoDTO FindByIdFullLoaded(int? ccaId, bool trazCnab = false)
        {
            var cnabConfigArq = FindById(ccaId);

            if (cnabConfigArq != null && trazCnab)
            {
                _cnabSRV.PreencherCnabsNaConfig(cnabConfigArq);
            }

            return cnabConfigArq;
        }


    }
}
