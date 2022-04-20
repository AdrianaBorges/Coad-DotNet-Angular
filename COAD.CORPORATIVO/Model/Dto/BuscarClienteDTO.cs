using Coad.GenericCrud.Validations;
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

    [Mapping(Source = typeof(CLIENTES))]
    public class BuscarClienteDTO
    {
        public BuscarClienteDTO()
        {
            //this.ASSINATURA = new HashSet<AssinaturaBuscaDTO>();
            this.ASSINATURA_EMAIL = new HashSet<AssinaturaEmailDTO>();
            this.ASSINATURA_TELEFONE = new HashSet<AssinaturaTelefoneDTO>();
            this.ClienteNaAgenda = true;
        }
    
        public int? CLI_ID { get; set; }
        public string Codigo { get; set; }

        [DisplayName("Nome")]
        [Required(ErrorMessage = "Digite o nome do Cliente")]
        public string CLI_NOME { get; set; }
        
        [DisplayName("CNPJ/CPF")]
        [RequiredIf("CLA_CLI_ID", 2, 3,ErrorMessage = "Digite o cnpj/cpf do cliente")]
        public string CLI_CPF_CNPJ { get; set; }
        public Nullable<System.DateTime> DATA_ULTIMO_HISTORICO { get; set; }
        public Nullable<bool> CLI_EXCLUIDO_VALIDACAO { get; set; }
        
        //public virtual ICollection<AssinaturaDTO> ASSINATURA { get; set; }
        public virtual ICollection<AssinaturaEmailDTO> ASSINATURA_EMAIL { get; set; }
        public virtual ICollection<AssinaturaTelefoneDTO> ASSINATURA_TELEFONE { get; set; }
        
        public Nullable<int> CLA_CLI_ID { get; set; }

        public virtual ClassificacaoClienteDTO CLASSIFICACAO_CLIENTE { get; set; }

        /// <summary>
        /// Esse campo é derivado. Não pertence ao banco.
        /// Serve para preencher quando um cliente não está associado a agenda.
        /// </summary>
        public virtual bool ClienteNaAgenda { get; set; }
    }

}
