//------------------------------------------------------------------------------
// <auto-generated>
//    O código foi gerado a partir de um modelo.
//
//    Alterações manuais neste arquivo podem provocar comportamento inesperado no aplicativo.
//    Alterações manuais neste arquivo serão substituídas se o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace COAD.CORPORATIVO.Repositorios.Contexto
{
    using System;
    using System.Collections.Generic;
    
    public partial class CLIENTES_VW
    {
        public int CLI_ID { get; set; }
        public string CLI_NOME { get; set; }
        public string CLI_A_C { get; set; }
        public string CLI_TP_PESSOA { get; set; }
        public string CLI_CPF_CNPJ { get; set; }
        public string CLI_INSCRICAO { get; set; }
        public string MXM_CODIGO { get; set; }
        public Nullable<System.DateTime> DATA_CADASTRO { get; set; }
        public Nullable<System.DateTime> DATA_ALTERA { get; set; }
        public string USU_LOGIN { get; set; }
        public Nullable<int> CLA_ID { get; set; }
        public Nullable<int> TIPO_CLI_ID { get; set; }
        public string CLI_SUFRAMA { get; set; }
        public string CLI_COD_PAIS { get; set; }
        public Nullable<int> CLA_CLI_ID { get; set; }
        public Nullable<System.DateTime> DATA_EXCLUSAO { get; set; }
        public Nullable<int> CODIGO_ANTIGO { get; set; }
        public string CLI_EMAIL { get; set; }
        public Nullable<System.DateTime> DATA_ULTIMO_HISTORICO { get; set; }
    }
}