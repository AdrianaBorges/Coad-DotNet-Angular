//------------------------------------------------------------------------------
// <auto-generated>
//    O código foi gerado a partir de um modelo.
//
//    Alterações manuais neste arquivo podem provocar comportamento inesperado no aplicativo.
//    Alterações manuais neste arquivo serão substituídas se o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace COAD.COADGED.Repositorios.Contexto
{
    using System;
    using System.Collections.Generic;
    
    public partial class CONSULTA_EMAIL_GRANDE_GRUPO
    {
        public CONSULTA_EMAIL_GRANDE_GRUPO()
        {
            this.CONSULTA_EMAIL = new HashSet<CONSULTA_EMAIL>();
        }
    
        public int GRG_ID { get; set; }
        public string GRG_DESCRICAO { get; set; }
        public int COLEC_ID { get; set; }
    
        public virtual ICollection<CONSULTA_EMAIL> CONSULTA_EMAIL { get; set; }
        public virtual CONSULTA_EMAIL_COLECIONADOR CONSULTA_EMAIL_COLECIONADOR { get; set; }
    }
}
