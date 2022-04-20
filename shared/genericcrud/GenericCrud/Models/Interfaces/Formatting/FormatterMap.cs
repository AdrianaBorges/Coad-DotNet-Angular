using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Models.Interfaces.Formatting
{
    /// <summary>
    /// Mapea vários tokens (marcações que serão formatadas na mensage) para um formatador
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public class FormatterMap
    {
        public FormatterMap()
        {
            this.priority = 0;
            this.tokens = new List<string>();
        }

        public string key { get; set; }
        public int priority { get; set; }
        public IList<string> tokens { get; set; }

        Type type { get; set; }

        public void AddToken(string token)
        {
            this.tokens.Add(token);
        }

        public void AddAllToken(params string[] tokens)
        {
            this.tokens = tokens;
        }

        /// <summary>
        /// Indica o formatador para extrair o valor dos parametros adicionais cuja a chave é a mesma definida pelo tokem.
        /// </summary>
        public bool UtilizaParametrosAdicionais { get; set; }

        /// <summary>
        /// Especifica que o valor do campo será obtido em um campo existente no DTO RegistroHistoricoDTO
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nomeDoCampo"></param>
        public void DefinirCampoDeValor(string nomeDoCampo)
        {
            UtilizaParametrosAdicionais = false;
            this.nomeCampoValor = nomeDoCampo;
        }

        public string nomeCampoValor { get; set; }
    }
}
