//------------------------------------------------------------------------------
// <auto-generated>
//    O código foi gerado a partir de um modelo.
//
//    Alterações manuais neste arquivo podem provocar comportamento inesperado no aplicativo.
//    Alterações manuais neste arquivo serão substituídas se o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace COAD.PROSPECTADOS.Repositorios.Contexto.Base
{
    using System;
    using System.Collections.Generic;
    
    public partial class cart_coad
    {
        public cart_coad()
        {
            this.EMAILS_PROSP = new HashSet<EMAILS_PROSP>();
            this.TELEFONES_PROSP = new HashSet<TELEFONES_PROSP>();
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
    
        public virtual prospects prospects { get; set; }
        public virtual ICollection<EMAILS_PROSP> EMAILS_PROSP { get; set; }
        public virtual ICollection<TELEFONES_PROSP> TELEFONES_PROSP { get; set; }
    }
}
