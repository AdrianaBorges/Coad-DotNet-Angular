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
    
    public partial class URA_COAD
    {
        public int ID { get; set; }
        public string URAID { get; set; }
        public Nullable<int> VIP { get; set; }
        public Nullable<int> DDD { get; set; }
        public string TELEFONE { get; set; }
        public Nullable<int> SENHA { get; set; }
        public string CODIGO { get; set; }
        public string NOME { get; set; }
        public Nullable<int> PODE { get; set; }
        public Nullable<int> QTE_CONS { get; set; }
        public Nullable<int> ACESSO { get; set; }
        public Nullable<int> QTE_REALIZ { get; set; }
        public string GRUPO { get; set; }
    
        public virtual URA URA { get; set; }
    }
}