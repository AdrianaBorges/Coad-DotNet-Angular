//------------------------------------------------------------------------------
// <auto-generated>
//    O código foi gerado a partir de um modelo.
//
//    Alterações manuais neste arquivo podem provocar comportamento inesperado no aplicativo.
//    Alterações manuais neste arquivo serão substituídas se o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace COAD.CORPORATIVO.LEGADO.Repositorios.Contexto
{
    using System;
    using System.Collections.Generic;
    
    public partial class AGENDA
    {
        public string CODIGO { get; set; }
        public string DATA_HIST { get; set; }
        public string HORA_HIST { get; set; }
        public string HISTORICO_BASICO { get; set; }
        public string HISTORICO_LIVRE { get; set; }
        public Nullable<short> CONTATO { get; set; }
        public Nullable<short> RESULTADO { get; set; }
        public string DATA_AGENDA { get; set; }
        public string HORA_AGENDA { get; set; }
        public string DATA_REAL_CANC { get; set; }
        public string CARTEIRA { get; set; }
        public string NOME { get; set; }
        public string A_C { get; set; }
        public Nullable<short> TIPO_ATEND { get; set; }
        public Nullable<short> TAREFA { get; set; }
        public string CARTEIRA_ATEND { get; set; }
        public Nullable<int> ID { get; set; }
        public Nullable<int> IDINC { get; set; }
        public Nullable<int> cancelamentoMotivo_id { get; set; }
    }
}
