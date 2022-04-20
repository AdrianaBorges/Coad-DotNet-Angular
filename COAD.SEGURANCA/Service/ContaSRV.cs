using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using COAD.SEGURANCA.Model;
using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.Config.DataAttributes;
using COAD.SEGURANCA.DAO;
using System.Data.Entity.Validation;
using Coad.GenericCrud.Exceptions;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Model.FiltersInfo;
using GenericCrud.Service;

namespace COAD.SEGURANCA.Service
{
    [ServiceConfig("CTA_ID")]
    public class ContaSRV : GenericService<CONTA, ContaDTO, int>
    {
        private ContaDAO _dao { get; set; }

        public ContaSRV()
        {
            this._dao = new ContaDAO();
            this.Dao = this._dao;
            
        }

        public ContaSRV(ContaDAO _dao)
        {
            this._dao = _dao;
            this.Dao = _dao;

        }

        public List<int> ObterContas(List<int> ctaId, bool cedenteEmite)
        {
            return _dao.ObterContas(ctaId, cedenteEmite);
        }

        public IList<ContaDTO> ListarPorEmpresa(int? empresa_id = null)
        {
            return _dao.ListarPorEmpresa(empresa_id);
        }

        public IList<ContaDTO> Listar(string banco_id)
        {
            return _dao.Listar(banco_id);
        }

        public IList<ContaDTO> ListarPorEmpresa(int emp_id)
        {
            return _dao.ListarPorEmpresa(emp_id);
        }
        public IList<ContaDTO> ListarContasBanco(string bco, int empid)
        {
            return _dao.ListarContasBanco(bco, empid);
        }
        public ContaDTO ListarEnviaBoletoAvulso(int empid, string banco_id)
        {
            return _dao.ListarEnviaBoletoAvulso(empid, banco_id);
        }
        public IList<ContaDTO> Listar(string banco_id, bool cta_emite_boleto)
        {
            return _dao.Listar(banco_id, cta_emite_boleto);
        }
        public IList<ContaDTO> Listar(int? empresa_id = null, string banco_id = null, string agencia = null, string tipo = null)
        {
            return _dao.Listar(empresa_id,banco_id,agencia,tipo);
        }

        public ContaDTO SalvarConta(ContaDTO conta)
        {
            try
            {
                if(conta != null)
                {
                    if(conta.CTA_ID != null)
                    {
                        conta.DATA_ALTERACAO = DateTime.Now;
                    }
                    else
                    {
                        conta.DATA_CADASTRO = DateTime.Now;
                    }

                    if(conta.EMP_ID == conta.EMP_ID_S_AVS)
                    {
                        throw new Exception("A empresa avalista não pode ser a mesma da conta");
                    }
                }

                var contaSalva = SaveOrUpdate(conta);
                return contaSalva;
                
            }
            catch(Exception e)
            {
                throw new Exception("Não é possível salvar a conta", e);
            }
        }

        public void DeletarConta(int contaId)
        {
            var conta = this.FindById(contaId);
            conta.DATA_EXCLUSAO = DateTime.Now;
            Merge(conta, "CTA_ID");
        }

        public ContaDTO BuscarContaBoletoAvuso()
        {
            return _dao.BuscarContaBoletoAvuso();
        }
        public ContaDTO BuscarContaBoletoAvulso(int empid, String bco)
        {
            return ServiceFactory.RetornarServico<ContaSRV>().ListarEnviaBoletoAvulso(empid, bco);
        }

        public Pagina<ContaDTO> BuscarContas(ContaFiltrosDTO filtros)
        {
            return _dao.BuscarContas(filtros);
        }

        /// <summary>
        /// Lista todas as contas marcadas para enviar boletos automáticos
        /// </summary>
        /// <returns></returns>
        public IList<ContaDTO> ListarContasEnviaBoleto()
        {
            return _dao.ListarContasEnviaBoleto();
        }

        /// <summary>
        /// Lista todas os ID de contas marcadas para enviar boletos automáticos
        /// </summary>
        /// <returns></returns>
        public List<int> ListarIDContasEnviaBoleto()
        {
            return _dao.ListarIDContasEnviaBoleto();
        }

        public ContaDTO BuscarContaRemessa(int empid, string bco, bool CTA_CEDENTE_EMITE_BOLETO)
        {
            return ServiceFactory.RetornarServico<ContaSRV>().ListarParaRemessa(empid, bco, CTA_CEDENTE_EMITE_BOLETO);
        }

        public ContaDTO ListarParaRemessa(int empid, string banco_id, bool CTA_CEDENTE_EMITE_BOLETO)
        {
            return _dao.ListarParaRemessa(empid, banco_id, CTA_CEDENTE_EMITE_BOLETO);
        }

        public bool incrementarUltimoNossoNumeroDeConta ( int? CTA_ID )
        {
            try
            {

                CONTA conta = this.Dao.FindById(CTA_ID);

                long? atualNossoNumero = conta.CTA_ULTIMO_NOSSO_NUMERO + 1;

                if (atualNossoNumero == null)
                    atualNossoNumero = 1;

                conta.CTA_ULTIMO_NOSSO_NUMERO = atualNossoNumero;

                conta = this.Dao.Update(conta);

                return (conta.CTA_ULTIMO_NOSSO_NUMERO == atualNossoNumero ? true : false);

            }
            catch (Exception e)
            {
                return false;
            }

        }

        public ContaDTO RetornarContaPorAgenciaEConta(int CTA_AGENCIA, int CTA_CONTA)
        {
            return _dao.RetornarContaPorAgenciaEConta(CTA_AGENCIA, CTA_CONTA);
        }


    }
}
