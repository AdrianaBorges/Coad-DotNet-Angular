using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(NOTA_FISCAL_ITEM))]
    public class NotaFiscalItemDTO
    {
        public int NF_ID { get; set; }
        public int NF_TIPO { get; set; }
        public int NF_NUMERO { get; set; }
        public string NF_SERIE { get; set; }
        public int PRO_ID { get; set; }
        public Nullable<decimal> NFI_QTDE { get; set; }
        public Nullable<decimal> NFI_VLR_UNIT { get; set; }
        public Nullable<decimal> NFI_VLR_TOTAL { get; set; }
        public Nullable<decimal> NFI_BASE_CALC_ICMS { get; set; }
        public Nullable<decimal> NFI_VLR_ICMS { get; set; }
        public Nullable<decimal> NFI_VLR_IPI { get; set; }
        public Nullable<decimal> NFI_ALIQ_ICMS { get; set; }
        public Nullable<decimal> NFI_ALIQ_IPI { get; set; }
        public string CFOP { get; set; }
        public string NFI_UN { get; set; }
        public string NFI_PRO_NOME { get; set; }
        public string CST_ID { get; set; }
        public Nullable<decimal> NFI_ALIQ_ISSQN { get; set; }
        public Nullable<decimal> NFI_BCALC_ISSQN { get; set; }
        public Nullable<decimal> NFI_VLR_ISSQN { get; set; }
        public Nullable<decimal> NFI_BC_RET { get; set; }
        public Nullable<decimal> NFI_BC_ST_ORIG_DEST { get; set; }
        public Nullable<decimal> NFI_ICMS_RET { get; set; }
        public Nullable<decimal> NFI_ICMS_ST_COMPL { get; set; }
        public Nullable<decimal> NFI_ICMS_ST_REP { get; set; }
        public string NCM_ID { get; set; }
        public string NFI_DISCRIMINACAO_SERVICO { get; set; }
        public Nullable<System.DateTime> DATA_EXCLUSAO { get; set; }
        public Nullable<int> CMP_ID { get; set; }

        public virtual CFOTableDTO CFOP_TABLE { get; set; }
        public virtual CSTDTO CST { get; set; }
        public virtual NotaFiscalDTO NOTA_FISCAL { get; set; }
        public virtual NotaFiscalItemOBSDTO NOTA_FISCAL_ITEM_OBS { get; set; }
        public virtual ProdutosDTO PRODUTOS { get; set; }
        public virtual UnidadeMedidaDTO UNIDADE_MEDIDA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ProdutoComposicaoDTO PRODUTO_COMPOSICAO { get; set; }
    }
}
