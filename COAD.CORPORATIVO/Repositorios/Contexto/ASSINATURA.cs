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
    
    public partial class ASSINATURA
    {
        public ASSINATURA()
        {
            this.AGENDA_COBRANCA = new HashSet<AGENDA_COBRANCA>();
            this.ASSINATURA1 = new HashSet<ASSINATURA>();
            this.CHEQUE_DEVOLVIDO = new HashSet<CHEQUE_DEVOLVIDO>();
            this.ASSINATURA_EMAIL = new HashSet<ASSINATURA_EMAIL>();
            this.ASSINATURA_TELEFONE = new HashSet<ASSINATURA_TELEFONE>();
            this.ASSINATURA_TRANSFERENCIA = new HashSet<ASSINATURA_TRANSFERENCIA>();
            this.CARTEIRA_ASSINATURA = new HashSet<CARTEIRA_ASSINATURA>();
            this.CARTEIRA_CLIENTE = new HashSet<CARTEIRA_CLIENTE>();
            this.CHEQUE_DEVOLVIDO1 = new HashSet<CHEQUE_DEVOLVIDO>();
            this.CLIENTE_USUARIO = new HashSet<CLIENTE_USUARIO>();
            this.CLIENTES_ENDERECO = new HashSet<CLIENTES_ENDERECO>();
            this.CONTRATOS = new HashSet<CONTRATOS>();
            this.HIST_ATEND_EMAIL = new HashSet<HIST_ATEND_EMAIL>();
            this.HIST_ATEND_URA = new HashSet<HIST_ATEND_URA>();
            this.HISTORICO_ATENDIMENTO = new HashSet<HISTORICO_ATENDIMENTO>();
            this.ITEM_PEDIDO = new HashSet<ITEM_PEDIDO>();
            this.ITEM_PEDIDO1 = new HashSet<ITEM_PEDIDO>();
            this.MOTIVO_CANCELAMENTO = new HashSet<MOTIVO_CANCELAMENTO>();
            this.PEDIDO_CRM = new HashSet<PEDIDO_CRM>();
            this.PROPOSTA = new HashSet<PROPOSTA>();
            this.PROPOSTA_ITEM = new HashSet<PROPOSTA_ITEM>();
            this.PROPOSTA_ITEM1 = new HashSet<PROPOSTA_ITEM>();
            this.REGISTRO_FATURAMENTO = new HashSet<REGISTRO_FATURAMENTO>();
            this.URA_LOG = new HashSet<URA_LOG>();
        }
    
        public string ASN_NUM_ASSINATURA { get; set; }
        public string ASN_ANO_COAD { get; set; }
        public Nullable<int> ASN_CORTESIA { get; set; }
        public string ASN_A_C { get; set; }
        public string ASN_E_MAIL { get; set; }
        public string ASN_REMESSA { get; set; }
        public string ASN_ANO_REMESSA { get; set; }
        public string ASN_NUM_TP_ENVIO { get; set; }
        public Nullable<int> PRO_ID { get; set; }
        public Nullable<int> CLI_ID { get; set; }
        public Nullable<int> TP_ASS_ID { get; set; }
        public int ASN_QTDE_CONS_CONTRATO { get; set; }
        public int ASN_QTDE_CONS_ADICIONAL { get; set; }
        public int ASN_QTDE_CONS_UTILIZADA { get; set; }
        public Nullable<int> UEN_ID { get; set; }
        public Nullable<int> CMP_ID { get; set; }
        public string ASN_MATERIA_ADICIONAL { get; set; }
        public string ASN_ATV_REM { get; set; }
        public string ASN_NUM_ASS_TRANSFERIDA { get; set; }
        public Nullable<bool> ASN_TRANSFERIDA { get; set; }
        public bool ASN_PROTOCOLADA { get; set; }
        public string ASN_ENTREGADOR { get; set; }
    
        public virtual ICollection<AGENDA_COBRANCA> AGENDA_COBRANCA { get; set; }
        public virtual ASSINATURA_SENHA ASSINATURA_SENHA { get; set; }
        public virtual ICollection<ASSINATURA> ASSINATURA1 { get; set; }
        public virtual ASSINATURA ASSINATURA2 { get; set; }
        public virtual ICollection<CHEQUE_DEVOLVIDO> CHEQUE_DEVOLVIDO { get; set; }
        public virtual CLIENTES CLIENTES { get; set; }
        public virtual ICollection<ASSINATURA_EMAIL> ASSINATURA_EMAIL { get; set; }
        public virtual PRODUTO_COMPOSICAO PRODUTO_COMPOSICAO { get; set; }
        public virtual PRODUTOS PRODUTOS { get; set; }
        public virtual ICollection<ASSINATURA_TELEFONE> ASSINATURA_TELEFONE { get; set; }
        public virtual TIPO_PERIODO_ASSINATURA TIPO_PERIODO_ASSINATURA { get; set; }
        public virtual ICollection<ASSINATURA_TRANSFERENCIA> ASSINATURA_TRANSFERENCIA { get; set; }
        public virtual UEN UEN { get; set; }
        public virtual ICollection<CARTEIRA_ASSINATURA> CARTEIRA_ASSINATURA { get; set; }
        public virtual ICollection<CARTEIRA_CLIENTE> CARTEIRA_CLIENTE { get; set; }
        public virtual ICollection<CHEQUE_DEVOLVIDO> CHEQUE_DEVOLVIDO1 { get; set; }
        public virtual ICollection<CLIENTE_USUARIO> CLIENTE_USUARIO { get; set; }
        public virtual ICollection<CLIENTES_ENDERECO> CLIENTES_ENDERECO { get; set; }
        public virtual ICollection<CONTRATOS> CONTRATOS { get; set; }
        public virtual ICollection<HIST_ATEND_EMAIL> HIST_ATEND_EMAIL { get; set; }
        public virtual ICollection<HIST_ATEND_URA> HIST_ATEND_URA { get; set; }
        public virtual ICollection<HISTORICO_ATENDIMENTO> HISTORICO_ATENDIMENTO { get; set; }
        public virtual ICollection<ITEM_PEDIDO> ITEM_PEDIDO { get; set; }
        public virtual ICollection<ITEM_PEDIDO> ITEM_PEDIDO1 { get; set; }
        public virtual ICollection<MOTIVO_CANCELAMENTO> MOTIVO_CANCELAMENTO { get; set; }
        public virtual ICollection<PEDIDO_CRM> PEDIDO_CRM { get; set; }
        public virtual ICollection<PROPOSTA> PROPOSTA { get; set; }
        public virtual ICollection<PROPOSTA_ITEM> PROPOSTA_ITEM { get; set; }
        public virtual ICollection<PROPOSTA_ITEM> PROPOSTA_ITEM1 { get; set; }
        public virtual ICollection<REGISTRO_FATURAMENTO> REGISTRO_FATURAMENTO { get; set; }
        public virtual ICollection<URA_LOG> URA_LOG { get; set; }
    }
}