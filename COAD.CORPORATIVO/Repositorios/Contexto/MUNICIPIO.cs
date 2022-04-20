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
    
    public partial class MUNICIPIO
    {
        public MUNICIPIO()
        {
            this.CEP_LOGRADOURO = new HashSet<CEP_LOGRADOURO>();
            this.CLIENTES_ENDERECO = new HashSet<CLIENTES_ENDERECO>();
            this.ENDERECO_VENDA = new HashSet<ENDERECO_VENDA>();
            this.FORNECEDOR = new HashSet<FORNECEDOR>();
            this.TRANSPORTADOR = new HashSet<TRANSPORTADOR>();
        }
    
        public int MUN_ID { get; set; }
        public string MUN_DESCRICAO { get; set; }
        public string MUN_TIPO { get; set; }
        public string UF { get; set; }
        public string IBGE_COD_COMPLETO { get; set; }
        public string MUN_CEP { get; set; }
        public Nullable<int> RG_ID { get; set; }
    
        public virtual ICollection<CEP_LOGRADOURO> CEP_LOGRADOURO { get; set; }
        public virtual ICollection<CLIENTES_ENDERECO> CLIENTES_ENDERECO { get; set; }
        public virtual ICollection<ENDERECO_VENDA> ENDERECO_VENDA { get; set; }
        public virtual ICollection<FORNECEDOR> FORNECEDOR { get; set; }
        public virtual REGIAO REGIAO { get; set; }
        public virtual UF UF1 { get; set; }
        public virtual ICollection<TRANSPORTADOR> TRANSPORTADOR { get; set; }
    }
}
