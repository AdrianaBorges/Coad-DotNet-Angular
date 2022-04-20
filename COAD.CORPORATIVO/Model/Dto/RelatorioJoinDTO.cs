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
    [Mapping(typeof(RELATORIO_JOIN))]
    public class RelatorioJoinDTO
    {
        public int REJ_ID { get; set; }
        public Nullable<int> RET_ID1 { get; set; }
        public Nullable<int> RET_ID2 { get; set; }
        public Nullable<int> TPJ_ID { get; set; }
        public string REJ_NOME_CAMPO1 { get; set; }
        public string REJ_NOME_CAMPO2 { get; set; }
        public Nullable<System.DateTime> DATA_EXCLUSAO { get; set; }

        public Nullable<int> REL_ID { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        [Required(ErrorMessage = "Informe a primeira tabela para o join.")]
        public virtual RelatorioTabelasDTO RELATORIO_TABELAS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        [Required(ErrorMessage = "Informe a segunda tabela para o join.")]
        public virtual RelatorioTabelasDTO RELATORIO_TABELAS1 { get; set; }
        
        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual TipoJoinDTO TIPO_JOIN { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual RelatorioPersonalizadoDTO RELATORIO_PERSONALIZADO { get; set; }
   

    }
}
