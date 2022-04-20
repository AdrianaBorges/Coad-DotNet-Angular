using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Model.Dto
{
    [Mapping(typeof(liquidacao))]
    public class LiquidacaoLegadoDTO
    {
        public string CONTRATO { get; set; }
        public string LETRA { get; set; }
        public string CD { get; set; }
        public string TIPO_DOC { get; set; }
        public string NUMERO { get; set; }
        public string BANCO { get; set; }
        public string NAUT { get; set; }
        public string DATA { get; set; }
        public string DT_VALIDAD { get; set; }
        public string PRACA { get; set; }
        public string VALOR { get; set; }
        public string DATA_DA_BAIXA { get; set; }
        public string NUM_ARQ { get; set; }
        public string ORIGEM_PGTO { get; set; }
        public string IDENT_DOCTO { get; set; }
        public string SEQ_BX_CART { get; set; }
        public string DT_BORDERO { get; set; }
        public string CHEQUE_EMITIDO_SN { get; set; }
        public string DT_EMISSAO_CHEQUE { get; set; }
        public string NUM_RECIBO { get; set; }
        public string NUM_LEITORA_CHEQUE { get; set; }
        public string DATA_LAYOUT_CHEQUE { get; set; }
        public string DH_SUBIR { get; set; }
        public string DH_SUBIU { get; set; }
        public string PAR_NUM_PARCELA { get; set; }
        public Nullable<bool> atualizarCodigo { get; set; }
    }
}
