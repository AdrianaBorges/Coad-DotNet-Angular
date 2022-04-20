using COAD.CORPORATIVO.Service;
using GenericCrud.Models.Interfaces.Formatting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Formatters
{
    /// <summary>
    /// Esse formater pega um repId recupera o representante do banco de retorna seu nome
    /// </summary>
    public class CliIdFormatter : IMessageFormatter
    {

        public string Format(object cliId)
        {
            var cliente = new ClienteSRV().FindById(cliId);

            string nomeCliente =
                                (cliente != null && !string.IsNullOrWhiteSpace(cliente.CLI_NOME))
                                    ? cliente.CLI_NOME : "(Nome indisponível)";

            return nomeCliente;
        }
    }
}
