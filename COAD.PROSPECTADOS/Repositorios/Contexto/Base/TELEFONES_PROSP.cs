//------------------------------------------------------------------------------
// <auto-generated>
//    O código foi gerado a partir de um modelo.
//
//    Alterações manuais neste arquivo podem provocar comportamento inesperado no aplicativo.
//    Alterações manuais neste arquivo serão substituídas se o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace COAD.PROSPECTADOS.Repositorios.Contexto.Base
{
    using System;
    using System.Collections.Generic;
    
    public partial class TELEFONES_PROSP
    {
        public string CODIGO { get; set; }
        public string DDD_TEL { get; set; }
        public string TELEFONE { get; set; }
        public string TIPO { get; set; }
    
        public virtual cart_coad cart_coad { get; set; }
    }
}
