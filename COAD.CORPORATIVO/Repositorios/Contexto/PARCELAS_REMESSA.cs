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
    
    public partial class PARCELAS_REMESSA
    {
        public PARCELAS_REMESSA()
        {
            this.PARCELA_ALOCADA = new HashSet<PARCELA_ALOCADA>();
            this.PARCELAS = new HashSet<PARCELAS>();
        }
    
        public int REM_ID { get; set; }
        public string REM_REF { get; set; }
        public Nullable<System.DateTime> REM_DATA { get; set; }
        public string REM_TRANSMITIDO { get; set; }
        public string BAN_ID { get; set; }
        public Nullable<int> CTA_ID { get; set; }
        public Nullable<int> EMP_ID { get; set; }
        public Nullable<System.DateTime> REM_DATA_TRANSMISSAO { get; set; }
        public Nullable<System.DateTime> REM_DATA_DESALOCACAO { get; set; }
        public Nullable<int> REM_QTDE { get; set; }
        public Nullable<decimal> REM_TOTAL_REMESSA { get; set; }
        public Nullable<System.DateTime> REM_DATA_REMESSA { get; set; }
        public Nullable<bool> REM_AVULSA { get; set; }
        public Nullable<int> TRE_ID { get; set; }
        public Nullable<bool> REM_SACADOR_AVALISTA { get; set; }
    
        public virtual BANCOS BANCOS { get; set; }
        public virtual CONTA_REF CONTA_REF { get; set; }
        public virtual EMPRESA_REF EMPRESA_REF { get; set; }
        public virtual ICollection<PARCELA_ALOCADA> PARCELA_ALOCADA { get; set; }
        public virtual ICollection<PARCELAS> PARCELAS { get; set; }
        public virtual TIPO_REMESSA TIPO_REMESSA { get; set; }
    }
}
