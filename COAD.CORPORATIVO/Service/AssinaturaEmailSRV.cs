using System;
using System.Collections.Generic;
using System.Linq;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.LEGADO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.SEGURANCA.Service;
using GenericCrud.Config.DataAttributes;
using GenericCrud.Validations;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("AEM_ID")]
    public class AssinaturaEmailSRV : ServiceAdapter<ASSINATURA_EMAIL, AssinaturaEmailDTO, int>
    {
        private AssinaturaEmailDAO _dao;
        public AssinaturaSRV _assinaturaSRV { get; set; }

        public AssinaturaEmailSRV(AssinaturaEmailDAO _dao)
        {
            this._dao = _dao;
            SetDao(_dao);
        }

        public AssinaturaEmailSRV()
        {
            this._dao = new AssinaturaEmailDAO();
            this._assinaturaSRV = new AssinaturaSRV();

            SetDao(_dao);
        }

        public IList<AssinaturaEmailDTO> BuscarEmailsRemovidos(string _assinatura, List<int> _emailsok)
        {
            return _dao.BuscarEmailsRemovidos(_assinatura, _emailsok);
        }
        
        public void ExcluirClienteEmail(ClienteDto cliente)
        {
            var CLI_ID = (int)cliente.CLI_ID;

            //AssinaturaDTO clientes = _assinaturaSRV.ExtrairAssinaturaFranquiaCliente(cliente);
            //string codigoAssinatura = clientes.ASN_NUM_ASSINATURA;
            //AssinaturaDTO assinaturaDoBanco = _assinaturaSRV.FindByIdFullLoaded(codigoAssinatura, false, true);
            ClienteDto clienteDoBanco = new ClienteSRV().FindByIdFullLoaded(CLI_ID, false, false, true);

            ExcluirList<ClienteDto>(cliente, clienteDoBanco, "ASSINATURA_EMAIL");

        }
        
        public void ExcluirClienteEmailAssinatura(ClienteDto cliente, int? PROD_ID)
        {
            var CLI_ID = (int)cliente.CLI_ID;

            AssinaturaDTO assinatura = _assinaturaSRV.ExtrairAssinaturaCliente(cliente, PROD_ID);
            string codigoAssinatura = assinatura.ASN_NUM_ASSINATURA;
            AssinaturaDTO assinaturaDoBanco = _assinaturaSRV.FindByIdFullLoaded(codigoAssinatura, false, true);

            ExcluirList<AssinaturaDTO>(assinatura, assinaturaDoBanco, "ASSINATURA_EMAIL");

        }

        public IList<AssinaturaEmailDTO> FindByNumAssinatura(string codigoAssinatura)
        {
            return _dao.FindByNumAssinatura(codigoAssinatura);
        }

        public void PreencherEmailAssinaturaNaAssinatura(AssinaturaDTO assinatura)
        {
            if (assinatura != null && !string.IsNullOrWhiteSpace(assinatura.ASN_NUM_ASSINATURA))
            {
                IList<AssinaturaEmailDTO> lstAssinaturaEmail = FindByNumAssinatura(assinatura.ASN_NUM_ASSINATURA);
                assinatura.ASSINATURA_EMAIL = lstAssinaturaEmail;
            }
        }

        public IList<AssinaturaEmailDTO> FindByCliente(int? CLI_ID)
        {
            return _dao.FindByCliente(CLI_ID);
        }

        public IList<AssinaturaEmailDTO> FindEmailsDoClienteEAssinatura(int? CLI_ID)
        {
            return _dao.FindEmailsDoClienteEAssinatura(CLI_ID);
        }

        public void PreencherEmailAssinaturaNoCliente(ClienteDto cliente)
        {
            if (cliente != null && cliente.ASN_NUM_ASSINATURA != null)
            {
                IList<AssinaturaEmailDTO> lstAssinaturaEmail = FindByCliente(cliente.CLI_ID);
                cliente.ASSINATURA_EMAIL = lstAssinaturaEmail;
            }
        }

        public void PreencherAssinaturaDaFranquia(IEnumerable<ClienteDto> clientes, bool trazTelefone = false, bool trazEmail = false)
        {
            if (clientes != null && clientes.Count() > 0)
            {
                foreach (var cli in clientes)
                {
                    _assinaturaSRV.PreencherAssinaturaDaFranquia(cli, trazTelefone, trazEmail);
                }
            }
        }

        //public void PreencherAssinaturaDaFranquia(IEnumerable<ClienteDto> clientes, bool trazTelefone = false, bool trazEmail = false)
        //{
        //    if (clientes != null && clientes.Count() > 0)
        //    {
        //        foreach (var cli in clientes)
        //        {
        //            _assinaturaSRV.PreencherAssinaturaDaFranquia(cli, trazTelefone, trazEmail);
        //        }
        //    }
        //}

        public ICollection<AssinaturaEmailDTO> ExtrairEmailDaAssinaturaFranquiaCliente(ClienteDto cliente, bool carregarDoBancoCasoNaoDisponivel = false)
        {
            if (cliente != null)
            {
                AssinaturaDTO assinatura = _assinaturaSRV.ExtrairAssinaturaFranquiaCliente(cliente, carregarDoBancoCasoNaoDisponivel, false, true);

                if (assinatura != null)
                {
                    ICollection<AssinaturaEmailDTO> assinaturaEmail = assinatura.ASSINATURA_EMAIL;
                    return assinaturaEmail;
                }
            }
            return null;
        }


        public ICollection<AssinaturaEmailDTO> ExtrairEmailDaAssinaturaCliente(ClienteDto cliente, int? PRO_ID, bool carregarDoBancoCasoNaoDisponivel = false)
        {
            if (cliente != null)
            {
                AssinaturaDTO assinatura = _assinaturaSRV.ExtrairAssinaturaCliente(cliente, PRO_ID, carregarDoBancoCasoNaoDisponivel, false, true);

                if (assinatura != null)
                {
                    ICollection<AssinaturaEmailDTO> assinaturaEmail = assinatura.ASSINATURA_EMAIL;
                    return assinaturaEmail;
                }
            }
            return null;
        }

        /// <summary>
        /// Salva os email de um determinado prospect
        /// </summary>
        /// <param name="CLI_ID">Id inserido no telefone antes de salvar</param>
        /// <param name="emails">Os telefones para serem salvos</param>
        /// <param name="atualizar">true = atualizar, false = incluir</param>
        public void SalvarEmails(ClienteDto cliente, IQueryable<AssinaturaEmailDTO> emails, AssinaturaDTO assinatura = null)
        {
            if (emails != null)
            {
                foreach (var tel in emails)
                {
                    if (tel.CLI_ID == null)
                    {
                        tel.CLI_ID = cliente.CLI_ID;
                    }

                    if (assinatura != null && !string.IsNullOrWhiteSpace(tel.ASN_NUM_ASSINATURA))
                    {
                        tel.ASN_NUM_ASSINATURA = assinatura.ASN_NUM_ASSINATURA;
                }
                }
                SaveOrUpdateAll(emails);

            }
        }

        /// <summary>
        /// Atualiza os emails e remove dos bancos os que foram excluidos da lista
        /// </summary>
        /// <param name="cliente"></param>
        public void SalvarEExcluirEmails(ClienteDto cliente)
        {
            ExcluirClienteEmail(cliente);

           // AssinaturaDTO clientes = _assinaturaSRV.ExtrairAssinaturaCliente(cliente, PROD_ID);
            //var lstTelefone = ExtrairEmailDaAssinaturaCliente(cliente, PROD_ID);
            var lstTelefone = cliente.ASSINATURA_EMAIL;

            if (lstTelefone != null)
            {
                SalvarEmails(cliente, lstTelefone.AsQueryable());
            }
        }

        /// <summary>
        /// Atualiza os emails e remove dos bancos os que foram excluidos da lista
        /// </summary>
        /// <param name="cliente"></param>
        public void SalvarEExcluirEmailsAssinatura(ClienteDto cliente, int? PROD_ID)
        {
            ExcluirClienteEmailAssinatura(cliente, PROD_ID);

            AssinaturaDTO assinatura = _assinaturaSRV.ExtrairAssinaturaCliente(cliente, PROD_ID);
            var lstTelefone = ExtrairEmailDaAssinaturaCliente(cliente, PROD_ID);
            //var lstTelefone = cliente.ASSINATURA_EMAIL;

            if (lstTelefone != null)
            {
                SalvarEmails(cliente, lstTelefone.AsQueryable(), assinatura);
            }
        }


        public List<AssinaturaEmailDTO> BuscarEmails(string _email)
        {
            return _dao.BuscarEmails(_email).ToList();
        }


        public void CopiarEmailsESalvar(ClienteDto cliente, ICollection<AssinaturaEmailDTO> emails)
        {
            ICollection<AssinaturaEmailDTO> lstEmailsNovos = new List<AssinaturaEmailDTO>();

            if (emails != null)
            {
                foreach (var email in emails)
                {
                    var objEmailNovo = email.Clone();
                    objEmailNovo.ASN_NUM_ASSINATURA = null;

                    if (objEmailNovo.CLI_ID == null)
                    {
                        objEmailNovo.CLI_ID = cliente.CLI_ID;
                    }

                    lstEmailsNovos.Add(objEmailNovo);
                }
                SaveOrUpdateAll(lstEmailsNovos);
            }
        }

        /// <summary>
        /// Pega a lista de Emails remove do cliente e associa a assinatura do cliente.
        /// </summary>
        /// <param name="assinatura"></param>
        /// <param name="emails"></param>
        public void CopiarEmailParaAssinaturaESalvar(AssinaturaDTO assinatura, ICollection<AssinaturaEmailDTO> emails, string USU_LOGIN)
        {
            AssinaturaEmailDTO emailNovo = null;

            var codAssinatura = assinatura.ASN_NUM_ASSINATURA;

            string assinaturaAnterior = emails.LastOrDefault().ASN_NUM_ASSINATURA;

            if (assinatura != null)
            {
                List<AssinaturaEmailDTO> emailsOriginais = emails.Where(x => x.ASN_NUM_ASSINATURA == assinaturaAnterior).Distinct().ToList();

                foreach (AssinaturaEmailDTO email in emailsOriginais)
                {

                    emailNovo = new AssinaturaEmailDTO();
                    emailNovo = email.Clone();

                    emailNovo.AEM_ID = null;
                    emailNovo.DATA_ALTERA = DateTime.Now;
                    emailNovo.USU_LOGIN = USU_LOGIN;

                    if (string.IsNullOrWhiteSpace(emailNovo.ASN_NUM_ASSINATURA) || emailNovo.ASN_NUM_ASSINATURA != codAssinatura)
                        emailNovo.ASN_NUM_ASSINATURA = codAssinatura;

                    new ClienteSRV()
                        .GravarHistorico(assinatura.UEN_ID, emailNovo.CLI_ID, assinaturaAnterior, "CopiarEmailParaAssinaturaESalvar");

                    Salvar(emailNovo);

                }
            }

                    //if ((assinatura != null) && (emails.Count > 0))
                    //{

                    //    var codAssinatura = assinatura.ASN_NUM_ASSINATURA;
                    //    AssinaturaEmailDTO email = emails.LastOrDefault();
                    //    var objEmailNovo = email.Clone();

                    //    objEmailNovo.AEM_ID = null;
                    //    objEmailNovo.DATA_ALTERA = DateTime.Now;
                    //    objEmailNovo.USU_LOGIN = USU_LOGIN;

                    //    if (string.IsNullOrWhiteSpace(objEmailNovo.ASN_NUM_ASSINATURA) || objEmailNovo.ASN_NUM_ASSINATURA != codAssinatura)
                    //        objEmailNovo.ASN_NUM_ASSINATURA = codAssinatura;

                    //    new ClienteSRV()
                    //        .GravarHistorico(assinatura.UEN_ID, objEmailNovo.CLI_ID, objEmailNovo.ASN_NUM_ASSINATURA, "CopiarEmailParaAssinaturaESalvar");

                    //    Salvar(objEmailNovo);

                    //}
        }

        /// <summary>
        /// Atualiza os telefones 
        /// </summary>
        /// <param name="lstEmailLegado"></param>
        public void SalvarReferenciaDoTelefoneLegado(IEnumerable<EmailsDTO> lstEmailLegado)
        {
            var lstEmails = new List<AssinaturaEmailDTO>();

            if (lstEmailLegado != null)
            {
                foreach (var emailLegado in lstEmailLegado)
                {
                    var idEmailCoadCorp = emailLegado.IdEmailCoadCorp;
                    var email = FindById(idEmailCoadCorp);
                    email.EMAIL_ID_LEGADO = emailLegado.AUTOID;

                    lstEmails.Add(email);
                }

                SaveOrUpdateAll(lstEmails);
            }
        }

        /// <summary>
        /// Retorna o primeiro e-mail encontrado do cliente ou de sua assinatura que seja válido.
        /// </summary>
        /// <param name="CLI_ID"></param>
        /// <returns></returns>
        public AssinaturaEmailDTO RetornarEmailDeContato(int? CLI_ID)
        {
            AssinaturaEmailDTO emailRet = null;
            var lstEmails = FindEmailsDoClienteEAssinaturaPorTipo(CLI_ID, 2);

            foreach(var email in lstEmails)
            {
                var validacao = ValidatorProxy.RecursiveValidate(email);
                if (validacao.IsValid)
                {
                    emailRet = email;
                }
            }

            if(emailRet == null)
            {
                lstEmails = FindEmailsDoClienteEAssinaturaPorTipo(CLI_ID, 5);
                foreach (var email in lstEmails)
                {
                    var validacao = ValidatorProxy.RecursiveValidate(email);
                    if (validacao.IsValid)
                    {
                        emailRet = email;
                    }
                }
            }

            if (emailRet == null)
            {
                lstEmails = FindEmailsDoClienteEAssinatura(CLI_ID);
                foreach (var email in lstEmails)
                {
                    var validacao = ValidatorProxy.RecursiveValidate(email);
                    if (validacao.IsValid)
                    {
                        emailRet = email;
                    }
                }
            }
            return emailRet;
        }

        public void SalvarEExcluirEmailsDeVariosClientes(IEnumerable<ClienteDto> lstClientes)
        {
            IList<AssinaturaEmailDTO> lstAssinaturaEmail = new List<AssinaturaEmailDTO>();

            if (lstClientes != null)
            {
                foreach (var cli in lstClientes)
                {
                    lstAssinaturaEmail = ProcessarEConcatenarAssinaturaEmail(cli, lstAssinaturaEmail);
                }

                BulkInsertOrMerge(lstAssinaturaEmail);
            }
        }

        public IList<AssinaturaEmailDTO> ProcessarEConcatenarAssinaturaEmail(ClienteDto cliente, IList<AssinaturaEmailDTO> listaAcumulada)
        {
            var listaEmail = cliente.ASSINATURA_EMAIL;
            if (listaEmail != null)
            {
                foreach (var assEmail in listaEmail)
                {
                    if (assEmail.CLI_ID == null)
                    {
                        assEmail.CLI_ID = cliente.CLI_ID;
                    }
                }

                listaAcumulada = listaAcumulada.Concat(listaEmail).ToList();
            }
            return listaAcumulada;
        }

        public AssinaturaEmailDTO FindPrimeiroEmailDoClienteOuAssinatura(int? CLI_ID)
        {
            return _dao.FindPrimeiroEmailDoClienteOuAssinatura(CLI_ID);
        }

        public bool ChecarClientePossuiEmail(string email, int? cliId)
        {
            return _dao.ChecarClientePossuiEmail(email, cliId);
        }
        public void AdicionarEmail(string email, int? cliId)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new Exception("Não é possível cadastrar o email. O E-Mail é nulla.");
            }
            if (cliId == null)
            {
                throw new Exception("Não é possível cadastrar o email. O código do cliente é nullo.");
            }
            if (!ChecarClientePossuiEmail(email, cliId))
            {                
                var assinaturaEmail = new AssinaturaEmailDTO()
                {
                    AEM_EMAIL = email,
                    CLI_ID = cliId,
                    OPC_ID = 2
                };

                Save(assinaturaEmail);
            }
        }

        public IList<AssinaturaEmailDTO> ListarDeTodasAsAssinaturas(int? cliId)
        {
            return _dao.ListarDeTodasAsAssinaturas(cliId);
        }

        public bool ChecarClientePossuiEmailAssinatura(string email, int? cliId)
        {
            return _dao.ChecarClientePossuiEmailAssinatura(email, cliId);
        }

        public IList<AssinaturaEmailDTO> FindEmailsDoClienteEAssinaturaPorTipo(int? CLI_ID, int? opcID = null)
        {
            return _dao.FindEmailsDoClienteEAssinaturaPorTipo(CLI_ID, opcID);
        }
    }
}
