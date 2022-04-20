using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.LEGADO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.SEGURANCA.Service;
using GenericCrud.Config.DataAttributes;
using GenericCrud.Models.Comparators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("ATE_ID")]
    public class AssinaturaTelefoneSRV : ServiceAdapter<ASSINATURA_TELEFONE, AssinaturaTelefoneDTO, int>
    {
        public AssinaturaTelDAO _dao = new AssinaturaTelDAO();
        private AssinaturaSRV _assinaturaSRV = new AssinaturaSRV();

        public AssinaturaTelefoneSRV()
        {
            SetDao(_dao);
        }
        public IList<AssinaturaTelefoneDTO> BuscarTelefonesRemovidos(string _assinatura, List<int> _telok)
        {
            return _dao.BuscarTelefonesRemovidos(_assinatura, _telok);
        }

        public ICollection<AssinaturaTelefoneDTO> FindAllByCliId(int CLI_ID)
        {
            return _dao.FindAllByCliId(CLI_ID);
        }


        private void _preencherChaves(int CLI_ID, IQueryable<AssinaturaTelefoneDTO> telefones)
        {
            if (telefones != null)
            {
                foreach (var tel in telefones)
                {
                    tel.CLI_ID = CLI_ID;
                }
            }
        }


        /// <summary>
        /// Pega a lista de telefones e associa ao cliente (diretamente ao cliente, não a assinatura)
        /// </summary>
        /// <param name="cliente"></param>
        /// <param name="telefones"></param>
        public void CopiarTelefonesESalvar(ClienteDto cliente, ICollection<AssinaturaTelefoneDTO> telefones)
        {
            ICollection<AssinaturaTelefoneDTO> lstTelefonesNovo = new List<AssinaturaTelefoneDTO>();

            if (telefones != null)
            {
                foreach (var telefone in telefones)
                {
                    var objTelefoneNovo = telefone.Clone();
                    objTelefoneNovo.ASN_NUM_ASSINATURA = null;

                    if (objTelefoneNovo.CLI_ID == null)
                    {
                        objTelefoneNovo.CLI_ID = cliente.CLI_ID;
                    }

                    lstTelefonesNovo.Add(objTelefoneNovo);
                }
                SaveOrUpdateAll(lstTelefonesNovo);
            }
        }

        /// <summary>
        /// Pega a lista de telefones remove do cliente e associa a assinatura do cliente.
        /// </summary>
        /// <param name="assinatura"></param>
        /// <param name="telefones"></param>
        public void CopiarTelefonesParaAssinaturaESalvar(AssinaturaDTO assinatura, ICollection<AssinaturaTelefoneDTO> telefones, string USU_LOGIN)
        {

            AssinaturaTelefoneDTO telefoneNovo = null;

            var codAssinatura = assinatura.ASN_NUM_ASSINATURA;

            string assinaturaAnterior = telefones.LastOrDefault().ASN_NUM_ASSINATURA;

            if (assinatura != null)
            {

                List<AssinaturaTelefoneDTO> telefonesOriginais = telefones.Where(x => x.ASN_NUM_ASSINATURA == assinaturaAnterior).Distinct().ToList();

                foreach (AssinaturaTelefoneDTO telefone in telefonesOriginais)
                {

                    if (telefone.TIPO_TEL_ID == null)
                        telefone.TIPO_TEL_ID = 4;

                    telefoneNovo = new AssinaturaTelefoneDTO();
                    telefoneNovo = telefone.Clone();

                    telefoneNovo.ATE_ID = null;
                    telefoneNovo.DATA_ALTERA = DateTime.Now;
                    telefoneNovo.USU_LOGIN = USU_LOGIN;
																																					 

                    if (string.IsNullOrWhiteSpace(telefoneNovo.ASN_NUM_ASSINATURA) || telefoneNovo.ASN_NUM_ASSINATURA != codAssinatura)
                        telefoneNovo.ASN_NUM_ASSINATURA = codAssinatura;

                    new ClienteSRV()
                        .GravarHistorico(assinatura.UEN_ID, telefoneNovo.CLI_ID, telefoneNovo.ASN_NUM_ASSINATURA, "CopiarTelefonesParaAssinaturaESalvar");

                    Salvar(telefoneNovo);

                }

            }

        }

        /// <summary>
        /// Salva os telefones de um determinado prospect
        /// </summary>
        /// <param name="CLI_ID">Id inserido no telefone antes de salvar</param>
        /// <param name="telefones">Os telefones para serem salvos</param>
        /// <param name="atualizar">true = atualizar, false = incluir</param>
        public void SalvarTelefones(ClienteDto cliente, IQueryable<AssinaturaTelefoneDTO> telefones)
        {
            if (telefones != null)
            {
                foreach (var tel in telefones)
                {
                    if (tel.CLI_ID == null)
                    {
                        tel.CLI_ID = cliente.CLI_ID;
                    }
                }
                SaveOrUpdateAll(telefones);

            }
        }

        /// <summary>
        /// Salva os telefones de um determinado prospect
        /// </summary>
        /// <param name="CLI_ID">Id inserido no telefone antes de salvar</param>
        /// <param name="telefones">Os telefones para serem salvos</param>
        /// <param name="atualizar">true = atualizar, false = incluir</param>
        public void SalvarTelefones(ClienteDto cliente, IQueryable<AssinaturaTelefoneDTO> telefones, AssinaturaDTO assinatura)
        {
            if (telefones != null)
            {
                foreach (var tel in telefones)
                {
                    if (tel.CLI_ID == null)
                    {
                        tel.CLI_ID = cliente.CLI_ID;
                    }

                    if (!string.IsNullOrWhiteSpace(tel.ASN_NUM_ASSINATURA))
                    {
                        tel.ASN_NUM_ASSINATURA = assinatura.ASN_NUM_ASSINATURA;
                    }

                }
                SaveOrUpdateAll(telefones);

            }
        }

        public void ExcluirClienteTelefone(ClienteDto cliente)
        {
            var CLI_ID = (int)cliente.CLI_ID;

            //AssinaturaDTO clientes = _assinaturaSRV.ExtrairAssinaturaFranquiaCliente(cliente);
            ClienteDto clienteDoBanco = new ClienteSRV().FindByIdFullLoaded(CLI_ID, false, true, false);

            ExcluirList<ClienteDto>(cliente, clienteDoBanco, "ASSINATURA_TELEFONE");

        }

        public void ExcluirClienteTelefoneAssinatura(ClienteDto cliente, int? PROD_ID)
        {
            var CLI_ID = (int)cliente.CLI_ID;

            AssinaturaDTO assinatura = _assinaturaSRV.ExtrairAssinaturaCliente(cliente, PROD_ID);
            string codigoAssinatura = assinatura.ASN_NUM_ASSINATURA;

            AssinaturaDTO assinaturaDoBanco = _assinaturaSRV.FindByIdFullLoaded(codigoAssinatura, true, false);
            ExcluirList<AssinaturaDTO>(assinatura, assinaturaDoBanco, "ASSINATURA_TELEFONE");

        }

        /// <summary>
        /// Atualiza os telefones e remove dos bancos os que foram excluidos da lista
        /// </summary>
        /// <param name="cliente"></param>
        public void SalvarEExcluirTelefones(ClienteDto cliente)
        {
            ExcluirClienteTelefone(cliente);

            //AssinaturaDTO clientes = _assinaturaSRV.ExtrairAssinaturaFranquiaCliente(cliente);            
            // var lstTelefone = ExtrairTelefoneDaAssinaturaFranquiaCliente(cliente);
            var lstTelefone = cliente.ASSINATURA_TELEFONE;

            if (lstTelefone != null)
            {
                SalvarTelefones(cliente, lstTelefone.AsQueryable());
            }
        }

        /// <summary>
        /// Atualiza os telefones e remove dos bancos os que foram excluidos da lista
        /// </summary>
        /// <param name="cliente"></param>
        public void SalvarEExcluirTelefonesAssinatura(ClienteDto cliente, int? PROD_ID)
        {
            ExcluirClienteTelefone(cliente);

            var lstTelefone = ExtrairTelefoneDaAssinaturaCliente(cliente, PROD_ID);
            AssinaturaDTO assinaturaFranquia = _assinaturaSRV.ExtrairAssinaturaCliente(cliente, PROD_ID);

            if (lstTelefone != null)
            {
                SalvarTelefones(cliente, lstTelefone.AsQueryable(), assinaturaFranquia);
            }
        }

        public ICollection<AssinaturaTelefoneDTO> ExtrairTelefoneDaAssinaturaCliente(ClienteDto cliente, int? PRO_ID, bool carregarDoBancoCasoNaoDisponivel = false)
        {
            if (cliente != null)
            {
                AssinaturaDTO assinatura = _assinaturaSRV.ExtrairAssinaturaCliente(cliente, PRO_ID, carregarDoBancoCasoNaoDisponivel, true);

                if (assinatura != null)
                {
                    ICollection<AssinaturaTelefoneDTO> assinaturaTelefone = assinatura.ASSINATURA_TELEFONE;

                    return assinaturaTelefone;
                }
            }
            return null;
        }
        public ICollection<AssinaturaTelefoneDTO> ExtrairTelefoneDaAssinaturaFranquiaCliente(ClienteDto cliente, bool carregarDoBancoCasoNaoDisponivel = false)
        {
            if (cliente != null)
            {
                AssinaturaDTO assinatura = _assinaturaSRV.ExtrairAssinaturaFranquiaCliente(cliente, carregarDoBancoCasoNaoDisponivel, true);

                if (assinatura != null)
                {
                    ICollection<AssinaturaTelefoneDTO> assinaturaTelefone = assinatura.ASSINATURA_TELEFONE;

                    return assinaturaTelefone;
                }
            }
            return null;
        }

        public IList<AssinaturaTelefoneDTO> FindByNumAssinatura(string codigoAssinatura)
        {
            return _dao.FindByNumAssinatura(codigoAssinatura);
        }

        public void PreencherTelefoneAssinaturaNaAssinatura(AssinaturaDTO assinatura)
        {
            if (assinatura != null && !string.IsNullOrWhiteSpace(assinatura.ASN_NUM_ASSINATURA))
            {
                IList<AssinaturaTelefoneDTO> lstAssinaturaTelefone = FindByNumAssinatura(assinatura.ASN_NUM_ASSINATURA);
                assinatura.ASSINATURA_TELEFONE = lstAssinaturaTelefone;
            }
        }

        public IList<AssinaturaTelefoneDTO> FindByCliente(int? CLI_ID)
        {
            return _dao.FindByCliente(CLI_ID);
        }

        public void PreencherTelefoneAssinaturaNoCliente(ClienteDto cliente)
        {
            if (cliente != null && cliente.CLI_ID != null)
            {
                IList<AssinaturaTelefoneDTO> lstAssinaturaTelefone = FindByCliente(cliente.CLI_ID);
                cliente.ASSINATURA_TELEFONE = lstAssinaturaTelefone;
            }
        }
        public List<AssinaturaTelefoneDTO> BuscarTelefone(string _telefone)
        {
            return _dao.BuscarTelefone(_telefone).ToList();
        }

        public AssinaturaTelefoneDTO BuscarPorAssinatura(string _assinatura)
        {
            return _dao.BuscarPorAssinatura(_assinatura);
        }

        public IList<AssinaturaTelefoneDTO> BuscarTelefonesURA(string _assinatura)
        {
            return _dao.BuscarTelefonesURA(_assinatura);
        }

        /// <summary>
        /// Atualiza os telefones 
        /// </summary>
        /// <param name="lstTelefoneLegado"></param>
        public void SalvarReferenciaDoTelefoneLegado(IEnumerable<Telefones2DTO> lstTelefoneLegado)
        {
            var lstTelefones = new List<AssinaturaTelefoneDTO>();

            if (lstTelefoneLegado != null)
            {
                foreach (var tel in lstTelefoneLegado)
                {
                    var telefone = base.FindById(id: (int?)tel.IdTelCoadCorp);
                    telefone.TEL_ID_LEGADO = tel.id;

                    new ClienteSRV()
                      .GravarHistorico(null, telefone.CLI_ID, telefone.ASN_NUM_ASSINATURA, "SalvarReferenciaDoTelefoneLegado");										  

                    lstTelefones.Add(telefone);
                }

                SaveOrUpdateAll(lstTelefones);
            }
        }

        public IList<AssinaturaTelefoneDTO> ProcessarEConcatenarAssinaturaTelefone(ClienteDto cliente, IList<AssinaturaTelefoneDTO> listaAcumulada)
        {
            var listaTelefone = cliente.ASSINATURA_TELEFONE;

            foreach (var assTel in listaTelefone)
            {
                if (assTel.CLI_ID == null)
                {
                    assTel.CLI_ID = cliente.CLI_ID;
                }
            }

            listaAcumulada = listaAcumulada.Concat(listaTelefone).ToList();
            return listaAcumulada;
        }

        public void SalvarEExcluirTelefonesDeVariosClientes(IEnumerable<ClienteDto> lstClientes)
        {
            IList<AssinaturaTelefoneDTO> lstTelefones = new List<AssinaturaTelefoneDTO>();

            if (lstTelefones != null)
            {
                foreach (var cli in lstClientes)
                {
                    lstTelefones = ProcessarEConcatenarAssinaturaTelefone(cli, lstTelefones);
                }

                BulkInsertOrMerge(lstTelefones);
            }
        }


        /// <summary>
        /// Retorna o primeiro telefone encontrado do cliente, tanto pelo cliente quanto pela assinatura.
        /// </summary>
        /// <returns></returns>
        public AssinaturaTelefoneDTO RetornarTelefoneContato(int? CLI_ID)
        {
            return FindPrimeiroTelefoneDoClienteOuAssinatura(CLI_ID);
        }

        public AssinaturaTelefoneDTO FindPrimeiroTelefoneDoClienteOuAssinatura(int? CLI_ID)
        {
            return _dao.FindPrimeiroTelefoneDoClienteOuAssinatura(CLI_ID);
        }

        public IList<AssinaturaTelefoneDTO> ListarDeTodasAsAssinaturas(int? cliId)
        {
            return _dao.ListarDeTodasAsAssinaturas(cliId);
        }
    }
}
