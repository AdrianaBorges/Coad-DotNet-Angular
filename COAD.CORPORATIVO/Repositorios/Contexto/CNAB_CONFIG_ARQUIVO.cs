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
    
    public partial class CNAB_CONFIG_ARQUIVO
    {
        public CNAB_CONFIG_ARQUIVO()
        {
            this.CNAB = new HashSet<CNAB>();
        }
    
        public int CCA_ID { get; set; }
        public int CNC_ID { get; set; }
        public string CCA_TIPO { get; set; }
        public string CCA_DESCRICAO { get; set; }
        public Nullable<System.DateTime> DATA_EXCLUSAO { get; set; }
    
        public virtual ICollection<CNAB> CNAB { get; set; }
        public virtual CNAB_CONFIG CNAB_CONFIG { get; set; }
    }
}
