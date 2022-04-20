using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COADCORP.Models
{
    public class DTOProspectCliente
    {
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
        public string DDD_TEL2 { get; set; }//vem do srv telefone
        public string TELEFONE2 { get; set; }//vem do srv telefone
        public string TIPOTELEFONE2 { get; set; }//vem do srv telefone
        public string E_MAIL1 { get; set; }
        public string E_MAIL2 { get; set; }//vem do srv email
        public string CARGO { get; set; }
        public string PROF { get; set; }
        public string IDENTIFICACAO { get; set; }
        public string DATA_CADASTRO { get; set; }
        public string cep_status { get; set; } //vem do srv prospectsinformações adicionais
        public string FUNC_IND { get; set; } //vem do srv prospectsinformações adicionais
        public string CART { get; set; } //vem do srv prospectsinformações adicionais
        public string DRIVE_CDROM { get; set; } //vem do srv prospectsinformações adicionais
        public string DATA_EMI_FICHA { get; set; } //vem do srv prospectsinformações adicionais
        public string AREA { get; set; } //vem do srv prospectsinformações adicionais
        public string DATA_ATRIBUICAO { get; set; } //vem do srv prospectsinformações adicionais
        public string MANTER { get; set; } //vem do srv prospectsinformações adicionais
        public string INTERNET { get; set; } //vem do srv prospectsinformações adicionais
        public string PFIS_PJUR { get; set; } //vem do srv prospectsinformações adicionais
        public string CPF_CNPJ { get; set; } //vem do srv prospectsinformações adicionais
        public string REGIAO { get; set; } //vem do srv prospectsinformações adicionais
        public string MALA_ADV_SN { get; set; } //vem do srv prospectsinformações adicionais
        public string INSCRICAO { get; set; } //vem do srv prospectsinformações adicionais
    }
}