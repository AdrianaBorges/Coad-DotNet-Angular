﻿//------------------------------------------------------------------------------
// <auto-generated>
//     O código foi gerado por uma ferramenta.
//     Versão de Tempo de Execução:4.0.30319.42000
//
//     As alterações ao arquivo poderão causar comportamento incorreto e serão perdidas se
//     o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace COAD.FISCAL.NfeAutorizacao {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NfeAutorizacao", ConfigurationName="NfeAutorizacao.NfeAutorizacaoSoap")]
    public interface NfeAutorizacaoSoap {
        
        // CODEGEN: Gerando contrato de mensagem porque a operação nfeAutorizacaoLote não é RPC nem documento codificado.
        [System.ServiceModel.OperationContractAttribute(Action="http://www.portalfiscal.inf.br/nfe/wsdl/NfeAutorizacao/nfeAutorizacaoLote", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        COAD.FISCAL.NfeAutorizacao.nfeAutorizacaoLoteResponse nfeAutorizacaoLote(COAD.FISCAL.NfeAutorizacao.nfeAutorizacaoLoteRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.portalfiscal.inf.br/nfe/wsdl/NfeAutorizacao/nfeAutorizacaoLote", ReplyAction="*")]
        System.Threading.Tasks.Task<COAD.FISCAL.NfeAutorizacao.nfeAutorizacaoLoteResponse> nfeAutorizacaoLoteAsync(COAD.FISCAL.NfeAutorizacao.nfeAutorizacaoLoteRequest request);
        
        // CODEGEN: Gerando contrato de mensagem porque a operação nfeAutorizacaoLoteZip não é RPC nem documento codificado.
        [System.ServiceModel.OperationContractAttribute(Action="http://www.portalfiscal.inf.br/nfe/wsdl/NfeAutorizacao/nfeAutorizacaoLoteZip", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        COAD.FISCAL.NfeAutorizacao.nfeAutorizacaoLoteZipResponse nfeAutorizacaoLoteZip(COAD.FISCAL.NfeAutorizacao.nfeAutorizacaoLoteZipRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.portalfiscal.inf.br/nfe/wsdl/NfeAutorizacao/nfeAutorizacaoLoteZip", ReplyAction="*")]
        System.Threading.Tasks.Task<COAD.FISCAL.NfeAutorizacao.nfeAutorizacaoLoteZipResponse> nfeAutorizacaoLoteZipAsync(COAD.FISCAL.NfeAutorizacao.nfeAutorizacaoLoteZipRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2053.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NfeAutorizacao")]
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
    public partial class nfeAutorizacaoLoteRequest {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NfeAutorizacao")]
        public COAD.FISCAL.NfeAutorizacao.nfeCabecMsg nfeCabecMsg;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NfeAutorizacao", Order=0)]
        public System.Xml.XmlNode nfeDadosMsg;
        
        public nfeAutorizacaoLoteRequest() {
        }
        
        public nfeAutorizacaoLoteRequest(COAD.FISCAL.NfeAutorizacao.nfeCabecMsg nfeCabecMsg, System.Xml.XmlNode nfeDadosMsg) {
            this.nfeCabecMsg = nfeCabecMsg;
            this.nfeDadosMsg = nfeDadosMsg;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class nfeAutorizacaoLoteResponse {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NfeAutorizacao")]
        public COAD.FISCAL.NfeAutorizacao.nfeCabecMsg nfeCabecMsg;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NfeAutorizacao", Order=0)]
        public System.Xml.XmlNode nfeAutorizacaoLoteResult;
        
        public nfeAutorizacaoLoteResponse() {
        }
        
        public nfeAutorizacaoLoteResponse(COAD.FISCAL.NfeAutorizacao.nfeCabecMsg nfeCabecMsg, System.Xml.XmlNode nfeAutorizacaoLoteResult) {
            this.nfeCabecMsg = nfeCabecMsg;
            this.nfeAutorizacaoLoteResult = nfeAutorizacaoLoteResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class nfeAutorizacaoLoteZipRequest {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NfeAutorizacao")]
        public COAD.FISCAL.NfeAutorizacao.nfeCabecMsg nfeCabecMsg;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NfeAutorizacao", Order=0)]
        public string nfeDadosMsgZip;
        
        public nfeAutorizacaoLoteZipRequest() {
        }
        
        public nfeAutorizacaoLoteZipRequest(COAD.FISCAL.NfeAutorizacao.nfeCabecMsg nfeCabecMsg, string nfeDadosMsgZip) {
            this.nfeCabecMsg = nfeCabecMsg;
            this.nfeDadosMsgZip = nfeDadosMsgZip;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class nfeAutorizacaoLoteZipResponse {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NfeAutorizacao")]
        public COAD.FISCAL.NfeAutorizacao.nfeCabecMsg nfeCabecMsg;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NfeAutorizacao", Order=0)]
        public System.Xml.XmlNode nfeAutorizacaoLoteZipResult;
        
        public nfeAutorizacaoLoteZipResponse() {
        }
        
        public nfeAutorizacaoLoteZipResponse(COAD.FISCAL.NfeAutorizacao.nfeCabecMsg nfeCabecMsg, System.Xml.XmlNode nfeAutorizacaoLoteZipResult) {
            this.nfeCabecMsg = nfeCabecMsg;
            this.nfeAutorizacaoLoteZipResult = nfeAutorizacaoLoteZipResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface NfeAutorizacaoSoapChannel : COAD.FISCAL.NfeAutorizacao.NfeAutorizacaoSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class NfeAutorizacaoSoapClient : System.ServiceModel.ClientBase<COAD.FISCAL.NfeAutorizacao.NfeAutorizacaoSoap>, COAD.FISCAL.NfeAutorizacao.NfeAutorizacaoSoap {
        
        public NfeAutorizacaoSoapClient() {
        }
        
        public NfeAutorizacaoSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public NfeAutorizacaoSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public NfeAutorizacaoSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public NfeAutorizacaoSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        COAD.FISCAL.NfeAutorizacao.nfeAutorizacaoLoteResponse COAD.FISCAL.NfeAutorizacao.NfeAutorizacaoSoap.nfeAutorizacaoLote(COAD.FISCAL.NfeAutorizacao.nfeAutorizacaoLoteRequest request) {
            return base.Channel.nfeAutorizacaoLote(request);
        }
        
        public System.Xml.XmlNode nfeAutorizacaoLote(ref COAD.FISCAL.NfeAutorizacao.nfeCabecMsg nfeCabecMsg, System.Xml.XmlNode nfeDadosMsg) {
            COAD.FISCAL.NfeAutorizacao.nfeAutorizacaoLoteRequest inValue = new COAD.FISCAL.NfeAutorizacao.nfeAutorizacaoLoteRequest();
            inValue.nfeCabecMsg = nfeCabecMsg;
            inValue.nfeDadosMsg = nfeDadosMsg;
            COAD.FISCAL.NfeAutorizacao.nfeAutorizacaoLoteResponse retVal = ((COAD.FISCAL.NfeAutorizacao.NfeAutorizacaoSoap)(this)).nfeAutorizacaoLote(inValue);
            nfeCabecMsg = retVal.nfeCabecMsg;
            return retVal.nfeAutorizacaoLoteResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<COAD.FISCAL.NfeAutorizacao.nfeAutorizacaoLoteResponse> COAD.FISCAL.NfeAutorizacao.NfeAutorizacaoSoap.nfeAutorizacaoLoteAsync(COAD.FISCAL.NfeAutorizacao.nfeAutorizacaoLoteRequest request) {
            return base.Channel.nfeAutorizacaoLoteAsync(request);
        }
        
        public System.Threading.Tasks.Task<COAD.FISCAL.NfeAutorizacao.nfeAutorizacaoLoteResponse> nfeAutorizacaoLoteAsync(COAD.FISCAL.NfeAutorizacao.nfeCabecMsg nfeCabecMsg, System.Xml.XmlNode nfeDadosMsg) {
            COAD.FISCAL.NfeAutorizacao.nfeAutorizacaoLoteRequest inValue = new COAD.FISCAL.NfeAutorizacao.nfeAutorizacaoLoteRequest();
            inValue.nfeCabecMsg = nfeCabecMsg;
            inValue.nfeDadosMsg = nfeDadosMsg;
            return ((COAD.FISCAL.NfeAutorizacao.NfeAutorizacaoSoap)(this)).nfeAutorizacaoLoteAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        COAD.FISCAL.NfeAutorizacao.nfeAutorizacaoLoteZipResponse COAD.FISCAL.NfeAutorizacao.NfeAutorizacaoSoap.nfeAutorizacaoLoteZip(COAD.FISCAL.NfeAutorizacao.nfeAutorizacaoLoteZipRequest request) {
            return base.Channel.nfeAutorizacaoLoteZip(request);
        }
        
        public System.Xml.XmlNode nfeAutorizacaoLoteZip(ref COAD.FISCAL.NfeAutorizacao.nfeCabecMsg nfeCabecMsg, string nfeDadosMsgZip) {
            COAD.FISCAL.NfeAutorizacao.nfeAutorizacaoLoteZipRequest inValue = new COAD.FISCAL.NfeAutorizacao.nfeAutorizacaoLoteZipRequest();
            inValue.nfeCabecMsg = nfeCabecMsg;
            inValue.nfeDadosMsgZip = nfeDadosMsgZip;
            COAD.FISCAL.NfeAutorizacao.nfeAutorizacaoLoteZipResponse retVal = ((COAD.FISCAL.NfeAutorizacao.NfeAutorizacaoSoap)(this)).nfeAutorizacaoLoteZip(inValue);
            nfeCabecMsg = retVal.nfeCabecMsg;
            return retVal.nfeAutorizacaoLoteZipResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<COAD.FISCAL.NfeAutorizacao.nfeAutorizacaoLoteZipResponse> COAD.FISCAL.NfeAutorizacao.NfeAutorizacaoSoap.nfeAutorizacaoLoteZipAsync(COAD.FISCAL.NfeAutorizacao.nfeAutorizacaoLoteZipRequest request) {
            return base.Channel.nfeAutorizacaoLoteZipAsync(request);
        }
        
        public System.Threading.Tasks.Task<COAD.FISCAL.NfeAutorizacao.nfeAutorizacaoLoteZipResponse> nfeAutorizacaoLoteZipAsync(COAD.FISCAL.NfeAutorizacao.nfeCabecMsg nfeCabecMsg, string nfeDadosMsgZip) {
            COAD.FISCAL.NfeAutorizacao.nfeAutorizacaoLoteZipRequest inValue = new COAD.FISCAL.NfeAutorizacao.nfeAutorizacaoLoteZipRequest();
            inValue.nfeCabecMsg = nfeCabecMsg;
            inValue.nfeDadosMsgZip = nfeDadosMsgZip;
            return ((COAD.FISCAL.NfeAutorizacao.NfeAutorizacaoSoap)(this)).nfeAutorizacaoLoteZipAsync(inValue);
        }
    }
}
