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
    
    public partial class IMPORTACAO_SUSPECT
    {
        public IMPORTACAO_SUSPECT()
        {
            this.CLIENTES = new HashSet<CLIENTES>();
            this.IMPORTACAO_HISTORICO = new HashSet<IMPORTACAO_HISTORICO>();
        }
    
        public int IPS_ID { get; set; }
        public string IPS_NOME { get; set; }
        public string IPS_CPF_CNPJ { get; set; }
        public string IPS_CONTATO { get; set; }
        public string IPS_TELEFONE { get; set; }
        public string IPS_FAX { get; set; }
        public string IPS_CELULAR { get; set; }
        public string IPS_EMAIL { get; set; }
        public string IPS_UF { get; set; }
        public string IPS_CIDADE { get; set; }
        public string IPS_BAIRRO { get; set; }
        public string IPS_CLASSIFICACAO { get; set; }
        public string IPS_REGIAO { get; set; }
        public Nullable<int> RG_ID { get; set; }
        public Nullable<int> CMP_ID { get; set; }
        public Nullable<int> O_CAD_ID { get; set; }
        public Nullable<int> AREA_ID { get; set; }
        public Nullable<int> IMS_ID { get; set; }
        public Nullable<int> IMP_ID { get; set; }
        public Nullable<System.DateTime> IMP_DATA_ULTIMA_EXECUCAO { get; set; }
        public string IPS_TIPO_CLIENTE { get; set; }
        public string IPS_PRODUTO_INTERESSE { get; set; }
        public string IPS_ORIGEM_CADASTRO { get; set; }
        public string IPS_AREA_INTERESSE { get; set; }
        public string IPS_COMENTARIO_CLIENTE { get; set; }
        public Nullable<int> IPS_REP_ID { get; set; }
        public string IPS_TIPO_IMPORTACAO { get; set; }
    
        public virtual AREAS AREAS { get; set; }
        public virtual ICollection<CLIENTES> CLIENTES { get; set; }
        public virtual IMPORTACAO IMPORTACAO { get; set; }
        public virtual ICollection<IMPORTACAO_HISTORICO> IMPORTACAO_HISTORICO { get; set; }
        public virtual IMPORTACAO_STATUS IMPORTACAO_STATUS { get; set; }
        public virtual ORIGEM_CADASTRO ORIGEM_CADASTRO { get; set; }
        public virtual PRODUTO_COMPOSICAO PRODUTO_COMPOSICAO { get; set; }
        public virtual REGIAO REGIAO { get; set; }
    }
}
