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
    
    public partial class CONTA_REF
    {
        public CONTA_REF()
        {
            this.CNAB_ARQUIVOS = new HashSet<CNAB_ARQUIVOS>();
            this.CONFIG_ALOCACAO_CONTA = new HashSet<CONFIG_ALOCACAO_CONTA>();
            this.PARCELA_ALOCADA = new HashSet<PARCELA_ALOCADA>();
            this.PARCELAS = new HashSet<PARCELAS>();
            this.PARCELAS_REMESSA = new HashSet<PARCELAS_REMESSA>();
        }
    
        public int CTA_ID { get; set; }
    
        public virtual ICollection<CNAB_ARQUIVOS> CNAB_ARQUIVOS { get; set; }
        public virtual ICollection<CONFIG_ALOCACAO_CONTA> CONFIG_ALOCACAO_CONTA { get; set; }
        public virtual ICollection<PARCELA_ALOCADA> PARCELA_ALOCADA { get; set; }
        public virtual ICollection<PARCELAS> PARCELAS { get; set; }
        public virtual ICollection<PARCELAS_REMESSA> PARCELAS_REMESSA { get; set; }
    }
}