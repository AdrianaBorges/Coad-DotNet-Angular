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
    
    public partial class CONSULTOR
    {
        public CONSULTOR()
        {
            this.CONSULTOR1 = new HashSet<CONSULTOR>();
            this.HIST_ATEND_EMAIL = new HashSet<HIST_ATEND_EMAIL>();
            this.HIST_ATEND_EMAIL1 = new HashSet<HIST_ATEND_EMAIL>();
        }
    
        public int CON_ID { get; set; }
        public string CON_NOME { get; set; }
        public string CON_TIPO { get; set; }
        public Nullable<bool> CON_SUPERVISOR { get; set; }
        public string USU_LOGIN { get; set; }
        public Nullable<int> CON_ID_SUPREVISOR { get; set; }
    
        public virtual ICollection<CONSULTOR> CONSULTOR1 { get; set; }
        public virtual CONSULTOR CONSULTOR2 { get; set; }
        public virtual ICollection<HIST_ATEND_EMAIL> HIST_ATEND_EMAIL { get; set; }
        public virtual ICollection<HIST_ATEND_EMAIL> HIST_ATEND_EMAIL1 { get; set; }
    }
}
