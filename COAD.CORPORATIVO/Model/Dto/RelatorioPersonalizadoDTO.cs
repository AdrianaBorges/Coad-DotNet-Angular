using Coad.GenericCrud.Validations;
using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(RELATORIO_PERSONALIZADO))]
    public class RelatorioPersonalizadoDTO
    {
        public RelatorioPersonalizadoDTO()
        {
            this.RELATORIO_TABELA_COLUNAS = new HashSet<RelatorioTabelaColunasDTO>();
            this.RELATORIO_TABELAS = new HashSet<RelatorioTabelasDTO>(); 
            this.RELATORIO_JOIN = new HashSet<RelatorioJoinDTO>();
            this.RELATORIO_CONDICAO = new HashSet<RelatorioCondicaoDTO>();       
        }
    
        public int? REL_ID { get; set; }
        public Nullable<int> REP_ID { get; set; }
        public string USU_LOGIN { get; set; }
        public string REL_DESCRICAO { get; set; }
        public string REL_SQL_GERADO { get; set; }
        public Nullable<System.DateTime> DATA_EXCLUSAO { get; set; }
        public Nullable<bool> RET_RAW_QUERY { get; set; }

        private Nullable<bool> _RET_RELATORIO_BASE;
        public Nullable<bool> RET_RELATORIO_BASE { 
            get 
            {
                return _RET_RELATORIO_BASE;
            } 
            set 
            {
                _RET_RELATORIO_BASE = value;

                if (value == null || value == false)
                {
                    IgnoreValidation = true;
                }
            } 
        }
        
        public Nullable<int> REL_ID_PAI { get; set; }

        public bool IgnoreValidation { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        [RequiredListIf("IgnoreValidation", 1, false, ErrorMessage = "Informe no mínimo uma coluna na consulta.")]
        public virtual ICollection<RelatorioTabelaColunasDTO> RELATORIO_TABELA_COLUNAS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        [RequiredListIf("IgnoreValidation", 1, false, ErrorMessage = "Informe no mínimo uma tabela na consulta.")]
        public virtual ICollection<RelatorioTabelasDTO> RELATORIO_TABELAS { get; set; }
        
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual RepresentanteDTO REPRESENTANTE { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<RelatorioJoinDTO> RELATORIO_JOIN { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<RelatorioCondicaoDTO> RELATORIO_CONDICAO { get; set; }

        private RelatorioPersonalizadoDTO _relatorioPai;
        
        public RelatorioPersonalizadoDTO RelatorioPai {
            get {
                return _relatorioPai;
            }
            set
            {
                _relatorioPai = value;

                if (_relatorioPai != null)
                {
                    _relatorioPai.IgnoreValidation = true;
                }
            }
        }

        public ICollection<RelatorioTabelaColunasDTO> Agrupamento
        {
            get 
            {
                if (RELATORIO_TABELAS != null)
                {
                    return RELATORIO_TABELA_COLUNAS
                        .Where(x => x.COR_AGRUPAR == true)
                        .OrderBy(x => x.COR_ORDENACAO)
                        .Select(x => x)
                        .ToList();
                }

                return new List<RelatorioTabelaColunasDTO>();
            }
            set { 
                
            }
        }

    }
}
