using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Model.Dto.Custons.WebService;
using COAD.CORPORATIVO.Model.Dto.Custons.WebService.ClienteIntegracao;
using COAD.CORPORATIVO.Model.Dto.Custons.WebService.ImportacaoSuspect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace COADSERVICE
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface ICoadService
    {

        [OperationContract]
        string ValidarCliente(string usuario, string senha);

        [OperationContract]
        DadosPedidoIntegracaoDTO GerarPedido(DadosPedidoIntegracaoDTO composite);

        [OperationContract]
        string LembrarSenha(string numAssinatura, int cliId, string email);

        [OperationContract]
        string CheckoutPagamento(CartaoIntegracaoDTO dadospagamento);

        [OperationContract]
        RenovacaoMalaResult EmitirPedidoRenovacaoDaMala(string numeroAssinatura, decimal valorPedido, int qtdParcelas);

        [OperationContract]
        LoginUnicoResultDTO RealizarLogin(string code, string encodecode);

        [OperationContract]
        ClienteIntegrResult RetornarDadosDoCliente(string numeroAssinatura, string senha);

        [OperationContract]
        ValidatorWebServiceResult SalvarDadosDoCliente(ClienteIntegrDTO cliente);

        [OperationContract]
        BuscarCEPResult BuscarCep(string cep);

        [OperationContract]
        BuscarMunicipioResult BuscarMunicipios(string uf, string descricao = null);

        [OperationContract]
        BuscarMunicipioResult BuscarMunicipiosPorCodIBGE(string codIBGE);

        [OperationContract]
        TipoClienteResult ListarTiposDeCliente();

        [OperationContract]
        BuscarDadosClienteStResult BuscarDadosClienteSt(string assinatura, string senha);

    }

    [ServiceContract]
    public interface ICoadServiceRest
    {

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "teste")]
        string[] teste();

    }


}
