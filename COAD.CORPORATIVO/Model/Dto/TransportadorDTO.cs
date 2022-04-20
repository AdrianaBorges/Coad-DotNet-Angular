using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class TransportadorDTO
    {
        public TransportadorDTO()
        {
            this.NOTA_FISCAL = new HashSet<NotaFiscalDTO>();
        }
    
        [DisplayName("Código")]
        public int TRA_ID { get; set; }
        [DisplayName("Razão Social")]
        public string TRA_RAZAO_SOCIAL { get; set; }
        [DisplayName("Nome Fantasia")]
        public string TRA_NOME_FANTASIA { get; set; }
        [DisplayName("Tipo Pessoa")]
        public string TRA_TIPESSOA { get; set; }
        [DisplayName("CNPJ")]
        public string TRA_CNPJ { get; set; }
        [DisplayName("Endereço")]
        public string TRA_ENDERECO { get; set; }
        [DisplayName("Complemento")]
        public string TRA_END_COMPLEMENTO { get; set; }
        [DisplayName("Bairro")]
        public string TRA_BAIRRO { get; set; }
        [DisplayName("Municipio")]
        public Nullable<int> MUN_ID { get; set; }
        [DisplayName("CEP")]
        public string TRA_CEP { get; set; }
        [DisplayName("Telefone")]
        public string TRA_TEL { get; set; }
        [DisplayName("Celular")]
        public string TRA_CEL { get; set; }
        [DisplayName("Fax")]
        public string TRA_FAX { get; set; }
        [DisplayName("Inscrição")]
        public string TRA_INSCRICAO { get; set; }
        [DisplayName("Contato")]
        public string TRA_CONTATO { get; set; }
        public Nullable<System.DateTime> TRA_DTMOV { get; set; }
        public Nullable<System.DateTime> TRA_DTCAD { get; set; }
        public Nullable<System.DateTime> TRA_DTNASC { get; set; }
        [DisplayName("Status")]
        public string TRA_ATIVO { get; set; }
        [DisplayName("Email")]
        public string TRA_EMAIL { get; set; }
        public string TRA_INSCMUNIP { get; set; }
        public string TRA_INSCSUFRAMA { get; set; }
        public Nullable<System.DateTime> DATA_CADASTRO { get; set; }
        public Nullable<System.DateTime> DATA_ALTERA { get; set; }
        public string USU_LOGIN { get; set; }
        public string TRA_END_NUMERO { get; set; }
        public string MUN_DESCRICAO { get; set; }

        public virtual MunicipioDTO MUNICIPIO { get; set; }
        public virtual ICollection<NotaFiscalDTO> NOTA_FISCAL { get; set; }
   

    }
}
