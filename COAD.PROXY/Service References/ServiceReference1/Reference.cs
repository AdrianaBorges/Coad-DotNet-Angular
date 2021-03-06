//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace COAD.PROXY.ServiceReference1 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference1.ICoadService")]
    public interface ICoadService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICoadService/ValidarCliente", ReplyAction="http://tempuri.org/ICoadService/ValidarClienteResponse")]
        string ValidarCliente(string usuario, string senha);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICoadService/ValidarCliente", ReplyAction="http://tempuri.org/ICoadService/ValidarClienteResponse")]
        System.Threading.Tasks.Task<string> ValidarClienteAsync(string usuario, string senha);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICoadService/GerarPedido", ReplyAction="http://tempuri.org/ICoadService/GerarPedidoResponse")]
        COAD.CORPORATIVO.Model.Dto.Custons.DadosPedidoIntegracaoDTO GerarPedido(COAD.CORPORATIVO.Model.Dto.Custons.DadosPedidoIntegracaoDTO composite);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICoadService/GerarPedido", ReplyAction="http://tempuri.org/ICoadService/GerarPedidoResponse")]
        System.Threading.Tasks.Task<COAD.CORPORATIVO.Model.Dto.Custons.DadosPedidoIntegracaoDTO> GerarPedidoAsync(COAD.CORPORATIVO.Model.Dto.Custons.DadosPedidoIntegracaoDTO composite);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICoadService/LembrarSenha", ReplyAction="http://tempuri.org/ICoadService/LembrarSenhaResponse")]
        string LembrarSenha(string numAssinatura, int cliId, string email);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICoadService/LembrarSenha", ReplyAction="http://tempuri.org/ICoadService/LembrarSenhaResponse")]
        System.Threading.Tasks.Task<string> LembrarSenhaAsync(string numAssinatura, int cliId, string email);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ICoadServiceChannel : COAD.PROXY.ServiceReference1.ICoadService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CoadServiceClient : System.ServiceModel.ClientBase<COAD.PROXY.ServiceReference1.ICoadService>, COAD.PROXY.ServiceReference1.ICoadService {
        
        public CoadServiceClient() {
        }
        
        public CoadServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public CoadServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CoadServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CoadServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string ValidarCliente(string usuario, string senha) {
            return base.Channel.ValidarCliente(usuario, senha);
        }
        
        public System.Threading.Tasks.Task<string> ValidarClienteAsync(string usuario, string senha) {
            return base.Channel.ValidarClienteAsync(usuario, senha);
        }

        public COAD.CORPORATIVO.Model.Dto.Custons.DadosPedidoIntegracaoDTO GerarPedido(COAD.CORPORATIVO.Model.Dto.Custons.DadosPedidoIntegracaoDTO composite)
        {
            return base.Channel.GerarPedido(composite);
        }

        public System.Threading.Tasks.Task<COAD.CORPORATIVO.Model.Dto.Custons.DadosPedidoIntegracaoDTO> GerarPedidoAsync(COAD.CORPORATIVO.Model.Dto.Custons.DadosPedidoIntegracaoDTO composite)
        {
            return base.Channel.GerarPedidoAsync(composite);
        }
        
        public string LembrarSenha(string numAssinatura, int cliId, string email) {
            return base.Channel.LembrarSenha(numAssinatura, cliId, email);
        }
        
        public System.Threading.Tasks.Task<string> LembrarSenhaAsync(string numAssinatura, int cliId, string email) {
            return base.Channel.LembrarSenhaAsync(numAssinatura, cliId, email);
        }
    }
}
