using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.FISCAL.Model.Enumerados;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(Source = typeof(IMPOSTO))]
    public class ImpostoDTO
    {    
        public ImpostoDTO()
        {
            this.CONFIG_IMPOSTO_IMPOSTO = new HashSet<ConfigImpostoImpostoDTO>();
            this.IMPOSTO_INFO_FATURA = new HashSet<ImpostoInfoFaturaDTO>();
        }

        public int? IMP_ID { get; set; }
        public string IMP_NOME { get; set; }
        public Nullable<decimal> IMP_ALIQUOTA { get; set; }
        public Nullable<bool> IMP_SOBRE_TOTAL { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<ConfigImpostoImpostoDTO> CONFIG_IMPOSTO_IMPOSTO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<ImpostoInfoFaturaDTO> IMPOSTO_INFO_FATURA { get; set; }

        public ImpostosEnum TipoImposto
        {
            get
            {

                switch (IMP_ID)
                {
                    case 1: return ImpostosEnum.IR;
                    case 2: return ImpostosEnum.PIS;
                    case 3: return ImpostosEnum.COFINS;
                    case 4: return ImpostosEnum.CSLL;
                    case 5: return ImpostosEnum.IR;
                    case 10: return ImpostosEnum.ISS;
                }

                return ImpostosEnum.OUTROS;
            }
        }

    }
}
