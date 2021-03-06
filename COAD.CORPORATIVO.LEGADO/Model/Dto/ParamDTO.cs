using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Model.Dto
{
    [Mapping(typeof(PARAM))]
    public class ParamDTO
    {
        public string PERIODO_BASE { get; set; }
        public string ANO_BASE { get; set; }
        public string PERIODO_MAXIMO { get; set; }
        public string ANO_MAXIMO { get; set; }
        public string ULT_SEQ_DIGITACAO { get; set; }
        public string MORA_MES { get; set; }
        public string NUM_CONTRATO { get; set; }
        public string SEQ_PARCELA { get; set; }
        public string ULT_PROC_BAIX_CART { get; set; }
        public string QUEBRA_SENHA { get; set; }
        public string SEQ_SENHA { get; set; }
        public string DT_ULT_SENHA { get; set; }
        public string EM_USO_SN { get; set; }
        public string SEQ_DIARIO { get; set; }
        public string IDENT_DOC { get; set; }
        public string SEQ_BX_CART { get; set; }
        public string DT_ULT_BORDERO { get; set; }
        public string DT_CNT_ACORD { get; set; }
        public string ULT_NUM_SOL_AC { get; set; }
        public string ULT_EXP_ACORDAO { get; set; }
        public string ULT_REM_SEM { get; set; }
        public string ULT_ANO_SEM { get; set; }
        public string ULT_REM_QUIZ { get; set; }
        public string ULT_ANO_QUIZ { get; set; }
        public string REGIST_INI { get; set; }
        public string ATCI_LIBERADO_SN { get; set; }
        public string ATCI_DATA_INSTRUC { get; set; }
        public string ADVI_LIBERADO_SN { get; set; }
        public string ADVI_DATA_INSTRUC { get; set; }
        public string ULT_NUM_CR { get; set; }
        public string DESP_ADM { get; set; }
        public string PERC_MALA { get; set; }
        public string SEQ_DIARIO_DEMAR_IV { get; set; }
        public string DATA_FECH_C_REC { get; set; }
        public string DATA_CONTROLE_CH_COAD { get; set; }
        public string SEQ_CONTROLE_CH_COAD { get; set; }
        public string DATA_CONTROLE_CH_ATL { get; set; }
        public string SEQ_CONTROLE_CH_ATL { get; set; }
        public Nullable<int> SEQ_IDENT { get; set; }
        public Nullable<int> SEQ_PROXIMO_RECIBO { get; set; }
        public Nullable<int> SEQ_MV { get; set; }
        public Nullable<int> SEQ_REGISTRO { get; set; }
        public Nullable<int> NUM_BORDERO_MV { get; set; }
        public Nullable<int> ID_LIQ_MV { get; set; }
        public Nullable<int> SEQ_VALE { get; set; }
        public string RODOU_PROG_LIMITE_SEMANAL { get; set; }
        public Nullable<int> SEQ_SUSPECT { get; set; }
        public string COD_FILIAL { get; set; }
        public string ULT_SEQ_CART_CURSO { get; set; }
        public string SEQ_ARQ_EMPRESTIMO { get; set; }
        public string SEQ_PARCELA2 { get; set; }
        public Nullable<int> LOTE_REMESSA { get; set; }
        public string SENHA_SAC { get; set; }
        public string SENHA_SAC_GERENCIA { get; set; }
        public Nullable<int> SEQ_ARQ_CAIXA { get; set; }
        public string LOG_DIARIO { get; set; }
        public int? ID { get; set; }
    }
}
