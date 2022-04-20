using Coad.GenericCrud.Exceptions;
using Coad.Reflection;
using GenericCrud.Metadatas;
using GenericCrud.Models.HistoryRegister;
using GenericCrud.Models.MessageFormatter;
using GenericCrud.Service.Formatting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GenericCrud.Service.HistoryRegister
{
    public abstract class HistoryRegisterService
    {
        /// <summary>
        /// Formata um objeto para um representação de string de acordo com a implementação da interface IFormatterMap
        /// </summary>
        public MessageFormatterService formatterService { get; set; }

        public HistoryRegisterService()
        {
            this.formatterService = new MessageFormatterService();
        }

        public HistoryRegisterService(MessageFormatterService messageService)
        {
            this.formatterService = messageService;
        }

        public string FormatarMensagem(RegistroHistDTO parametros, string mensagem)
        {
            if (mensagem != null)
            {
                string padrao = @"\{[^{}]*\}";
                Match match = Regex.Match(mensagem, padrao);

                mensagem = mensagem.Replace("{obs}", parametros.Observacoes);
                while (match.Success)
                {
                    string token = match.Value;
                    string tokenLimpo = token.Replace("{", "").Replace("}", "");
                    bool formatou = false;

                    if (this.formatterService != null && this.formatterService.Maps != null) // busco todos os mapeamentos de formatadores
                    {
                        foreach (var map in this.formatterService.Maps) // busco todos os mapeamentos de formatadores
                        {
                            if (map.tokens.Where(x => x == tokenLimpo).Count() > 0) // verifico se o token está mapeado para algum formatador
                            {
                                var key = map.key;

                                if (map.UtilizaParametrosAdicionais)
                                {
                                    if (parametros.ParametrosAdicionais.Where(x => x.Key == tokenLimpo).Count() > 0)
                                    {
                                        var valor = parametros.ParametrosAdicionais[tokenLimpo];
                                        var valorDosParametros = formatterService.Format(valor, key);

                                        mensagem = mensagem.Replace(token, valorDosParametros);
                                        formatou = true;
                                    }
                                }
                                else
                                {
                                    object valor = null;
                                    if (ReflectionProvider.TryGetMemberValue<object>(parametros, map.nomeCampoValor, out valor))
                                    {
                                        if (valor != null)
                                        {
                                            var valorCast = formatterService.Format(valor, key);
                                            mensagem = mensagem.Replace(token, valorCast);
                                            formatou = true;
                                        }
                                    } 
                                }
                            }
                        }

                        if (!formatou) // Se não foi encontrado nenhum formatador espeficicado para o token
                        {
                            // se não existe formatador o token é substituido por um valor literal
                            object valor = null;

                            if (ReflectionProvider.TryGetMemberValue<object>(parametros, tokenLimpo, out valor)) // verifico se a substituição será realizada por algum campo do objeto HistoricoNotificacaoDTO
                            {
                                if (valor != null)
                                {
                                    mensagem = mensagem.Replace(token, valor.ToString());
                                }
                            }
                            else // se ainda assim o token não for encontrado então busco no dicionário 'parametros adicionais'
                            {
                                if (parametros.ParametrosAdicionais != null && parametros.ParametrosAdicionais.Where(x => x.Key == tokenLimpo).Count() > 0)
                                {
                                    var valorParamentro = parametros.ParametrosAdicionais[tokenLimpo];
                                    if (valorParamentro != null)
                                    {
                                        mensagem = mensagem.Replace(token, valorParamentro.ToString());
                                    }
                                }
                            }
                        }
                    }

                    match = match.NextMatch();
                }

            }

            return mensagem;
        }


        public bool Validar(RegistroHistDTO registroHistorico, TipoRegistro tipoRegistro, StringBuilder sb)
        {
            bool valido = true;
       
            switch (tipoRegistro)
            {
                case TipoRegistro.HISTORICO_PEDIDO:
                    {

                        if (registroHistorico.PstId == null)
                        {
                            sb.Append(" Campo PstId: O campo PstId é necessário para o tipo de registro : HISTORICO_PEDIDO. \n");
                            valido = false;
                        }

                        break; 
                    }

                case TipoRegistro.HISTORICO_CLIENTE:
                    {

                        if (registroHistorico.AcaId == null)
                        {
                            sb.Append(" Campo AcaId: O campo AcaId é necessário para o tipo de registro : HISTORICO_PEDIDO. \n");
                            valido = false;
                        }

                        break;
                    }

                case TipoRegistro.NOTIFICACAO:
                    {

                        if (registroHistorico.TipoNoticacao == null)
                        {
                            sb.Append(" Campo TipoNoticacao: O campo TipoNoticacao é necessário para o tipo de registro : NOTIFICACAO. \n");
                            valido = false;
                        }
                        
                        if (registroHistorico.UrgenciaNotificacao == null)
                        {
                            sb.Append(" Campo UrgenciaNotificacao: O campo UrgenciaNotificacao é necessário para o tipo de registro : NOTIFICACAO. \n");
                            valido = false;
                        }


                        break;
                    }

            }

            return valido;

        }
        
        public void Validar(RegistroHistDTO registroHistorico, IList<TipoRegistro> lstTiposRegistro)
        {
            StringBuilder sb = new StringBuilder("Erro ao validar. ");
            bool valido = true;

            if (registroHistorico == null)
            {
                throw new ValidacaoException(" Os parametros para registrar históricos/notificações não pode ser nulas. ");
            }

            if (lstTiposRegistro == null || lstTiposRegistro.Count() <= 0)
            {
                throw new ValidacaoException(" Especique pelo menos uma maneira de registro. ");
            }
                        
            if (registroHistorico.RepId != null)
            {

                sb.Append(" Campo RepId: O campo RepId é necessário em todos os casos. \n");
                valido = false;
            }

            if (registroHistorico.RepIdQueExecutouAcao != null)
            {

                sb.Append(" Campo RepIdQueExecutouAcao: O campo RepIdQueExecutouAcao é necessário em todos os casos. \n");
                valido = false;
            }

            if (!string.IsNullOrWhiteSpace(registroHistorico.UsuLogin))
            {

                sb.Append(" Campo UsuLogin: O campo UsuLogin é necessário em todos os casos. \n");
                valido = false;
            }

            if (!lstTiposRegistro.Contains(TipoRegistro.TODOS))
            {
                valido = (valido && Validar(registroHistorico, TipoRegistro.HISTORICO_CLIENTE, sb));
                valido = (valido && Validar(registroHistorico, TipoRegistro.HISTORICO_PEDIDO, sb));
                valido = (valido && Validar(registroHistorico, TipoRegistro.NOTIFICACAO, sb));
            }
            else
            {
                foreach (var tipo in lstTiposRegistro)
                {
                    valido = (valido && Validar(registroHistorico, tipo, sb));                    
                }
            }

            if (valido)
            {
                throw new ValidacaoException(sb.ToString());
            }
        }

    
    }
}
