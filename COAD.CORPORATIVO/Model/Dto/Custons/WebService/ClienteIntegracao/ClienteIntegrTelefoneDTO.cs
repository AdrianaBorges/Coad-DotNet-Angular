using Coad.GenericCrud.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.WebService.ClienteIntegracao
{
    [DataContract]
    public class ClienteIntegrTelefoneDTO
    {
        [DataMember]
        public int? CodigoTelefone { get; set; }
        
        [RequiredIf("IsEmpty", false, ErrorMessage = "Se algum dado de telefone for preenchido preencha também o DDD")]
        [DataMember]
        public string DDD { get; set; }

        [RequiredIf("IsEmpty", false, ErrorMessage = "Se algum dado de telefone for preenchido preencha também o Número de Telefone")]
        [DataMember]
        public string Telefone { get; set; }
        [DataMember]
        public string Contato { get; set; }
        [DataMember]
        public string Ramal { get; set; }

        public bool IsEmpty
        {
            get {
                return (
                
                    string.IsNullOrWhiteSpace(DDD) &&
                    string.IsNullOrWhiteSpace(Telefone) &&
                    string.IsNullOrWhiteSpace(Contato) &&
                    string.IsNullOrWhiteSpace(Ramal)
                );
            }

            set { }
        }
        
    }
}
