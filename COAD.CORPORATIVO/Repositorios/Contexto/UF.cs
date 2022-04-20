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
    
    public partial class UF
    {
        public UF()
        {
            this.CADASTRO_GRATUITO = new HashSet<CADASTRO_GRATUITO>();
            this.CARTEIRA = new HashSet<CARTEIRA>();
            this.CFOP_ICMS = new HashSet<CFOP_ICMS>();
            this.CFOP_ICMS1 = new HashSet<CFOP_ICMS>();
            this.CONTRATOS = new HashSet<CONTRATOS>();
            this.MUNICIPIO = new HashSet<MUNICIPIO>();
            this.NOTA_FISCAL = new HashSet<NOTA_FISCAL>();
            this.PROPOSTA_ITEM_COMPROVANTE = new HashSet<PROPOSTA_ITEM_COMPROVANTE>();
            this.REPRESENTANTE = new HashSet<REPRESENTANTE>();
            this.URA = new HashSet<URA>();
        }
    
        public string UF_SIGLA { get; set; }
        public string UF_DESCRICAO { get; set; }
        public Nullable<bool> UF_VALIDA { get; set; }
        public Nullable<int> RG_ID { get; set; }
        public string UF_COD { get; set; }
    
        public virtual ICollection<CADASTRO_GRATUITO> CADASTRO_GRATUITO { get; set; }
        public virtual ICollection<CARTEIRA> CARTEIRA { get; set; }
        public virtual ICollection<CFOP_ICMS> CFOP_ICMS { get; set; }
        public virtual ICollection<CFOP_ICMS> CFOP_ICMS1 { get; set; }
        public virtual ICollection<CONTRATOS> CONTRATOS { get; set; }
        public virtual ICollection<MUNICIPIO> MUNICIPIO { get; set; }
        public virtual ICollection<NOTA_FISCAL> NOTA_FISCAL { get; set; }
        public virtual ICollection<PROPOSTA_ITEM_COMPROVANTE> PROPOSTA_ITEM_COMPROVANTE { get; set; }
        public virtual REGIAO REGIAO { get; set; }
        public virtual ICollection<REPRESENTANTE> REPRESENTANTE { get; set; }
        public virtual ICollection<URA> URA { get; set; }
    }
}