using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons.Mundipagg;
using COAD.CORPORATIVO.Service.Base;
using COAD.CORPORATIVO.Service.Custons;
using COAD.CORPORATIVO.Settings.Mundipagg;
using COAD.SEGURANCA.Repositorios.Base;
using MundiAPI.PCL;
using MundiAPI.PCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service.Mundipagg
{
    public class AssinaturaMundipaggSRV : MundipaggSRV
    {
        public override MundipaggStt MundipaggSTT { get; set; }
        private ClienteIntegracaoSRV _clienteIntegracaoSRV { get; set; }
        private ClienteMundipaggSRV _clienteMundipaggSRV { get; set; }
        private ClienteSRV _clienteSRV { get; set; }
        private PropostaSRV _propostaSRV { get; set; }
        private MundipaggAssinaturaSRV _mundipaggAssinaturaSRV { get; set; }

        private MundipaggClienteSRV _mundipaggClienteSRV { get; set; }

        public AssinaturaMundipaggSRV(ClienteIntegracaoSRV clienteIntegracaoSRV, MundipaggAssinaturaSRV mundipaggAssinaturaSRV, ClienteMundipaggSRV clienteMundipaggSRV, ClienteSRV clienteSRV, MundipaggClienteSRV mundipaggClienteSRV, PropostaSRV propostaSRV)
        {
            this._clienteIntegracaoSRV = clienteIntegracaoSRV;
            this._clienteMundipaggSRV = clienteMundipaggSRV;
            this._clienteSRV = clienteSRV;
            this._mundipaggClienteSRV = mundipaggClienteSRV;
            this._propostaSRV = propostaSRV;
            this._mundipaggAssinaturaSRV = mundipaggAssinaturaSRV;

        }

        public BuscarAssinaturaMundipaggResponseDTO CriarAssinatura(CriarAssinaturaMundipaggRequestDTO criarAssinaturaRequestDTO)
        {
            if (criarAssinaturaRequestDTO.CreateSubscriptionRequest == null)
            {
                throw new Exception("Informar dados para o pagamento.");
            }
            if (criarAssinaturaRequestDTO.CreateSubscriptionRequest.PlanId == null)
            {
                throw new Exception("Informar dados do plano.");
            }
            if (!criarAssinaturaRequestDTO.CreateSubscriptionRequest.PaymentMethod.Equals("credit_card"))
            {
                throw new Exception("Informar o método de pagamento.");
            }
            if (criarAssinaturaRequestDTO.ClienteMundipaggDTO == null)
            {
                throw new Exception("Informar dados do cliente.");
            }

            MundipaggClienteDTO mundipaggClienteDTO = null;
            if (criarAssinaturaRequestDTO.ClienteMundipaggDTO.ClientId != null)
            {
                mundipaggClienteDTO = _mundipaggClienteSRV.BuscarMundipaggClientePorCliId(criarAssinaturaRequestDTO.ClienteMundipaggDTO.ClientId);
            }

            if (mundipaggClienteDTO != null)
            {
                criarAssinaturaRequestDTO.CreateSubscriptionRequest.CustomerId = mundipaggClienteDTO.MPG_CLI_CUS_ID;
            }
            else
            {
                var criarClienteMundipaggRequestDTO = new CriarClienteMundipaggRequestDTO();
                criarClienteMundipaggRequestDTO.ClienteMundipaggDTO = criarAssinaturaRequestDTO.ClienteMundipaggDTO;
                _clienteMundipaggSRV.MundipaggSTT = MundipaggSTT;
                var buscarClienteMundipaggResponseDTO = _clienteMundipaggSRV.CadastrarClienteMundipagg(criarClienteMundipaggRequestDTO);
                criarAssinaturaRequestDTO.CreateSubscriptionRequest.CustomerId = buscarClienteMundipaggResponseDTO.GetCustomerResponse.Id;
                mundipaggClienteDTO = _mundipaggClienteSRV.BuscarMundipaggClientePorCusId(criarAssinaturaRequestDTO.CreateSubscriptionRequest.CustomerId);
            }

            var mundipaggApiCliente = new MundiAPIClient(MundipaggSTT.SecretKey, "");
            var getSubscriptionResponse = mundipaggApiCliente.Subscriptions.CreateSubscription(criarAssinaturaRequestDTO.CreateSubscriptionRequest);

            var mundipaggAssinaturaDTO =  new MundipaggAssinaturaDTO();
            mundipaggAssinaturaDTO.MPG_ASN_SUB_ID = getSubscriptionResponse.Id;
            mundipaggAssinaturaDTO.MPG_CLI_ID = mundipaggClienteDTO.MPG_CLI_ID;
            _mundipaggAssinaturaSRV.Save(mundipaggAssinaturaDTO);

            var buscarAssinaturaMundipaggResponseDTO = new BuscarAssinaturaMundipaggResponseDTO();
            buscarAssinaturaMundipaggResponseDTO.GetSubscriptionResponse = getSubscriptionResponse;

            //Gerar proposta
            PropostaDTO propostaVendaNovaOnline = new PropostaDTO();
            propostaVendaNovaOnline.TPP_ID = 1;
            propostaVendaNovaOnline.CLI_ID = mundipaggClienteDTO.CLI_ID;
            propostaVendaNovaOnline.PST_ID = 1;
            propostaVendaNovaOnline.RG_ID = 2;
            propostaVendaNovaOnline.REP_ID = 2601;
            propostaVendaNovaOnline.USU_LOGIN = SessionContext.login;
            propostaVendaNovaOnline.DATA_CADASTRO = DateTime.Now;
            //propostaVendaNovaOnline.PRT_VALOR_UNITARIO = buscarAssinaturaMundipaggResponseDTO.GetSubscriptionResponse

            /*propostaDTO.
            propostaDTO.CMP_ID = 
CLI_ID
PST_ID = 9
RG_ID = 12
REP = ??
USU_LOGIN = ??
DATA_CADASTRO = ??
DATA_CONFIRMACAO_PAGAMENTO = null
PRT_VALOR_UNITARIO = VALOR DO PRODUTO MENSAL
PRT_QTD_PARCELAS = 1
PRT_VALOR_TOTAL
EMP_ID = ???
PRT_OBSERVACOES = Esta compra foi efetuada pela mundipagg
REP_ID_EMINENTE = null ??
CAR_ID = null
PRT_EMAIL_CONTATO
TNE_ID = 1*/

            return buscarAssinaturaMundipaggResponseDTO;
        }
    }
}
