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
    
    public partial class CARTEIRA_TRANSF_ITEM
    {
        public int CAT_ID { get; set; }
        public string CAR_ID { get; set; }
        public string ASN_NUM_ASSINATURA { get; set; }
        public Nullable<int> UEN_ID { get; set; }
        public string CAR_ID_ANT { get; set; }
    
        public virtual CARTEIRA_TRANSF CARTEIRA_TRANSF { get; set; }
    }
}