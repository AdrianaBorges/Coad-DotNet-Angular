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
    
    public partial class URA_LOG
    {
        public int ULG_ID { get; set; }
        public string URA_ID { get; set; }
        public string ASN_NUM_ASSINATURA { get; set; }
        public System.DateTime ULG_DATA { get; set; }
        public int URA_TP_ATU_ID { get; set; }
        public string ULG_OBS { get; set; }
        public string USU_LOGIN { get; set; }
    
        public virtual ASSINATURA ASSINATURA { get; set; }
        public virtual URA URA { get; set; }
        public virtual URA_TP_ATU URA_TP_ATU { get; set; }
    }
}