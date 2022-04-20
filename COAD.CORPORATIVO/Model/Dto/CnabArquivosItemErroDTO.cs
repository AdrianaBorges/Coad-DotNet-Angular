using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(Source = typeof(CNAB_ARQUIVOS_ITEM_ERRO))]
    public partial class CnabArquivosItemErroDTO
    {
        public int CNE_ID { get; set; }
        public string CNE_LINHA_ERRO { get; set; }
        public Nullable<int> CNQ_ID { get; set; }
        public string CNE_NUM_LINHA { get; set; }
        public string CNE_NUM_PARCELA { get; set; }
        public string CNE_ERRO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual CnabArquivosDTO CNAB_ARQUIVOS { get; set; }
    }
}
