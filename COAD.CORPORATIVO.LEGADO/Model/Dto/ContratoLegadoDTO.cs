using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Model.Dto
{
    [Mapping(typeof(CONTRATOS))]
    public class ContratoLegadoDTO
    {
        public string CONTRATO { get; set; }
        public string ASSINATURA { get; set; }
        public string ANO_VIGENCIA { get; set; }
        public string PEDIDO { get; set; }
        public string ANO_FAT { get; set; }
        public string PERIODO_FAT { get; set; }
        public string SEMANA_FAT { get; set; }
        public string DATA_FAT { get; set; }
        public string ANO_PROD { get; set; }
        public string PERIODO_PROD { get; set; }
        public string DATA_PRODUCAO { get; set; }
        public string AREA { get; set; }
        public string REGIAO { get; set; }
        public string REPRESENTANTE { get; set; }
        public string VLR_ENTRADA { get; set; }
        public string VLR_PARC_REST { get; set; }
        public string QTE_PARC_REST { get; set; }
        public string DATA_CANC { get; set; }
        public string PART_BTC { get; set; }
        public string EMISS_CONTRA { get; set; }
        public string QTE_MES_VIG { get; set; }
        public string DATA_REAT { get; set; }
        public string DATA_FIM_VIGENCIA { get; set; }
        public string DATA_AUTORIZACAO { get; set; }
        public string LIXO { get; set; }
        public string DT_OFERTA_1 { get; set; }
        public string DT_OFERTA_2 { get; set; }
        public string DT_OFERTA_3 { get; set; }
        public string DT_OFERTA_ZERO { get; set; }
        public string DT_DIARIO_FAT { get; set; }
        public string DT_OFERTA_4 { get; set; }
        public string ULT_MOD_RP { get; set; }
        public string DATA_RP { get; set; }
        public string INDICACAO { get; set; }
        public string PROSPECTADO { get; set; }
        public string DATA_ATRIBUICAO { get; set; }
        public string COD_CONSULTA { get; set; }
        public string CONTRATO_NEGOCIADO { get; set; }
        public string SEM_ENTRADA { get; set; }
        public string DT_CANCELAR { get; set; }
        public string DH_SUBIR { get; set; }
        public string DH_SUBIU { get; set; }
        public Nullable<int> EMPRESA_ID { get; set; }
        public Nullable<System.DateTime> DATA_INSERT { get; set; }
        public Nullable<System.DateTime> DATA_ALTERA { get; set; }
        public string INCLUIR_TAXA_ADM { get; set; }
        public Nullable<bool> VENDA_RECORRENTE { get; set; }
        public Nullable<short> DIA_VENCIMENTO { get; set; }
    }
}
