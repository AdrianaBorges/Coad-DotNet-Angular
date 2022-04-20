using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(RELATORIO_CONDICAO))]
    public class RelatorioCondicaoDTO
    {
        public RelatorioCondicaoDTO()
        {
            this.RELATORIO_CONDICAO_RELATORIO_OPERADOR_CONDICIONAL = new HashSet<RelatorioCondicaoRelatorioOperadorCondicionalDTO>();
        }
    
        public int? REC_ID { get; set; }
        public string REC_OPERADOR_CONDICIONAL { get; set; }
        public string REC_CAMPO { get; set; }
        public Nullable<int> ROL_ID { get; set; }
        public string REC_VALOR { get; set; }
        public Nullable<int> RTV_ID { get; set; }
        public Nullable<int> REL_ID { get; set; }
        public Nullable<int> RET_ID { get; set; }
        public Nullable<System.DateTime> DATA_EXCLUSAO { get; set; }
        public Nullable<bool> REC_FILTRO { get; set; }
        public string REC_NOME_TIPO { get; set; }
        public string REC_LABEL_FILTRO { get; set; }

        public string TableAlias { get; set; }
        
        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual RelatorioOperadorLogicoDTO RELATORIO_OPERADOR_LOGICO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual RelatorioPersonalizadoDTO RELATORIO_PERSONALIZADO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual RelatorioTabelasDTO RELATORIO_TABELAS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual RelatorioTipoDadoDTO RELATORIO_TIPO_DADO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<RelatorioCondicaoRelatorioOperadorCondicionalDTO> RELATORIO_CONDICAO_RELATORIO_OPERADOR_CONDICIONAL { get; set; }

        
    }
}
