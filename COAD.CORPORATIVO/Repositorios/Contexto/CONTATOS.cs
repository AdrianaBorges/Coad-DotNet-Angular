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
    
    public partial class CONTATOS
    {
        public int CON_ID { get; set; }
        public string CON_CARGO { get; set; }
        public string CON_PROFISAO { get; set; }
        public string CON_EMAIL { get; set; }
        public string CON_TELEFONE { get; set; }
        public Nullable<int> CLI_ID { get; set; }
    
        public virtual CLIENTES CLIENTES { get; set; }
    }
}
