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
    
    public partial class TIPO_CLIENTE
    {
        public TIPO_CLIENTE()
        {
            this.CLIENTES = new HashSet<CLIENTES>();
            this.CONFIG_IMPOSTO = new HashSet<CONFIG_IMPOSTO>();
        }
    
        public int TIPO_CLI_ID { get; set; }
        public string TIPO_CLI_DESCRICAO { get; set; }
        public Nullable<int> TIPO_CLI_ATIVO { get; set; }
    
        public virtual ICollection<CLIENTES> CLIENTES { get; set; }
        public virtual ICollection<CONFIG_IMPOSTO> CONFIG_IMPOSTO { get; set; }
    }
}
