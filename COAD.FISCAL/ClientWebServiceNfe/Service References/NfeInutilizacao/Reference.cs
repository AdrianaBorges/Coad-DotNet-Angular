//------------------------------------------------------------------------------
// <auto-generated>
//     O código foi gerado por uma ferramenta.
//     Versão de Tempo de Execução:4.0.30319.42000
//
//     As alterações ao arquivo poderão causar comportamento incorreto e serão perdidas se
//     o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace COAD.FISCAL.NfeInutilizacao {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NfeInutilizacao2", ConfigurationName="NfeInutilizacao.NfeInutilizacao2Soap")]
    public interface NfeInutilizacao2Soap {
        
        // CODEGEN: Gerando contrato de mensagem porque a operação nfeInutilizacaoNF2 não é RPC nem documento codificado.
        [System.ServiceModel.OperationContractAttribute(Action="http://www.portalfiscal.inf.br/nfe/wsdl/NfeInutilizacao2/nfeInutilizacaoNF2", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        COAD.FISCAL.NfeInutilizacao.nfeInutilizacaoNF2Response nfeInutilizacaoNF2(COAD.FISCAL.NfeInutilizacao.nfeInutilizacaoNF2Request request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.portalfiscal.inf.br/nfe/wsdl/NfeInutilizacao2/nfeInutilizacaoNF2", ReplyAction="*")]
        System.Threading.Tasks.Task<COAD.FISCAL.NfeInutilizacao.nfeInutilizacaoNF2Response> nfeInutilizacaoNF2Async(COAD.FISCAL.NfeInutilizacao.nfeInutilizacaoNF2Request request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2053.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NfeInutilizacao2")]
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
    public partial class nfeInutilizacaoNF2Request {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NfeInutilizacao2")]
        public COAD.FISCAL.NfeInutilizacao.nfeCabecMsg nfeCabecMsg;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NfeInutilizacao2", Order=0)]
        public System.Xml.XmlNode nfeDadosMsg;
        
        public nfeInutilizacaoNF2Request() {
        }
        
        public nfeInutilizacaoNF2Request(COAD.FISCAL.NfeInutilizacao.nfeCabecMsg nfeCabecMsg, System.Xml.XmlNode nfeDadosMsg) {
            this.nfeCabecMsg = nfeCabecMsg;
            this.nfeDadosMsg = nfeDadosMsg;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class nfeInutilizacaoNF2Response {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NfeInutilizacao2")]
        public COAD.FISCAL.NfeInutilizacao.nfeCabecMsg nfeCabecMsg;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NfeInutilizacao2", Order=0)]
        public System.Xml.XmlNode nfeInutilizacaoNF2Result;
        
        public nfeInutilizacaoNF2Response() {
        }
        
        public nfeInutilizacaoNF2Response(COAD.FISCAL.NfeInutilizacao.nfeCabecMsg nfeCabecMsg, System.Xml.XmlNode nfeInutilizacaoNF2Result) {
            this.nfeCabecMsg = nfeCabecMsg;
            this.nfeInutilizacaoNF2Result = nfeInutilizacaoNF2Result;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface NfeInutilizacao2SoapChannel : COAD.FISCAL.NfeInutilizacao.NfeInutilizacao2Soap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class NfeInutilizacao2SoapClient : System.ServiceModel.ClientBase<COAD.FISCAL.NfeInutilizacao.NfeInutilizacao2Soap>, COAD.FISCAL.NfeInutilizacao.NfeInutilizacao2Soap {
        
        public NfeInutilizacao2SoapClient() {
        }
        
        public NfeInutilizacao2SoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public NfeInutilizacao2SoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public NfeInutilizacao2SoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public NfeInutilizacao2SoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        COAD.FISCAL.NfeInutilizacao.nfeInutilizacaoNF2Response COAD.FISCAL.NfeInutilizacao.NfeInutilizacao2Soap.nfeInutilizacaoNF2(COAD.FISCAL.NfeInutilizacao.nfeInutilizacaoNF2Request request) {
            return base.Channel.nfeInutilizacaoNF2(request);
        }
        
        public System.Xml.XmlNode nfeInutilizacaoNF2(ref COAD.FISCAL.NfeInutilizacao.nfeCabecMsg nfeCabecMsg, System.Xml.XmlNode nfeDadosMsg) {
            COAD.FISCAL.NfeInutilizacao.nfeInutilizacaoNF2Request inValue = new COAD.FISCAL.NfeInutilizacao.nfeInutilizacaoNF2Request();
            inValue.nfeCabecMsg = nfeCabecMsg;
            inValue.nfeDadosMsg = nfeDadosMsg;
            COAD.FISCAL.NfeInutilizacao.nfeInutilizacaoNF2Response retVal = ((COAD.FISCAL.NfeInutilizacao.NfeInutilizacao2Soap)(this)).nfeInutilizacaoNF2(inValue);
            nfeCabecMsg = retVal.nfeCabecMsg;
            return retVal.nfeInutilizacaoNF2Result;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<COAD.FISCAL.NfeInutilizacao.nfeInutilizacaoNF2Response> COAD.FISCAL.NfeInutilizacao.NfeInutilizacao2Soap.nfeInutilizacaoNF2Async(COAD.FISCAL.NfeInutilizacao.nfeInutilizacaoNF2Request request) {
            return base.Channel.nfeInutilizacaoNF2Async(request);
        }
        
        public System.Threading.Tasks.Task<COAD.FISCAL.NfeInutilizacao.nfeInutilizacaoNF2Response> nfeInutilizacaoNF2Async(COAD.FISCAL.NfeInutilizacao.nfeCabecMsg nfeCabecMsg, System.Xml.XmlNode nfeDadosMsg) {
            COAD.FISCAL.NfeInutilizacao.nfeInutilizacaoNF2Request inValue = new COAD.FISCAL.NfeInutilizacao.nfeInutilizacaoNF2Request();
            inValue.nfeCabecMsg = nfeCabecMsg;
            inValue.nfeDadosMsg = nfeDadosMsg;
            return ((COAD.FISCAL.NfeInutilizacao.NfeInutilizacao2Soap)(this)).nfeInutilizacaoNF2Async(inValue);
        }
    }
}
