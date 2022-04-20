using Coad.GenericCrud.Validations;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using GenericCrud.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Script.Serialization;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(Source = typeof(CLIENTES))]
    public class ClienteDto : ICliente
    {
        public ClienteDto()
        {
            this.ASSINATURA = new HashSet<AssinaturaDTO>();
            this.AUDITORIA = new HashSet<AuditoriaDTO>();
            this.CARTEIRA_CLIENTE = new HashSet<CarteiraClienteDTO>();
            this.CLIENTES_ENDERECO = new HashSet<ClienteEnderecoDto>();
            this.CLIENTES_HISTORICO = new HashSet<ClienteHistoricoDTO>();
            this.CONTATOS = new HashSet<ContatoDTO>();
            this.NOTA_FISCAL = new HashSet<NotaFiscalDTO>();
            this.ASSINATURA_EMAIL = new HashSet<AssinaturaEmailDTO>();
            this.ASSINATURA_TELEFONE = new HashSet<AssinaturaTelefoneDTO>();
            this.ASSINATURA_TRANSFERENCIA = new HashSet<AssinaturaTransferenciaDTO>();
            this.CLIENTES_TELEFONE = new HashSet<ClienteTelefoneDTO>();
            this.AGENDAMENTO = new HashSet<AgendamentoDTO>();
            this.NOTIFICACOES = new HashSet<NotificacoesDTO>();
            this.PEDIDO_CRM = new HashSet<PedidoCRMDTO>();
            this.PEDIDO_PARTICIPANTE = new HashSet<PedidoParticipanteDTO>();
            this.CLIENTE_USUARIO = new HashSet<ClienteUsuarioDTO>();
            this.REGISTRO_LIBERACAO = new HashSet<RegistroLiberacaoDTO>();
            this.NOTA_FISCAL_LOTE_ITEM = new HashSet<NotaFiscalLoteItemDTO>();
            this.CARRINHO_COMPRAS = new HashSet<CarrinhoComprasDTO>();

        }
    
        public int? CLI_ID { get; set; }


        [DisplayName("Nome")]
        [Required(ErrorMessage = "Digite o nome do Cliente")]
        [TextValidator(ErrorMessage = "O nome não pode possuir caracteres especiais e espaço no início e fim do texto.")]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "O Nome deve possuir no máximo 60 caracteres")]
        public string CLI_NOME { get; set; }

        [DisplayName("Aos Cuidados")]   
        //[Required(ErrorMessage = "Digite o campo 'ao cuidados' ('AC')")]
        public string CLI_A_C { get; set; }

        [DisplayName("Tipo de Pessoa")]
        public string CLI_TP_PESSOA { get; set; }


        [DisplayName("CNPJ/CPF")]
        [RequiredIf("CLA_CLI_ID", 2, 3,ErrorMessage = "Digite o cnpj/cpf do cliente")]
        [RequiredIf("ValidacaoTotal", true, ErrorMessage = "Digite o cnpj/cpf do cliente")]
        [StringLengthIf(11, "TIPO_CLI_ID", 2, ErrorMessage = "O CPF deve conter 11 caracteres")]
        [StringLengthIf(14, "TIPO_CLI_ID", 1, 3, 4, ErrorMessage = "O CNPJ deve conter 14 caracteres")]
        [CpfOrCnpjValidator("EhPessoaJuridica", ErrorMessage = "CPF/CNPJ inválido.")]
        public string CLI_CPF_CNPJ { get; set; }

        [DisplayName("Inscrição Estadual")]
        [StringLength(13, MinimumLength = 2, ErrorMessage = "O campo [IE] Inscrição estadual deve possuir no mínimo 2 e no máximo 13 caracteres")]
        [TextValidator(ErrorMessage = "O Campo [IE] Inscrição estadual possui caracteres especiais.")]
        public string CLI_INSCRICAO { get; set; }

        public Nullable<bool> CLI_ISENTO_INSCRICAO { get; set; }

        public string MXM_CODIGO { get; set; }
        public Nullable<int> CODIGO_ANTIGO { get; set; }
        public Nullable<System.DateTime> DATA_CADASTRO { get; set; }
        public Nullable<System.DateTime> DATA_ALTERA { get; set; }
        public Nullable<System.DateTime> DATA_EXCLUSAO { get; set; }
        public Nullable<System.DateTime> DATA_ULTIMO_HISTORICO { get; set; }
        public string SITUACAO { get; set; }

        public string USU_LOGIN { get; set; }
        public Nullable<int> CLA_ID { get; set; }

        [DisplayName("Tipo de Cliente")]
        [Required(ErrorMessage = "Selecione o tipo de Cliente")]
        public Nullable<int> TIPO_CLI_ID { get; set; }

        [DisplayName("Suframa")]
        public string CLI_SUFRAMA { get; set; }

        [DisplayName("Código do País")]
        public string CLI_COD_PAIS { get; set; }

        public Nullable<int> CLA_CLI_ID { get; set; }

        [EmailAddress(ErrorMessage = "O E-Mail digitado não é válido")]
        public string CLI_EMAIL { get; set; }
        
        public Nullable<bool> CLI_IMPORTADO { get; set; }
        public Nullable<bool> CLI_EXCLUIDO_VALIDACAO { get; set; }
        public string CLI_CAR_ID_PROSPECT { get; set; }
        public ClienteEnderecoDto ENDERECO_ENTREGA { get; set; }
        public ClienteEnderecoDto ENDERECO_FATURAMENTO { get; set; }
        public string CLI_NOME_RESPONSAVEL_EMPRESA { get; set; }
        public string CLI_CPF_RESPONSAVEL_EMPRESA { get; set; }
        public Nullable<int> IPS_ID { get; set; }

        public virtual ICollection<AssinaturaDTO> ASSINATURA { get; set; }
        public virtual ICollection<AssinaturaTransferenciaDTO> ASSINATURA_TRANSFERENCIA { get; set; }
        public virtual ICollection<AuditoriaDTO> AUDITORIA { get; set; }
        public virtual ICollection<CarteiraClienteDTO> CARTEIRA_CLIENTE { get; set; }
        public virtual ClassificacaoDTO CLASSIFICACAO { get; set; }
        public virtual ClassificacaoClienteDTO CLASSIFICACAO_CLIENTE { get; set; }

        public virtual string CTR_NUM_CONTRATO { get; set; }
        public virtual Nullable<DateTime> CTR_DATA_CANC { get; set; }
        public virtual Nullable<DateTime> CTR_DATA_FAT { get; set; }
        public virtual Nullable<DateTime> CTR_DATA_INI_VIGENCIA { get; set; }
        public virtual Nullable<DateTime> CTR_DATA_FIM_VIGENCIA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        [RequiredListIf("ValidacaoTotal", true, ErrorMessage = "O Cliente deve possuir pelo menos 1 endereço")]
        public virtual ICollection<ClienteEnderecoDto> CLIENTES_ENDERECO { get; set; }
        public virtual ICollection<ClienteHistoricoDTO> CLIENTES_HISTORICO { get; set; }
        public virtual TipoClienteDTO TIPO_CLIENTE { get; set; }
        public virtual ICollection<ContatoDTO> CONTATOS { get; set; }
        public virtual ICollection<NotaFiscalDTO> NOTA_FISCAL { get; set; }
        public Nullable<int> PROSP_ID { get; set; }

        //[RequiredListIf("ValidacaoTotal", true, ErrorMessage = "O Cliente deve possuir pelo menos 1 E-Mail")]
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<AssinaturaEmailDTO> ASSINATURA_EMAIL { get; set; }

        //[RequiredListIf("ValidacaoTotal", true,ErrorMessage = "O Cliente deve possuir pelo menos 1 telefone")]
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<AssinaturaTelefoneDTO> ASSINATURA_TELEFONE { get; set; }

        public virtual InfoMarketingDTO INFO_MARKETING { get; set; }
        public virtual ICollection<ClienteTelefoneDTO> CLIENTES_TELEFONE { get; set; }
        public virtual ICollection<AgendamentoDTO> AGENDAMENTO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<NotificacoesDTO> NOTIFICACOES { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<HISTORICO_ATENDIMENTO> HISTORICO_ATENDIMENTO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<PedidoCRMDTO> PEDIDO_CRM { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<PedidoParticipanteDTO> PEDIDO_PARTICIPANTE { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ClienteUsuarioDTO> CLIENTE_USUARIO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<RegistroLiberacaoDTO> REGISTRO_LIBERACAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ImportacaoSuspectDTO IMPORTACAO_SUSPECT { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<NotaFiscalLoteItemDTO> NOTA_FISCAL_LOTE_ITEM { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<CarrinhoComprasDTO> CARRINHO_COMPRAS { get; set; }


        //--- Campos imcluidos no dto para atender a consulta de cliente. 
        //--- Não existem na tabela.
        public bool CLI_CONTRATO_ATIVO { get; set; }
        public bool CLI_CONTRATO_PRORROGADO { get; set; }
        public int CLI_PARCELAS_ATRASO { get; set; }
        public string ASN_NUM_ASSINATURA { get; set; }

        public Nullable<bool> CLI_FLAG_VALIDO { get; set; }
        public Nullable<int> SUS_ID_REF { get; set; }
        public string CLI_SENHA { get; set; }
        public string CLI_LOGIN { get; set; }

        //Esse atributo não existe no banco
        public string CodigoProspect { get; set; }



        /// <summary>
        /// Esse campo é derivado. Não pertence ao banco.
        /// Serve para preencher quando um representante pertence a carteira do cliente.
        /// </summary>
        public virtual bool PodeEditar {get; set;}
        public virtual bool Validar { get; set; }
        public bool ValidacaoTotal { get; set; }
        /// <summary>
        /// Esse campo não faz parte da tabela
        /// </summary>
        public int? RegiaoIdParaRodizio { get; set; }

        /// <summary>
        /// Esse campo não faz parte da tabela
        /// </summary>
        public int? RepIdParaRodizio { get; set; }
        public bool ValidarCPF_CNPJ { get; set; }

        public bool? EhPessoaJuridica()
        {
            if (ValidarCPF_CNPJ)
            {
                if (TIPO_CLI_ID != null)
                {
                    return (TIPO_CLI_ID != 2);
                }
                else
                {
                    if (!string.IsNullOrEmpty(CLI_CPF_CNPJ))
                    {
                        if (CLI_CPF_CNPJ.Length == 14)
                            return true;
                        else return false;
                    }
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Esse campo é derivado. Não pertence ao banco.
        /// Serve para preencher quando um cliente não está associado a agenda.
        /// </summary>
        public virtual bool ClienteExisteNaAgenda { get; set; }

        [ScriptIgnore]
        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public string CNPJ_CPF
        {
            get { return this.CLI_CPF_CNPJ; }
        }

        [ScriptIgnore]
        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public IEnumerable<string> TELEFONE
        {
            get 
            {
                if (ASSINATURA_TELEFONE != null && ASSINATURA_TELEFONE.Count > 0)
                {
                    return this.ASSINATURA_TELEFONE
                        .Where(x => x.TIPO_TEL_ID == 4)
                        .Select(sel => sel.ATE_DDD + sel.ATE_TELEFONE)
                        .ToList();
                }
                return null;
            }
        }


        [ScriptIgnore]
        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public IEnumerable<string> FAX
        {
            get {

                if (ASSINATURA_TELEFONE != null && ASSINATURA_TELEFONE.Count > 0)
                {
                    return this.ASSINATURA_TELEFONE
                        .Where(x => x.TIPO_TEL_ID == 2)
                        .Select(sel => sel.ATE_DDD + sel.ATE_TELEFONE)
                        .ToList();
                }
                return null;
            }
        }

        [ScriptIgnore]
        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public IEnumerable<string> CELULAR
        {
            get {

                if (ASSINATURA_TELEFONE != null && ASSINATURA_TELEFONE.Count > 0)
                {
                    return this.ASSINATURA_TELEFONE
                        .Where(x => x.TIPO_TEL_ID == 1)
                        .Select(sel => sel.ATE_DDD + sel.ATE_TELEFONE)
                        .ToList();
                }
                return null;
            }
        }


        [ScriptIgnore]
        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public IEnumerable<string> EMAIL
        {
            get
            {
                if (ASSINATURA_EMAIL != null && ASSINATURA_EMAIL.Count > 0)
                {
                    return this.ASSINATURA_EMAIL.Select(sel => sel.AEM_EMAIL);
                }
                return null;
            }
        }
    }
}
