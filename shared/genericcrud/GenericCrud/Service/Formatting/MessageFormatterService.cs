using Coad.Reflection;
using GenericCrud.Models.HistoryRegister;
using GenericCrud.Models.Interfaces.Formatting;
using GenericCrud.Models.MessageFormatter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GenericCrud.Service.Formatting
{
    
    /// <summary>
    /// Formata um objeto para um representação de string de acordo com a implementação da interface IFormatterMap
    /// </summary>
    public class MessageFormatterService
    {
        public Dictionary<string, IMessageFormatter> Formatters = new Dictionary<string, IMessageFormatter>();
        public IList<FormatterMap> Maps = new List<FormatterMap>();
        
        public FormatterMap AddFormater(string key, IMessageFormatter formatter)
        {
            Formatters.Add(key, formatter);
            
            var map = new FormatterMap() { 
                key = key
            };
            this.Maps.Add(map);

            return map;

        }

        public string Format<TSource>(TSource obj, string key = null)
        {
            var formater = Formatters[key];

            if (formater != null)
            {
                string message = null;
                message = formater.Format(obj);
                                
                return message;
            }

            return null;
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

                    if (this.Maps != null) // busco todos os mapeamentos de formatadores
                    {
                        foreach (var map in this.Maps) // busco todos os mapeamentos de formatadores
                        {
                            if (map.tokens.Where(x => x == tokenLimpo).Count() > 0) // verifico se o token está mapeado para algum formatador
                            {
                                var key = map.key;

                                if (map.UtilizaParametrosAdicionais)
                                {
                                    if (parametros.ParametrosAdicionais.Where(x => x.Key == tokenLimpo).Count() > 0)
                                    {
                                        var valor = parametros.ParametrosAdicionais[tokenLimpo];
                                        var valorDosParametros = Format(valor, key);

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
                                            var valorCast = Format(valor, key);
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


    }
}
