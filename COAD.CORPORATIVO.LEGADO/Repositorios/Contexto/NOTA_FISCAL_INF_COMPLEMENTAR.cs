//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace COAD.CORPORATIVO.Repositorios.Contexto
{
    using System;
    using System.Collections.Generic;
    
    public partial class NOTA_FISCAL_INF_COMPLEMENTAR
    {
        public int NFIC_ID { get; set; }
        public int NF_TIPO { get; set; }
        public int NF_NUMERO { get; set; }
        public string NF_SERIE { get; set; }
        public string NFIC_DESCRICAO { get; set; }
    
        public virtual NOTA_FISCAL NOTA_FISCAL { get; set; }
    }
}
