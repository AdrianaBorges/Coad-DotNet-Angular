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
    public class RepIdFormatter : IMessageFormatter
    {

        public string Format(object repId)
        {
            // Recuperando as informações dos representantes para registrar seus nomes no histórico
            var representanteQueExecutouAAcao = new RepresentanteSRV().FindById(repId);

            string nomeRepresentanteQueExecutouAAcao =
                                (representanteQueExecutouAAcao != null && !string.IsNullOrWhiteSpace(representanteQueExecutouAAcao.REP_NOME))
                                    ? representanteQueExecutouAAcao.REP_NOME : "(Nome indisponível)";

            return nomeRepresentanteQueExecutouAAcao;
        }
    }
}
