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
    
    public partial class FATOR_CONVERSAO
    {
        public string UND_ID { get; set; }
        public Nullable<decimal> FAT_CONVERSAO { get; set; }
    
        public virtual UNIDADE_MEDIDA UNIDADE_MEDIDA { get; set; }
    }
}