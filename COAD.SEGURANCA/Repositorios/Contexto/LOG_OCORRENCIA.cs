//------------------------------------------------------------------------------
// <auto-generated>
//    O código foi gerado a partir de um modelo.
//
//    Alterações manuais neste arquivo podem provocar comportamento inesperado no aplicativo.
//    Alterações manuais neste arquivo serão substituídas se o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace COAD.SEGURANCA.Repositorios.Contexto
{
    using System;
    using System.Collections.Generic;
    
    public partial class LOG_OCORRENCIA
    {
        public int LOG_SEQ { get; set; }
        public string LOG_MESSAGE { get; set; }
        public string LOG_IP_ACESSO { get; set; }
        public string ITM_PATH { get; set; }
        public string USU_LOGIN { get; set; }
        public Nullable<System.DateTime> LOG_DATA { get; set; }
        public string LOG_ID_ERRO { get; set; }
        public string LOG_EMAIL { get; set; }
        public string LOG_STACK_TRACE { get; set; }
        public string LOG_FULL_MSG { get; set; }
    }
}
