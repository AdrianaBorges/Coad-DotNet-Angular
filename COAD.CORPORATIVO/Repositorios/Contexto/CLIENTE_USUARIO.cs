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
    
    public partial class CLIENTE_USUARIO
    {
        public int USC_ID { get; set; }
        public string USC_LOGIN { get; set; }
        public string USC_SENHA { get; set; }
        public Nullable<bool> USC_ATIVO { get; set; }
        public Nullable<System.DateTime> DATA_EXCLUSAO { get; set; }
        public Nullable<int> CLI_ID { get; set; }
        public string COD_ASSINATURA_PRINCIPAL { get; set; }
    
        public virtual ASSINATURA ASSINATURA { get; set; }
        public virtual CLIENTES CLIENTES { get; set; }
    }
}
