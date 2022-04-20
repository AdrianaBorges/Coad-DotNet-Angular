using COAD.PROSPECTADOS.Repositorios.Contexto.Base;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PROSPECTADOS.Model.Dto
{
    [Mapping(Source = typeof(cart_coad))]
    public class CartCoadDTO
    {
        public CartCoadDTO()
        {
            this.EMAILS_PROSP = new HashSet<EmailsProspDTO>();
            this.TELEFONES_PROSP = new HashSet<TelefoneProspectDTO>();
        }
    
        public string CODIGO { get; set; }
        public string NOME { get; set; }
        public string A_C { get; set; }
        public string TIPO { get; set; }
        public string LOGRAD { get; set; }
        public string NUMERO { get; set; }
        public string TIPO_COMPL { get; set; }
        public string COMPL { get; set; }
        public string TIPO_COMPL2 { get; set; }
        public string COMPL2 { get; set; }
        public string TIPO_COMPL3 { get; set; }
        public string COMPL3 { get; set; }
        public string BAIRRO { get; set; }
        public string MUNIC { get; set; }
        public string UF { get; set; }
        public string CEP { get; set; }
        public string DDD_TEL { get; set; }
        public string TELEFONE { get; set; }
        public string DDD_FAX { get; set; }
        public string FAX { get; set; }
        public string E_MAIL { get; set; }
        public string CARGO { get; set; }
        public string PROF { get; set; }
        public string IDENTIFICACAO { get; set; }
        public string DATA_CADASTRO { get; set; }
        public string cep_status { get; set; }
        public Nullable<int> CLI_ID { get; set; }
    
        [IgnoreMemberMapping(MappingDirection.SourceToDestiny)]
        public virtual ICollection<EmailsProspDTO> EMAILS_PROSP { get; set; }

        [IgnoreMemberMapping(MappingDirection.DestinyToSource)]
        public virtual ProspectsDTO prospects { get; set; }

        [IgnoreMemberMapping(MappingDirection.SourceToDestiny)]
        public virtual ICollection<TelefoneProspectDTO> TELEFONES_PROSP { get; set; }
    
    }
}
