//------------------------------------------------------------------------------
// <auto-generated>
//    O código foi gerado a partir de um modelo.
//
//    Alterações manuais neste arquivo podem provocar comportamento inesperado no aplicativo.
//    Alterações manuais neste arquivo serão substituídas se o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace COAD.PORTAL.Repositorios.Contexto
{
    using System;
    using System.Collections.Generic;
    
    public partial class CO_USUARIOLOGS
    {
        public int id { get; set; }
        public string ip_usuario { get; set; }
        public string nome_usuario { get; set; }
        public int id_usuario { get; set; }
        public System.DateTime ultimo_acesso { get; set; }
        public Nullable<long> id_calendario { get; set; }
        public string acao_realizada { get; set; }
        public string texto_obrigacao { get; set; }
    }
}