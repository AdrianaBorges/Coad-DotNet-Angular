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
    
    public partial class consultoria_busca_nao_encontrada
    {
        public int id { get; set; }
        public Nullable<int> id_cliente { get; set; }
        public Nullable<int> id_consultor { get; set; }
        public Nullable<System.DateTime> data_requisicao { get; set; }
        public Nullable<System.DateTime> data_resposta { get; set; }
        public Nullable<int> status { get; set; }
        public string txt_pergunta { get; set; }
        public string txt_resposta { get; set; }
        public string palavra { get; set; }
        public string cli_email { get; set; }
    }
}