using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.CORPORATIVO.Service;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.SEGURANCA.Service;
using COAD.CORPORATIVO.Model.Dto;

namespace COAD.CORPORATIVO.Service
{
    public class DadosClientePortalSRV
    {

        public AssinaturaSRV _assinaturaSRV { get; set; }
        public AssinaturaEmailSRV _assinaturaEmailSRV { get; set; }
        public AssinaturaTelefoneSRV _assinaturaTelefoneSRV { get; set; }
        public AssinaturaSenhaSRV _assinaturaSenhaSRV { get; set; }
        public ClienteSRV _clienteSRV { get; set; }
        public ClienteEnderecoSRV _clienteEnderecoSRV { get; set; }
        public ContratoSRV _contratoSRV { get; set; }
        public ProdutoComposicaoItemSRV _produtoComposicaoItemSRV { get; set; }
        public ProdutoComposicaoSRV _produtoComposicaoSRV { get; set; }
        public ProdutosSRV _produtosSRV { get; set; }
        public ItemPedidoSRV _itemPedidoSRV { get; set; }

        public DadosClienteStDTO BuscarDadosClienteSt(string assinatura, string senha)
        {

            var dadosContrato = _contratoSRV.BuscarUltimoContratoValido(assinatura);
            if (dadosContrato == null)
            {
                throw new Exception("Cliente sem contrato ativo. Favor entrar em contato com o SAC.");
            }

            var dadosSenha = _assinaturaSenhaSRV.BuscarSenhaAtiva(dadosContrato.ASN_NUM_ASSINATURA);
            if (!dadosSenha.ASN_SENHA.Equals(senha))
            {
                throw new Exception("Assinatura ou senha inválida.");
            }

            var dadosItemPedido = _itemPedidoSRV.BuscarPorId(dadosContrato.IPE_ID);
            if (dadosItemPedido == null)
            {
                throw new Exception("Cliente com pedido em processo para ser aprovado. Favor entrar em contato com o SAC para mais informações.");
            }

            if(dadosItemPedido.CMP_ID == null)
            {
                throw new Exception("Cliente sem produto cadastrado. Favor entrar em contato com o SAC para mais informações.");
            }

            var dadosAssinatura = _assinaturaSRV.FindById(dadosContrato.ASN_NUM_ASSINATURA);
            var dadosCliente = _clienteSRV.FindById(dadosAssinatura.CLI_ID);
            var dadosEmail = _assinaturaEmailSRV.RetornarEmailDeContato(dadosAssinatura.CLI_ID);
            var dadosTelefone = _assinaturaTelefoneSRV.FindPrimeiroTelefoneDoClienteOuAssinatura(dadosAssinatura.CLI_ID);
            var dadosEndereco = _clienteEnderecoSRV.FindEnderecoCliente(dadosAssinatura.CLI_ID, 1);
            var dadosProdComp = _produtoComposicaoSRV.FindById(dadosItemPedido.CMP_ID);

            var dadosProdCmpItem = _produtoComposicaoItemSRV.BuscaProdutoComposicaoItemPorComposicaoId(dadosItemPedido.CMP_ID);

            var dadosProdCmpCalcIcms = dadosProdCmpItem.Where(p => p.PRODUTOS.TPC_ID == 3).FirstOrDefault();
            var dadosProdCmpConsAliqMva = dadosProdCmpItem.Where(p => p.PRODUTOS.TPC_ID == 4).FirstOrDefault();
            var dadosProdCmpConsNcmCest = dadosProdCmpItem.Where(p => p.PRODUTOS.TPC_ID == 5).FirstOrDefault();
            var dadosProdCmpMonitNcm = dadosProdCmpItem.Where(p => p.PRODUTOS.TPC_ID == 6).FirstOrDefault();
            var dadosProdCmpSupOn = dadosProdCmpItem.Where(p => p.PRODUTOS.TPC_ID == 7).FirstOrDefault();
            var dadosProdCmpQuantUsuarioLogado = dadosProdCmpItem.Where(p => p.PRODUTOS.TPC_ID == 8).FirstOrDefault();
            var dadosNumeroPlano = dadosProdCmpItem.Where(p => p.PRODUTOS.TPC_ID == 9).FirstOrDefault();

            var dadosClienteStDTO = new DadosClienteStDTO();
            dadosClienteStDTO.login = dadosContrato.ASN_NUM_ASSINATURA;
            dadosClienteStDTO.senha = dadosSenha.ASN_SENHA;
            if (dadosCliente != null)
            {
                dadosClienteStDTO.nome = dadosCliente.CLI_NOME;
                dadosClienteStDTO.cpf = dadosCliente.CLI_CPF_CNPJ;
                dadosClienteStDTO.tipoUsuario = dadosCliente.CLI_TP_PESSOA;
            }
            if (dadosEmail != null) { 
                dadosClienteStDTO.email = dadosEmail.AEM_EMAIL;
            }
            if (dadosTelefone != null) { 
                dadosClienteStDTO.telefone = dadosTelefone.ATE_TELEFONE;
            }
            if(dadosEndereco != null)
            {
                dadosClienteStDTO.logradouro = dadosEndereco.END_LOGRADOURO;
                dadosClienteStDTO.numero = dadosEndereco.END_NUMERO;
                dadosClienteStDTO.complemento = dadosEndereco.END_COMPLEMENTO;
                dadosClienteStDTO.bairro = dadosEndereco.END_BAIRRO;
                dadosClienteStDTO.cidade = dadosEndereco.END_MUNICIPIO;
                dadosClienteStDTO.estado = dadosEndereco.END_UF;
                dadosClienteStDTO.cep = dadosEndereco.END_CEP;
            }
            dadosClienteStDTO.dataExpiracao = dadosContrato.CTR_DATA_FIM_VIGENCIA;
            dadosClienteStDTO.createdTime = dadosContrato.CTR_DATA_INI_VIGENCIA;

            //Pegar do CMI (PRODUTO_COMPOSICAO_ITEM). Atualmente pega do produto a quantidade.
            //Fazer uma verificação para que, caso não tenha o valor seja de zero.
            
            if(dadosProdCmpQuantUsuarioLogado != null)
            {
                if(dadosProdCmpQuantUsuarioLogado.CMI_QTDE!=null && dadosProdCmpQuantUsuarioLogado.CMI_QTDE != 0)
                {
                    dadosClienteStDTO.quantidadeSessoes = dadosProdCmpQuantUsuarioLogado.CMI_QTDE;
                }
                else if (dadosProdCmpQuantUsuarioLogado.CMI_QTDE_PERIODO != null && dadosProdCmpQuantUsuarioLogado.CMI_QTDE_PERIODO != 0)
                {
                    dadosClienteStDTO.quantidadeSessoes = dadosProdCmpQuantUsuarioLogado.CMI_QTDE_PERIODO;
                }
                else
                {
                    dadosClienteStDTO.quantidadeSessoes = 1;
                }
                
            }
            if (dadosProdComp != null)
            {
                dadosClienteStDTO.planoNome = dadosProdComp.CMP_DESCRICAO;
            }
            if (dadosProdCmpCalcIcms != null)
            {
                if (dadosProdCmpCalcIcms.CMI_QTDE_PERIODO != null && dadosProdCmpCalcIcms.CMI_QTDE_PERIODO != 0) 
                {
                    dadosClienteStDTO.quantidadeCalculoIcmsSt = dadosProdCmpCalcIcms.CMI_QTDE_PERIODO;
                }
                else if (dadosProdCmpCalcIcms.CMI_QTDE != null && dadosProdCmpCalcIcms.CMI_QTDE != 0)
                {
                    dadosClienteStDTO.quantidadeCalculoIcmsSt = dadosProdCmpCalcIcms.CMI_QTDE;
                }
                else
                {
                    dadosClienteStDTO.quantidadeCalculoIcmsSt = 0;
                }
            }
            if(dadosProdCmpConsAliqMva != null)
            {
                if (dadosProdCmpConsAliqMva.CMI_QTDE_PERIODO != null && dadosProdCmpConsAliqMva.CMI_QTDE_PERIODO != 0)
                {
                    dadosClienteStDTO.quantidadeConsultaAliquotaMva = dadosProdCmpConsAliqMva.CMI_QTDE_PERIODO;
                }
                else if (dadosProdCmpConsAliqMva.CMI_QTDE != null && dadosProdCmpConsAliqMva.CMI_QTDE != 0)
                {
                    dadosClienteStDTO.quantidadeConsultaAliquotaMva = dadosProdCmpConsAliqMva.CMI_QTDE;
                }
                else
                {
                    dadosClienteStDTO.quantidadeConsultaAliquotaMva = 0;
                }
            }
            if(dadosProdCmpMonitNcm != null)
            {
                if (dadosProdCmpMonitNcm.CMI_QTDE != null && dadosProdCmpMonitNcm.CMI_QTDE != 0)
                {
                    dadosClienteStDTO.quantidadeMonitoramentoNcm = dadosProdCmpMonitNcm.CMI_QTDE;
                }
                else if (dadosProdCmpMonitNcm.CMI_QTDE_PERIODO != null && dadosProdCmpMonitNcm.CMI_QTDE_PERIODO != 0)
                {
                    dadosClienteStDTO.quantidadeMonitoramentoNcm = dadosProdCmpMonitNcm.CMI_QTDE_PERIODO;
                }
                else
                {
                    dadosClienteStDTO.quantidadeMonitoramentoNcm = 0;
                }
            }
            if(dadosProdCmpSupOn != null)
            {
                if (dadosProdCmpSupOn.CMI_QTDE_PERIODO != null && dadosProdCmpSupOn.CMI_QTDE_PERIODO != 0)
                {
                    dadosClienteStDTO.acessoSuporteChat = dadosProdCmpSupOn.CMI_QTDE_PERIODO;
                }
                else if (dadosProdCmpSupOn.CMI_QTDE != null && dadosProdCmpSupOn.CMI_QTDE != 0)
                {
                    dadosClienteStDTO.acessoSuporteChat = dadosProdCmpSupOn.CMI_QTDE;
                }
                else
                {
                    dadosClienteStDTO.acessoSuporteChat = 0;
                }
            }
            if(dadosNumeroPlano != null)
            {
                if (dadosNumeroPlano.CMI_QTDE != null && dadosNumeroPlano.CMI_QTDE != 0)
                {
                    dadosClienteStDTO.perfil = dadosNumeroPlano.CMI_QTDE; 
                }
                else if (dadosNumeroPlano.CMI_QTDE_PERIODO != null && dadosNumeroPlano.CMI_QTDE_PERIODO != 0)
                {
                    dadosClienteStDTO.perfil = dadosNumeroPlano.CMI_QTDE_PERIODO;
                }
                else
                {
                    dadosClienteStDTO.perfil = 1;
                }
            }
            
            //Ainda não serão usados, estes dois. Futuramente alterar aqui para que estes possam ser usados.
            dadosClienteStDTO.acessoTraxo = true;
            dadosClienteStDTO.quantidadeConsultaEmail = 999;

            return dadosClienteStDTO;

        }

