using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(CARTEIRA_REPRESENTANTE))]
    public class CarteiraRepresentanteDTO
    {
        [Required(ErrorMessage = "Preencha a carteira")]
        public string CAR_ID { get; set; }
        public int REP_ID { get; set; }

        //[Required(ErrorMessage ="Informe a Empresa")]
        public int? EMP_ID { get; set; }
        public string REP_OPER_ID { get; set; }
        public Nullable<System.DateTime> DATA_ASSOCIACAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual CarteiraDTO CARTEIRA { get; set; }
        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual EmpresaRefDTO EMPRESA_REF { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual RepresentanteDTO REPRESENTANTE { get; set; }

        /// <summary>
        /// Esse campo não existe no banco
        /// </summary>
        public bool Deletar { get; set; }

    }
}
