using GenericCrud.Models.HistoryRegister;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Models.MessageFormatter
{
    public class RegistroHistDTO
    {

        public RegistroHistDTO()
        {
            this.TiposDeRegistro = new List<TipoRegistro>();
            this.MensagensIndividuais = new List<MensagemIndividual>();
            this.ParametrosAdicionais = new Dictionary<string, object>();
        }

        /// <summary>
        /// A mensagem que será exibida. 
        /// Se preenchido adiciona a mesma mensagem para todas os registros (históricos, notificações)
        /// </summary>
        public string Mensagem { get; set; }
        
        /// <summary>
        /// Controle interno para identificar se a mensagem é individual o geral
        /// </summary>
        public bool Individual = true;

        /// <summary>
        /// Mensagens específicas por tipos
        /// </summary>
        public IList<MensagemIndividual> MensagensIndividuais {get; set;}

        /// <summary>
        /// Login do usuário logado <em></em>
        /// Tipo de Registro: Todos
        /// </summary>
        public string UsuLogin { get; set; }
        
        /// <summary>
        /// Id do representante que receberá o histórico ou notificação
        /// Tipo de Registro: Todos
        /// </summary>
        public int? RepId { get; set; }

        /// <summary>
        /// Id do representante que executou a ação e gerou histórico
        /// Tipo de Registro: Todos
        /// </summary>
        public int? RepIdQueExecutouAcao { get; set; }
        
        /// <summary>
        /// Id do cliente
        /// </summary>
        public int? CliId { get; set; }
        
        
        /// <summary>
        /// Id do pedidoCRM
        /// </summary>
        public int? PedCrmId { get; set; }

        /// <summary>
        /// Id do item de pedido
        /// Tipo de Registro: HISTORICO_PEDIDO
        /// </summary>
        public int? IpeId { get; set; }

        /// <summary>
        /// Id da proposta item
        /// Tipo de Registro: HISTORICO_PEDIDO
        /// </summary>
        public int? PpiId { get; set; }

        /// <summary>
        /// Status do pedido quando o evento ocorreu
        /// Tipo de Registro: HISTORICO_PEDIDO
        /// </summary>
        public int? PstId { get; set; }
        
        /// <summary>
        /// Id do Tipo de Histórico de cliente
        /// Tipo de Registro: HISTORICO_CLIENTE
        /// </summary>
        public int? AcaId { get; set; }

        /// <summary>
        /// Id do tipo de notificação
        /// Tipo de Registro: NOTIFICAÇÃO
        /// </summary>
        public int TipoNoticacao { get; set; }

        /// <summary>
        /// Id da Urgencia da notificação
        /// Tipo de Registro: NOTIFICAÇÃO
        /// </summary>
        public string UrgenciaNotificacao { get; set; }
        
        /// <summary>
        /// Texto que será substituído pelo token {obs} no texto da mensagem
        /// </summary>
        public string Observacoes { get; set; }

        /// <summary>
        /// Valores adicionais que não estão presentes nativamente neste DTO que serão substuídos no texto da mensagem
        /// </summary>
        public Dictionary<string, object> ParametrosAdicionais { get; set; }
        

        /// <summary>
        /// Define quais tipos de registros serão levados em conta no evento de registro
        /// </summary>
        public IList<TipoRegistro> TiposDeRegistro { get; set; }

        /// <summary>
        /// Se todas as mensagens são a mesmas utilize esse método para identificar quais tipos de registros serão inseridos.
        /// </summary>
        /// <param name="lstTipoRegistro"></param>
        public void DefinirTipos(params TipoRegistro[] lstTipoRegistro)
        {
            if(lstTipoRegistro != null)
            {
                this.TiposDeRegistro = this.TiposDeRegistro.Concat(lstTipoRegistro).ToList();
            }            
        }


        /// <summary>
        /// Utilize esse método para registrar mensagens específicas por cada tipo.
        /// Não é necessário utilizar 'Definir Tipos' se utilizar essa e vise-versa.
        /// </summary>
        /// <param name="lstTipoRegistro"></param>
        public void AddMensagensIndividuais(TipoRegistro tipoRegistro, string mensagem)
        {
            Individual = false;
            if(tipoRegistro.Equals(TipoRegistro.TODOS))
            {
                throw new Exception("O tipo de registro 'TODOS' é inválido para esse tipo e mensagem.");
            }

            MensagemIndividual mensagemIndividual = new MensagemIndividual(){
                
                TipoRegistro = tipoRegistro,
                Mensagem = mensagem
            };

            MensagensIndividuais.Add(mensagemIndividual);
        }
        
    }
}