        public DadosClientePortalDTO BuscarDadosClienteContrato(string assinatura, string senha, string produtosId)
        {

            var dadosContrato = _contratoSRV.BuscarUltimoContratoValido(assinatura);
            if (dadosContrato == null)
            {
                throw new Exception("Cliente sem contrato ativo. Favor entrar em contato com o SAC.");
            }

            var dadosSenha = _assinaturaSenhaSRV.BuscarSenhaAtiva(dadosContrato.ASN_NUM_ASSINATURA);
            if (!dadosSenha.ASN_SENHA.Equals(senha))
            {
                throw new Exception("Assinatura ou senha inválida.");
            }

            var dadosItemPedido = _itemPedidoSRV.BuscarPorId(dadosContrato.IPE_ID);
            if (dadosItemPedido == null)
            {
                throw new Exception("Cliente com pedido em processo para ser aprovado. Favor entrar em contato com o SAC para mais informações.");
            }

            if (dadosItemPedido.CMP_ID == null)
            {
                throw new Exception("Cliente sem produto cadastrado. Favor entrar em contato com o SAC para mais informações.");
            }

            var dadosAssinatura = _assinaturaSRV.FindById(dadosContrato.ASN_NUM_ASSINATURA);

            if (!produtosId.Equals(assinatura.Substring(0, 2))){
                throw new Exception("Assinatura informada não dá suporte a este produto.");
            }

            var dadosCliente = _clienteSRV.FindById(dadosAssinatura.CLI_ID);
            var dadosEmail = _assinaturaEmailSRV.RetornarEmailDeContato(dadosAssinatura.CLI_ID);
            var dadosTelefone = _assinaturaTelefoneSRV.FindPrimeiroTelefoneDoClienteOuAssinatura(dadosAssinatura.CLI_ID);
            var dadosEndereco = _clienteEnderecoSRV.FindEnderecoCliente(dadosAssinatura.CLI_ID, 1);

            var clientePortalDTO = new DadosClientePortalDTO();

            clientePortalDTO.login = dadosContrato.ASN_NUM_ASSINATURA;
            clientePortalDTO.senha = dadosSenha.ASN_SENHA;
            if (dadosCliente != null)
            {
                clientePortalDTO.nome = dadosCliente.CLI_NOME;
                clientePortalDTO.cpf = dadosCliente.CLI_CPF_CNPJ;
                clientePortalDTO.tipoUsuario = dadosCliente.CLI_TP_PESSOA;
            }
            if (dadosEmail != null)
            {
                clientePortalDTO.email = dadosEmail.AEM_EMAIL;
            }
            if (dadosTelefone != null)
            {
                clientePortalDTO.telefone = dadosTelefone.ATE_TELEFONE;
            }
            if (dadosEndereco != null)
            {
                clientePortalDTO.logradouro = dadosEndereco.END_LOGRADOURO;
                clientePortalDTO.numero = dadosEndereco.END_NUMERO;
                clientePortalDTO.complemento = dadosEndereco.END_COMPLEMENTO;
                clientePortalDTO.bairro = dadosEndereco.END_BAIRRO;
                clientePortalDTO.cidade = dadosEndereco.END_MUNICIPIO;
                clientePortalDTO.estado = dadosEndereco.END_UF;
                clientePortalDTO.cep = dadosEndereco.END_CEP;
            }
            
            return clientePortalDTO;

        }

    }
}
