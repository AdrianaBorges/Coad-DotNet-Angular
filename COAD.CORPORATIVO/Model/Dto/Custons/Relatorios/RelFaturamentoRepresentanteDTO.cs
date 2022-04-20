using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Relatorios
{
    public class RelFaturamentoRepresentanteSintDTO
    {
        public int? EMP_ID { get; set; }
        public int? MES_FAT { get; set; }
        public int? ANO_FAT { get; set; }
        public int? REP_ID { get; set; }
        public string REP_NOME { get; set; }
        public string CAR_ID { get; set; }
        public int QTDE_CONTRATOS { get; set; }
        public int QTDE_PRODUTOS { get; set; }
        public int QTDE_CURSO { get; set; }
        public int QTDE_ASSINATURA { get; set; }
        public int QTDE_VENDA { get; set; }
        public int QTDE_RENOVACAO { get; set; }
        public decimal? VALOR_ASSINATURA { get; set; }
        public decimal? VALOR_CURSO { get; set; }
        public decimal? VALOR_PRODUTO { get; set; }
        public decimal? VALOR_RENOVACAO { get; set; }
        public decimal? VALOR_VENDA { get; set; }
        public decimal? VALOR_TOTAL { get; set; }
        public decimal? VALOR_RECEBIDO { get; set; }
        public decimal? PERC_RECEBIDO { get; set; }
        


    }

    public class RelFaturamentoRepresentanteDTO
    {
        public string ASN_NUM_ASSINATURA { get; set; }
        public string CAR_ID { get; set; }
        public int? CLI_ID { get; set; }
        public string CLI_NOME { get; set; }
        public string UF { get; set; }
        public DateTime? CTR_DATA_FAT { get; set; }
        public string CTR_NUM_CONTRATO { get; set; }
        public decimal? VALOR_TOTAL { get; set; }
        public DateTime? CTR_DATA_FAT_ANT { get; set; }
        public string CTR_NUM_CONTRATO_ANT { get; set; }
        public decimal? VALOR_TOTAL_ANT { get; set; }
        public int? ASN_QTDE_CONS_CONTRATO { get; set; }


        public int? MES_FAT { get; set; }
        public int? ANO_FAT { get; set; }
        public int? REP_ID { get; set; }
        public string REP_NOME { get; set; }

    }
}
