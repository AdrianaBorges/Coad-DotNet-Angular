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
using COAD.CORPORATIVO.Model.Dto.Custons.Pesquisas;
using GenericCrud.Excel;
using COAD.CORPORATIVO.Model.Dto.Custons.Planilhas;
using COAD.CORPORATIVO.Util;
using GenericCrud.Excel.Impl;
using COAD.SEGURANCA.Service;
using System.Text.RegularExpressions;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("CNC_ID")]
    public class CnabConfigSRV : GenericService<CNAB_CONFIG, CnabConfigDTO, Int32>
    {

        public CnabConfigDAO _dao { get; set; }
        public CnabSRV _cnabSRV { get; set; }
        public CnabConfigArquivoSRV _cnabConfigArquivoSRV { get; set; }
        public EmpresaSRV _empresaSRV { get; set; }

        public CnabConfigSRV(CnabConfigDAO _dao)
        {
            this._dao = _dao;
            this.Dao = _dao;
        }
        public Pagina<CnabConfigDTO> PesquisarCnabConfig(PesquisaCnabConfigDTO pesquisa)
        {
            var lstCnabConfig = _dao.PesquisarCnabConfig(pesquisa);
            PreencherEmpresa(lstCnabConfig.lista);
            return lstCnabConfig;
        }

        

        public CnabConfigDTO FindByIdFullLoaded(int? cncId, bool trazCnabArq = false, bool trazCnabs = false)
        {
            var cnabConfig = FindById(cncId);

            if(cnabConfig != null && trazCnabArq)
            {
                _cnabConfigArquivoSRV.PreencherCnabsNaConfig(cnabConfig, trazCnabs);
            }

            return cnabConfig;
        }

        public void SalvarCnabConfig(CnabConfigDTO cnabConfig)
        {
            if (cnabConfig != null)
            {
                //Validar(carrinhoCompras);
                using (TransactionScope scope = new TransactionScope())
                {
                    _processarSalvamentoCnabConfig(cnabConfig);
                    scope.Complete();
                }
            }
        }

        private void _processarSalvamentoCnabConfig(CnabConfigDTO cnabConfig)
        {
            if (cnabConfig == null)
            {
                throw new ValidacaoException("Config de Cnab não encontrada");
            }
            

            var cnabsConfigArq = cnabConfig.CNAB_CONFIG_ARQUIVO;
            cnabConfig.CNAB_CONFIG_ARQUIVO = null;

            var carrinhoSalvo = SaveOrUpdate(cnabConfig);
            cnabConfig.CNC_ID = carrinhoSalvo.CNC_ID;

            cnabConfig.CNAB_CONFIG_ARQUIVO = cnabsConfigArq;
            _cnabConfigArquivoSRV.SalvarEExcluirCnab(cnabConfig);
        }


        public void PreencherEmpresa(IEnumerable<CnabConfigDTO> lstCnabConfig)
        {
            if (lstCnabConfig != null)
            {
                foreach (var pro in lstCnabConfig)
                {
                    if (pro.EMP_ID != null)
                    {
                        pro.EMPRESAS = _empresaSRV.FindById(pro.EMP_ID);
                    }
                }
            }
        }
    }
}
