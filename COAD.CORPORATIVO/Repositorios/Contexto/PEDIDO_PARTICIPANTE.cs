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
    
    public partial class PEDIDO_PARTICIPANTE
    {
        public int PPR_ID { get; set; }
        public string PPR_NOME { get; set; }
        public string PPR_CPF_CNPJ { get; set; }
        public string PPR_EMAIL { get; set; }
        public string PPR_DDD { get; set; }
        public string PPR_TELEFONE { get; set; }
        public Nullable<int> CLI_ID { get; set; }
        public Nullable<int> PED_CRM_ID { get; set; }
        public Nullable<int> IPE_ID { get; set; }
        public Nullable<bool> PED_EH_O_COMPRADOR { get; set; }
        public Nullable<int> PRT_ID { get; set; }
        public Nullable<int> PPI_ID { get; set; }
    
        public virtual CLIENTES CLIENTES { get; set; }
        public virtual ITEM_PEDIDO ITEM_PEDIDO { get; set; }
        public virtual PEDIDO_CRM PEDIDO_CRM { get; set; }
        public virtual PROPOSTA_ITEM PROPOSTA_ITEM { get; set; }
        public virtual PROPOSTA PROPOSTA { get; set; }
    }
}