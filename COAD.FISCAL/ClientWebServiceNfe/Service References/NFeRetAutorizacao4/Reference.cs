﻿//------------------------------------------------------------------------------
// <auto-generated>
//     O código foi gerado por uma ferramenta.
//     Versão de Tempo de Execução:4.0.30319.42000
//
//     As alterações ao arquivo poderão causar comportamento incorreto e serão perdidas se
//     o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace COAD.FISCAL.NFeRetAutorizacao4 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NFeRetAutorizacao4", ConfigurationName="NFeRetAutorizacao4.NFeRetAutorizacao4Soap")]
    public interface NFeRetAutorizacao4Soap {
        
        // CODEGEN: Gerando contrato de mensagem porque a operação nfeRetAutorizacaoLote não é RPC nem documento codificado.
        [System.ServiceModel.OperationContractAttribute(Action="http://www.portalfiscal.inf.br/nfe/wsdl/NFeRetAutorizacao4/nfeRetAutorizacaoLote", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        COAD.FISCAL.NFeRetAutorizacao4.nfeRetAutorizacaoLoteResponse nfeRetAutorizacaoLote(COAD.FISCAL.NFeRetAutorizacao4.nfeRetAutorizacaoLoteRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.portalfiscal.inf.br/nfe/wsdl/NFeRetAutorizacao4/nfeRetAutorizacaoLote", ReplyAction="*")]
        System.Threading.Tasks.Task<COAD.FISCAL.NFeRetAutorizacao4.nfeRetAutorizacaoLoteResponse> nfeRetAutorizacaoLoteAsync(COAD.FISCAL.NFeRetAutorizacao4.nfeRetAutorizacaoLoteRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class nfeRetAutorizacaoLoteRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NFeRetAutorizacao4", Order=0)]
        public System.Xml.XmlNode nfeDadosMsg;
        
        public nfeRetAutorizacaoLoteRequest() {
        }
        
        public nfeRetAutorizacaoLoteRequest(System.Xml.XmlNode nfeDadosMsg) {
            this.nfeDadosMsg = nfeDadosMsg;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class nfeRetAutorizacaoLoteResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NFeRetAutorizacao4", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Xml.XmlNode nfeResultMsg;
        
        public nfeRetAutorizacaoLoteResponse() {
        }
        
        public nfeRetAutorizacaoLoteResponse(System.Xml.XmlNode nfeResultMsg) {
            this.nfeResultMsg = nfeResultMsg;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface NFeRetAutorizacao4SoapChannel : COAD.FISCAL.NFeRetAutorizacao4.NFeRetAutorizacao4Soap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class NFeRetAutorizacao4SoapClient : System.ServiceModel.ClientBase<COAD.FISCAL.NFeRetAutorizacao4.NFeRetAutorizacao4Soap>, COAD.FISCAL.NFeRetAutorizacao4.NFeRetAutorizacao4Soap {
        
        public NFeRetAutorizacao4SoapClient() {
        }
        
        public NFeRetAutorizacao4SoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public NFeRetAutorizacao4SoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public NFeRetAutorizacao4SoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public NFeRetAutorizacao4SoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        COAD.FISCAL.NFeRetAutorizacao4.nfeRetAutorizacaoLoteResponse COAD.FISCAL.NFeRetAutorizacao4.NFeRetAutorizacao4Soap.nfeRetAutorizacaoLote(COAD.FISCAL.NFeRetAutorizacao4.nfeRetAutorizacaoLoteRequest request) {
            return base.Channel.nfeRetAutorizacaoLote(request);
        }
        
        public System.Xml.XmlNode nfeRetAutorizacaoLote(System.Xml.XmlNode nfeDadosMsg) {
            COAD.FISCAL.NFeRetAutorizacao4.nfeRetAutorizacaoLoteRequest inValue = new COAD.FISCAL.NFeRetAutorizacao4.nfeRetAutorizacaoLoteRequest();
            inValue.nfeDadosMsg = nfeDadosMsg;
            COAD.FISCAL.NFeRetAutorizacao4.nfeRetAutorizacaoLoteResponse retVal = ((COAD.FISCAL.NFeRetAutorizacao4.NFeRetAutorizacao4Soap)(this)).nfeRetAutorizacaoLote(inValue);
            return retVal.nfeResultMsg;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<COAD.FISCAL.NFeRetAutorizacao4.nfeRetAutorizacaoLoteResponse> COAD.FISCAL.NFeRetAutorizacao4.NFeRetAutorizacao4Soap.nfeRetAutorizacaoLoteAsync(COAD.FISCAL.NFeRetAutorizacao4.nfeRetAutorizacaoLoteRequest request) {
            return base.Channel.nfeRetAutorizacaoLoteAsync(request);
        }
        
        public System.Threading.Tasks.Task<COAD.FISCAL.NFeRetAutorizacao4.nfeRetAutorizacaoLoteResponse> nfeRetAutorizacaoLoteAsync(System.Xml.XmlNode nfeDadosMsg) {
            COAD.FISCAL.NFeRetAutorizacao4.nfeRetAutorizacaoLoteRequest inValue = new COAD.FISCAL.NFeRetAutorizacao4.nfeRetAutorizacaoLoteRequest();
            inValue.nfeDadosMsg = nfeDadosMsg;
            return ((COAD.FISCAL.NFeRetAutorizacao4.NFeRetAutorizacao4Soap)(this)).nfeRetAutorizacaoLoteAsync(inValue);
        }

        public System.Xml.XmlNode nfeRetAutorizacaoLoteMG(System.Xml.XmlNode nfeDadosMsg)
        {
            COAD.FISCAL.NFeRetAutorizacao4.nfeRetAutorizacaoLoteRequest inValue = new COAD.FISCAL.NFeRetAutorizacao4.nfeRetAutorizacaoLoteRequest();
            inValue.nfeDadosMsg = nfeDadosMsg;
            COAD.FISCAL.NFeRetAutorizacao4.nfeRetAutorizacaoLoteResponse retVal = ((COAD.FISCAL.NFeRetAutorizacao4.NFeRetAutorizacao4SoapMG)(this)).nfeRetAutorizacaoLote(inValue);
            return retVal.nfeResultMsg;
        }

        public System.Threading.Tasks.Task<COAD.FISCAL.NFeRetAutorizacao4.nfeRetAutorizacaoLoteResponse> nfeRetAutorizacaoLoteAsyncMG(System.Xml.XmlNode nfeDadosMsg) {
            COAD.FISCAL.NFeRetAutorizacao4.nfeRetAutorizacaoLoteRequest inValue = new COAD.FISCAL.NFeRetAutorizacao4.nfeRetAutorizacaoLoteRequest();
            inValue.nfeDadosMsg = nfeDadosMsg;
            return ((COAD.FISCAL.NFeRetAutorizacao4.NFeRetAutorizacao4SoapMG)(this)).nfeRetAutorizacaoLoteAsync(inValue);
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://www.portalfiscal.inf.br/nfe/wsdl/NFeAutorizacao4", ConfigurationName = "NFeAutorizacao4.NFeAutorizacao4SoapMG")]
    public interface NFeAutorizacao4SoapMG
    {

        // CODEGEN: Gerando contrato de mensagem porque a operação nfeAutorizacaoLote não é RPC nem documento codificado.
        [System.ServiceModel.OperationContractAttribute(Action = "http://www.portalfiscal.inf.br/nfe/wsdl/NFeAutorizacao4/nfeAutorizacaoLote", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        COAD.FISCAL.NFeAutorizacao4.nfeAutorizacaoLoteResponse nfeAutorizacaoLote(COAD.FISCAL.NFeAutorizacao4.nfeAutorizacaoLoteRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.portalfiscal.inf.br/nfe/wsdl/NFeAutorizacao4/nfeAutorizacaoLote", ReplyAction = "*")]
        System.Threading.Tasks.Task<COAD.FISCAL.NFeAutorizacao4.nfeAutorizacaoLoteResponse> nfeAutorizacaoLoteAsync(COAD.FISCAL.NFeAutorizacao4.nfeAutorizacaoLoteRequest request);

        // CODEGEN: Gerando contrato de mensagem porque a operação nfeAutorizacaoLoteZip não é RPC nem documento codificado.
        [System.ServiceModel.OperationContractAttribute(Action = "http://www.portalfiscal.inf.br/nfe/wsdl/NFeAutorizacao4/nfeAutorizacaoLoteZip", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        COAD.FISCAL.NFeAutorizacao4.nfeAutorizacaoLoteZipResponse nfeAutorizacaoLoteZip(COAD.FISCAL.NFeAutorizacao4.nfeAutorizacaoLoteZipRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.portalfiscal.inf.br/nfe/wsdl/NFeAutorizacao4/nfeAutorizacaoLoteZip", ReplyAction = "*")]
        System.Threading.Tasks.Task<COAD.FISCAL.NFeAutorizacao4.nfeAutorizacaoLoteZipResponse> nfeAutorizacaoLoteZipAsync(COAD.FISCAL.NFeAutorizacao4.nfeAutorizacaoLoteZipRequest request);
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://www.portalfiscal.inf.br/nfe/wsdl/NFeRetAutorizacao4", ConfigurationName = "NFeRetAutorizacao4.NFeRetAutorizacao4SoapMG")]
    public interface NFeRetAutorizacao4SoapMG
    {

        // CODEGEN: Gerando contrato de mensagem porque a operação nfeRetAutorizacaoLote não é RPC nem documento codificado.
        [System.ServiceModel.OperationContractAttribute(Action = "http://www.portalfiscal.inf.br/nfe/wsdl/NFeRetAutorizacao4/nfeRetAutorizacaoLote", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        COAD.FISCAL.NFeRetAutorizacao4.nfeRetAutorizacaoLoteResponse nfeRetAutorizacaoLote(COAD.FISCAL.NFeRetAutorizacao4.nfeRetAutorizacaoLoteRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.portalfiscal.inf.br/nfe/wsdl/NFeRetAutorizacao4/nfeRetAutorizacaoLote", ReplyAction = "*")]
        System.Threading.Tasks.Task<COAD.FISCAL.NFeRetAutorizacao4.nfeRetAutorizacaoLoteResponse> nfeRetAutorizacaoLoteAsync(COAD.FISCAL.NFeRetAutorizacao4.nfeRetAutorizacaoLoteRequest request);
    }
}