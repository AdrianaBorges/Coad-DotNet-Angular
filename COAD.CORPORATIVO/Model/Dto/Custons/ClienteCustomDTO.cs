using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class ClienteCustomDTO
    {
        public Nullable<int> CLI_ID { get; set; }
        public string ASN_NUM_ASSINATURA { get; set; }
        public string CLI_NOME { get; set; }
        public string CLI_CPF_CNPJ { get; set; }
        public string CLA_CLI_ID { get; set; }
        public string CTR_ANO_VIGENCIA { get; set; }
        public string SITUACAO { get; set; }
        public Nullable<DateTime> CTR_DATA_INI_VIGENCIA { get; set; }
        public Nullable<DateTime> CTR_DATA_FIM_VIGENCIA { get; set; }
        public Nullable<DateTime> CTR_DATA_FAT { get; set; }
        public Nullable<int> PAR_QTDE_ABERTO { get; set; }
        public Nullable<int> QTDE_CONTRATOS { get; set; }
        public Nullable<int> QTDE_RENOVACAO { get; set; }
        public string ASN_ANO_COAD { get; set; }
        public string END_UF { get; set; }
        public Nullable<decimal> CTR_VLR_CONTRATO { get; set; }
        public Nullable<decimal> CTR_VLR_BRUTO { get; set; }
        public string AEM_EMAIL { get; set; }


    }
}