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
    
    public partial class NOTICIA_GRUPO_LINHA_PROD
    {
        public int NGR_ID { get; set; }
        public int LIN_PRO_ID { get; set; }
        public string DATA_ASSOCIACAO { get; set; }
    
        public virtual LINHA_PRODUTO_REF LINHA_PRODUTO_REF { get; set; }
        public virtual NOTICIA_GRUPO NOTICIA_GRUPO { get; set; }
    }
}