using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(Source = typeof(CNAB_ARQUIVOS))]
    public partial class CnabArquivosDTO
    {
        public CnabArquivosDTO()
        {
            this.CNAB_ARQUIVOS_ITEM_ERRO = new HashSet<CnabArquivosItemErroDTO>();
            this.CNAB_ARQUIVOS_ITEM = new HashSet<CnabArquivosItemDTO>();
            this.PARCELAS = new HashSet<ParcelasDTO>();
        }

        public int CNQ_ID { get; set; }
        public int EMP_ID { get; set; }
        public string BAN_ID { get; set; }
        public int CTA_ID { get; set; }
        public int CNQ_QTD_LINHAS { get; set; }
        public System.DateTime CNQ_DATA_ARQUIVO { get; set; }
        public string CNQ_NOME { get; set; }
        public byte[] CNQ_ARQUIVO { get; set; }
        public Nullable<System.DateTime> CNQ_DATA_LIDO { get; set; }
        public Nullable<System.DateTime> CNQ_DATA_PROCESSADO { get; set; }
        public Nullable<System.DateTime> CNQ_DATA_ESTORNADO { get; set; }
        public string USU_LOGIN { get; set; }
        public System.DateTime DATA_CADASTRO { get; set; }
        public string CNQ_COD_CEDENTE { get; set; }
        public string CNQ_EMP_CEDENTE { get; set; }
        public string CNQ_TIPO_ARQUIVO { get; set; }
        public string CNQ_LINHA_ARQUIVO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual BancosDTO BANCOS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ContaRefDTO CONTA_REF { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual EmpresaRefDTO EMPRESA_REF { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ParcelasDTO> PARCELAS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<CnabArquivosItemDTO> CNAB_ARQUIVOS_ITEM { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<CnabArquivosItemErroDTO> CNAB_ARQUIVOS_ITEM_ERRO { get; set; }
        


    }
}
