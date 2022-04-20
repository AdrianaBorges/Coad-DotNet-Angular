using COAD.CORPORATIVO.Model.DTO;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.SEGURANCA.Model;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(REPRESENTANTE))]
    public class RepresentanteDTO
    {
        public RepresentanteDTO()
        {
            this.AGENDAMENTO = new HashSet<AgendamentoDTO>();
            this.AREA_CONSULTORIA_REPRESENTANTE = new HashSet<AreaConsultoriaRepresentanteDTO>();
            this.CARTEIRA_REPRESENTANTE = new HashSet<CarteiraRepresentanteDTO>();
            this.HISTORICO_ATENDIMENTO = new HashSet<HistoricoAtendimentoDTO>();
            this.HISTORICO_PEDIDO = new HashSet<HistoricoPedidoDTO>();
            this.NOTIFICACOES = new HashSet<NotificacoesDTO>();
            this.PEDIDO_CRM = new HashSet<PedidoCRMDTO>();
            this.PROPOSTA = new HashSet<PropostaDTO>();
            this.REGISTRO_FATURAMENTO = new HashSet<RegistroFaturamentoDTO>();  
            this.REGISTRO_LIBERACAO = new HashSet<RegistroLiberacaoDTO>();
            this.IMPORTACAO_HISTORICO = new HashSet<ImportacaoHistoricoDTO>();
            this.IMPORTACAO_RESULTADO_RODIZIO = new HashSet<ImportacaoResultadoRodizioDTO>();
            this.APURACAO_PREMIACAO_MENSAL = new HashSet<ApuracaoPremiacaoMensalDTO>();
            this.APURACAO_PREMIACAO_SEMANA = new HashSet<ApuracaoPremiacaoSemanaDTO>();
            this.REPRESENTANTE_META = new HashSet<RepresentanteMetaDTO>();
            this.SEMANA_PREMIACAO_REPR = new HashSet<SemanaPremiacaoReprDTO>();

        }

        private string carId { get; set; }
        public int? REP_ID { get; set; }
        public string REP_OPER_ID { get; set; }
        public string REP_NOME { get; set; }

        public string CAR_ID { 
            get 
            {
                return carId; 
            }
            set {
                if (value == null)
                    carId = "0";
                else
                    carId = value;
            } 
    }
        
        public Nullable<int> AREA_ID { get; set; }
        public string REP_MATRICULA { get; set; }
        public Nullable<int> REP_ATIVO { get; set; }
        public int REP_VARIAS_CARTEIRAS { get; set; }
        public Nullable<int> UEN_ID { get; set; }
        public Nullable<int> RG_ID { get; set; }
        public Nullable<bool> REP_GERENTE { get; set; }
        public Nullable<bool> REP_ADMINISTRATIVO { get; set; }
        public string REP_COD_CARTEIRA_ANTIGO { get; set; }
        public Nullable<int> NRP_ID { get; set; }
        public Nullable<int> EMP_ID { get; set; }
        public string REGIAO_UF { get; set; }
        public string REP_EMAIL { get; set; }
        public string REP_RAMAL { get; set; }
        public string REP_DDD_TELEFONE { get; set; }
        public string REP_TELEFONE { get; set; }
        public string REP_DDD_TELEFONE2 { get; set; }
        public string REP_TELEFONE2 { get; set; }
        public Nullable<int> REP_ID_SUPERVISOR { get; set; }
        public Nullable<bool> REP_SUPERVISOR { get; set; }
        public Nullable<bool> REP_INATIVO_RODIZIO_IMP { get; set; }


        public UsuarioModel USUARIO { get; set; }
        /// <summary>
        /// Esse atributo é uma representação do nome da operadora, no banco configsys.
        /// Ou seja, é um atributo derivado de consultas suplementares
        /// </summary>
        public int OP_NOME { get; set; }
    
        public virtual CarteiraDTO CARTEIRA { get; set; }
        //public virtual ICollection<ContratoDTO> CONTRATOS { get; set; }
        //public virtual UF UF { get; set; }


        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<CarteiraRepresentanteDTO> CARTEIRA_REPRESENTANTE { get; set; }
        public virtual ICollection<AgendamentoDTO> AGENDAMENTO { get; set; }
        public virtual UENDTO UEN { get; set; }
        public virtual ICollection<HistoricoAtendimentoDTO> HISTORICO_ATENDIMENTO { get; set; }
        public virtual ICollection<NotificacoesDTO> NOTIFICACOES { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<FilaCadastroDTO> FILA_CADASTRO { get; set; }
        public virtual RegiaoDTO REGIAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<PedidoCRMDTO> PEDIDO_CRM { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual NivelRepresentanteDTO NIVEL_REPRESENTANTE { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<AreaConsultoriaRepresentanteDTO> AREA_CONSULTORIA_REPRESENTANTE { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<HistoricoPedidoDTO> HISTORICO_PEDIDO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<PropostaDTO> PROPOSTA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<PropostaDTO> PROPOSTA1 { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<PedidoCRMDTO> PEDIDO_CRM1 { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<RegistroFaturamentoDTO> REGISTRO_FATURAMENTO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<RegistroLiberacaoDTO> REGISTRO_LIBERACAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ImportacaoHistoricoDTO> IMPORTACAO_HISTORICO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ImportacaoResultadoRodizioDTO> IMPORTACAO_RESULTADO_RODIZIO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<RepresentanteMetaDTO> REPRESENTANTE_META { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ApuracaoPremiacaoMensalDTO> APURACAO_PREMIACAO_MENSAL { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ApuracaoPremiacaoSemanaDTO> APURACAO_PREMIACAO_SEMANA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<SemanaPremiacaoReprDTO> SEMANA_PREMIACAO_REPR { get; set; }

    }
}
