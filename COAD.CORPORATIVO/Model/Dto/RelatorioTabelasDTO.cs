using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(RELATORIO_TABELAS))]
    public class RelatorioTabelasDTO : IEquatable<RelatorioTabelasDTO>
    {
        public RelatorioTabelasDTO()
        {
            this.RELATORIO_TABELA_COLUNAS = new HashSet<RelatorioTabelaColunasDTO>();
        }
    
        public int? RET_ID { get; set; }
        public string RET_NOME_TABELA { get; set; }
        public Nullable<int> REL_ID { get; set; }
        public string RET_SCHEMA { get; set; }
        public Nullable<System.DateTime> DATA_EXCLUSAO { get; set; }

        public IList<RelatorioTabelaColunasDTO> Colunas { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual RelatorioPersonalizadoDTO RELATORIO_PERSONALIZADO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<RelatorioTabelaColunasDTO> RELATORIO_TABELA_COLUNAS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<RelatorioJoinDTO> RELATORIO_JOIN { get; set; }
        
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<RelatorioJoinDTO> RELATORIO_JOIN1 { get; set; }
        
        public bool Equals(RelatorioTabelasDTO other)
        {
            if (other != null)
            {
                if (string.IsNullOrEmpty(this.RET_NOME_TABELA) || string.IsNullOrEmpty(other.RET_NOME_TABELA))
                    return false;

                return this.RET_NOME_TABELA.Equals(other.RET_NOME_TABELA);

            }
            return false;
        }
    }
}
