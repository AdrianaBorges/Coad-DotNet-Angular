//------------------------------------------------------------------------------
// <auto-generated>
//    O código foi gerado a partir de um modelo.
//
//    Alterações manuais neste arquivo podem provocar comportamento inesperado no aplicativo.
//    Alterações manuais neste arquivo serão substituídas se o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace COAD.SEGURANCA.Repositorios.Contexto
{
    using System;
    using System.Collections.Generic;
    
    public partial class JOB_AGENDAMENTO
    {
        public JOB_AGENDAMENTO()
        {
            this.JOB_NOTIFICACAO = new HashSet<JOB_NOTIFICACAO>();
        }
    
        public int JOB_ID { get; set; }
        public string JOB_NOME { get; set; }
        public string JOB_DESCRICAO { get; set; }
        public Nullable<bool> JOB_ATIVADO { get; set; }
        public Nullable<System.DateTime> JOB_DATA_ULTIMA_EXECUCAO { get; set; }
        public Nullable<int> JOB_MINUTOS_MAX_OCIOSO { get; set; }
        public Nullable<bool> JOB_EXECUTANDO { get; set; }
        public string JOB_EMAIL_ENVIO { get; set; }
        public Nullable<bool> JOB_EXECUTAR_AGORA { get; set; }
        public Nullable<System.DateTime> JOB_INICIO_EXECUCAO { get; set; }
        public Nullable<bool> JOB_ATIVAR { get; set; }
        public string JOB_BATCH_NOME { get; set; }
        public Nullable<int> JOB_BATCH_PROCESSED_ITENS { get; set; }
        public Nullable<int> JOB_BATCH_TOTAL_ITENS { get; set; }
        public Nullable<int> JOB_BATCH_PROGRESS { get; set; }
        public string JOB_PENDING_ITENS_DESC { get; set; }
        public Nullable<int> JOB_COD_EXE_REF { get; set; }
        public string JOB_COD_STR_EXE_REF { get; set; }
        public string JOB_COD_EXE_REF_DESC { get; set; }
        public Nullable<int> JOB_QTD_SUCESSO { get; set; }
        public Nullable<int> JOB_QTD_FALHA { get; set; }
    
        public virtual ICollection<JOB_NOTIFICACAO> JOB_NOTIFICACAO { get; set; }
    }
}
