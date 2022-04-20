using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    [DataContract]
    public class DadosPedidoIntegracaoDTO 
    {
        [DataMember]
        public string numeroDocumento { get; set; }
        [DataMember]
        public int tipoDocumento { get; set; }
        [DataMember]
        public string email { get; set; }
        [DataMember]
        public string nome { get; set; }
        [DataMember]
        public int tipoPessoa { get; set; }
        [DataMember]
        public string dddComercial { get; set; }
        [DataMember]
        public string foneComercial { get; set; }
        [DataMember]
        public string dddCelular { get; set; }
        [DataMember]
        public string foneCelular { get; set; }
        [DataMember]
        public int tipoEndereco { get; set; }
        [DataMember]
        public string cidade { get; set; }
        [DataMember]
        public string complemento { get; set; }
        [DataMember]
        public string bairro { get; set; }
        [DataMember]
        public string numero { get; set; }
        [DataMember]
        public string UF { get; set; }
        [DataMember]
        public string endereco { get; set; }
        [DataMember]
        public string CEP { get; set; }
        [DataMember]
        public Nullable<int> cmp_id { get; set; }
        [DataMember]
        public int idpedido { get; set; }
        [DataMember]
        public string chavepedido { get; set; }
        [DataMember]
        public Nullable<int> cli_id { get; set; }
        [DataMember]
        public string senha { get; set; }
        [DataMember]
        public int formapgto { get; set; }
        [DataMember]
        public int numeroparcelas { get; set; }
        [DataMember]
        public int recorrente { get; set; }
        [DataMember]
        public Nullable<int> erro { get; set; }
        [DataMember]
        public string mensagem { get; set; }
        
    }
}
