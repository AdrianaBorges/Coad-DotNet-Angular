//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace COAD.CORPORATIVO.Repositorios.Contexto
{
    using System;
    using System.Collections.Generic;
    
    public partial class FORNECECEDOR
    {
        public FORNECECEDOR()
        {
            this.NOTA_FISCAL = new HashSet<NOTA_FISCAL>();
            this.PRODUTO_FORNECEDOR = new HashSet<PRODUTO_FORNECEDOR>();
        }
    
        public int FOR_ID { get; set; }
        public string FOR_RAZAO_SOCIAL { get; set; }
        public string FOR_NOME_FANTASIA { get; set; }
        public string FOR_TIPESSOA { get; set; }
        public string FOR_CNPJ { get; set; }
        public string FOR_ENDERECO { get; set; }
        public string FOR_END_COMPLEMENTO { get; set; }
        public string FOR_BAIRRO { get; set; }
        public Nullable<int> MUN_ID { get; set; }
        public string FOR_CEP { get; set; }
        public string FOR_TEL { get; set; }
        public string FOR_CEL { get; set; }
        public string FOR_FAX { get; set; }
        public string FOR_INSCRICAO { get; set; }
        public string FOR_CONTATO { get; set; }
        public Nullable<System.DateTime> FOR_DTMOV { get; set; }
        public Nullable<System.DateTime> FOR_DTCAD { get; set; }
        public Nullable<System.DateTime> FOR_DTNASC { get; set; }
        public string FOR_ATIVO { get; set; }
        public string FOR_EMAIL { get; set; }
        public string FOR_INSCMUNIP { get; set; }
        public string FOR_INSCSUFRAMA { get; set; }
        public Nullable<System.DateTime> DATA_CADASTRO { get; set; }
        public Nullable<System.DateTime> DATA_ALTERA { get; set; }
        public string USU_LOGIN { get; set; }
        public Nullable<int> FOR_TIPO { get; set; }
        public string FOR_END_NUMERO { get; set; }
    
        public virtual MUNICIPIO MUNICIPIO { get; set; }
        public virtual ICollection<NOTA_FISCAL> NOTA_FISCAL { get; set; }
        public virtual ICollection<PRODUTO_FORNECEDOR> PRODUTO_FORNECEDOR { get; set; }
    }
}