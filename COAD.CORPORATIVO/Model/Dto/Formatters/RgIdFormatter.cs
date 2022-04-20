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
    public class RgIdFormatter : IMessageFormatter
    {

        public string Format(object rgId)
        {
            // Recuperando as informações dos representantes para registrar seus nomes no histórico
            var regiao = new RegiaoSRV().FindById(rgId);

            string nomeRegiao =
                                (regiao != null && !string.IsNullOrWhiteSpace(regiao.RG_DESCRICAO))
                                    ? regiao.RG_DESCRICAO : "(Nome indisponível)";

            return nomeRegiao;
        }
    }
}
