﻿//------------------------------------------------------------------------------
// <auto-generated>
//     O código foi gerado por uma ferramenta.
//     Versão de Tempo de Execução:4.0.30319.42000
//
//     As alterações ao arquivo poderão causar comportamento incorreto e serão perdidas se
//     o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace COAD.FISCAL.NfeRetAutorizacao {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NfeRetAutorizacao", ConfigurationName="NfeRetAutorizacao.NfeRetAutorizacaoSoap")]
    public interface NfeRetAutorizacaoSoap {
        
        // CODEGEN: Gerando contrato de mensagem porque a operação nfeRetAutorizacaoLote não é RPC nem documento codificado.
        [System.ServiceModel.OperationContractAttribute(Action="http://www.portalfiscal.inf.br/nfe/wsdl/NfeRetAutorizacao/nfeRetAutorizacaoLote", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        COAD.FISCAL.NfeRetAutorizacao.nfeRetAutorizacaoLoteResponse nfeRetAutorizacaoLote(COAD.FISCAL.NfeRetAutorizacao.nfeRetAutorizacaoLoteRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.portalfiscal.inf.br/nfe/wsdl/NfeRetAutorizacao/nfeRetAutorizacaoLote", ReplyAction="*")]
        System.Threading.Tasks.Task<COAD.FISCAL.NfeRetAutorizacao.nfeRetAutorizacaoLoteResponse> nfeRetAutorizacaoLoteAsync(COAD.FISCAL.NfeRetAutorizacao.nfeRetAutorizacaoLoteRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2053.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NfeRetAutorizacao")]
    public partial class nfeCabecMsg : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string cUFField;
        
        private string versaoDadosField;
        
        private System.Xml.XmlAttribute[] anyAttrField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string cUF {
            get {
                return this.cUFField;
            }
            set {
                this.cUFField = value;
                this.RaisePropertyChanged("cUF");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string versaoDados {
            get {
                return this.versaoDadosField;
            }
            set {
                this.versaoDadosField = value;
                this.RaisePropertyChanged("versaoDados");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr {
            get {
                return this.anyAttrField;
            }
            set {
                this.anyAttrField = value;
                this.RaisePropertyChanged("AnyAttr");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class nfeRetAutorizacaoLoteRequest {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NfeRetAutorizacao")]
        public COAD.FISCAL.NfeRetAutorizacao.nfeCabecMsg nfeCabecMsg;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NfeRetAutorizacao", Order=0)]
        public System.Xml.XmlNode nfeDadosMsg;
        
        public nfeRetAutorizacaoLoteRequest() {
        }
        
        public nfeRetAutorizacaoLoteRequest(COAD.FISCAL.NfeRetAutorizacao.nfeCabecMsg nfeCabecMsg, System.Xml.XmlNode nfeDadosMsg) {
            this.nfeCabecMsg = nfeCabecMsg;
            this.nfeDadosMsg = nfeDadosMsg;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class nfeRetAutorizacaoLoteResponse {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NfeRetAutorizacao")]
        public COAD.FISCAL.NfeRetAutorizacao.nfeCabecMsg nfeCabecMsg;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NfeRetAutorizacao", Order=0)]
        public System.Xml.XmlNode nfeRetAutorizacaoLoteResult;
        
        public nfeRetAutorizacaoLoteResponse() {
        }
        
        public nfeRetAutorizacaoLoteResponse(COAD.FISCAL.NfeRetAutorizacao.nfeCabecMsg nfeCabecMsg, System.Xml.XmlNode nfeRetAutorizacaoLoteResult) {
            this.nfeCabecMsg = nfeCabecMsg;
            this.nfeRetAutorizacaoLoteResult = nfeRetAutorizacaoLoteResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface NfeRetAutorizacaoSoapChannel : COAD.FISCAL.NfeRetAutorizacao.NfeRetAutorizacaoSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class NfeRetAutorizacaoSoapClient : System.ServiceModel.ClientBase<COAD.FISCAL.NfeRetAutorizacao.NfeRetAutorizacaoSoap>, COAD.FISCAL.NfeRetAutorizacao.NfeRetAutorizacaoSoap {
        
        public NfeRetAutorizacaoSoapClient() {
        }
        
        public NfeRetAutorizacaoSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public NfeRetAutorizacaoSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public NfeRetAutorizacaoSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public NfeRetAutorizacaoSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        COAD.FISCAL.NfeRetAutorizacao.nfeRetAutorizacaoLoteResponse COAD.FISCAL.NfeRetAutorizacao.NfeRetAutorizacaoSoap.nfeRetAutorizacaoLote(COAD.FISCAL.NfeRetAutorizacao.nfeRetAutorizacaoLoteRequest request) {
            return base.Channel.nfeRetAutorizacaoLote(request);
        }
        
        public System.Xml.XmlNode nfeRetAutorizacaoLote(ref COAD.FISCAL.NfeRetAutorizacao.nfeCabecMsg nfeCabecMsg, System.Xml.XmlNode nfeDadosMsg) {
            COAD.FISCAL.NfeRetAutorizacao.nfeRetAutorizacaoLoteRequest inValue = new COAD.FISCAL.NfeRetAutorizacao.nfeRetAutorizacaoLoteRequest();
            inValue.nfeCabecMsg = nfeCabecMsg;
            inValue.nfeDadosMsg = nfeDadosMsg;
            COAD.FISCAL.NfeRetAutorizacao.nfeRetAutorizacaoLoteResponse retVal = ((COAD.FISCAL.NfeRetAutorizacao.NfeRetAutorizacaoSoap)(this)).nfeRetAutorizacaoLote(inValue);
            nfeCabecMsg = retVal.nfeCabecMsg;
            return retVal.nfeRetAutorizacaoLoteResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<COAD.FISCAL.NfeRetAutorizacao.nfeRetAutorizacaoLoteResponse> COAD.FISCAL.NfeRetAutorizacao.NfeRetAutorizacaoSoap.nfeRetAutorizacaoLoteAsync(COAD.FISCAL.NfeRetAutorizacao.nfeRetAutorizacaoLoteRequest request) {
            return base.Channel.nfeRetAutorizacaoLoteAsync(request);
        }
        
        public System.Threading.Tasks.Task<COAD.FISCAL.NfeRetAutorizacao.nfeRetAutorizacaoLoteResponse> nfeRetAutorizacaoLoteAsync(COAD.FISCAL.NfeRetAutorizacao.nfeCabecMsg nfeCabecMsg, System.Xml.XmlNode nfeDadosMsg) {
            COAD.FISCAL.NfeRetAutorizacao.nfeRetAutorizacaoLoteRequest inValue = new COAD.FISCAL.NfeRetAutorizacao.nfeRetAutorizacaoLoteRequest();
            inValue.nfeCabecMsg = nfeCabecMsg;
            inValue.nfeDadosMsg = nfeDadosMsg;
            return ((COAD.FISCAL.NfeRetAutorizacao.NfeRetAutorizacaoSoap)(this)).nfeRetAutorizacaoLoteAsync(inValue);
        }
    }
}