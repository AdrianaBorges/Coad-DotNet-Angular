using COAD.CORPORATIVO.Model.Dto.Formatters;
using GenericCrud.Service.Formatting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service.Custons
{
    public static class FormatterServiceLocalFactory
    {
        public static MessageFormatterService CriarMessageFormatterServiceCoorporativo()
        {
            MessageFormatterService formatterService = new MessageFormatterService();

            var repIdMapper = formatterService.AddFormater("repId", new RepIdFormatter());

            repIdMapper.AddAllToken("representante", "representanteQueExecutouAcao");
            repIdMapper.DefinirCampoDeValor("RepId");

            var cliIdMapper = formatterService.AddFormater("cliId", new CliIdFormatter());

            cliIdMapper.AddToken("cliente");
            cliIdMapper.DefinirCampoDeValor("CliId");

            var rgIdMapper = formatterService.AddFormater("rgId", new RgIdFormatter());

            rgIdMapper.AddToken("regiao");
            rgIdMapper.UtilizaParametrosAdicionais = true;

            return formatterService;
        
        }
    }
}
