using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;

namespace COAD.SEGURANCA.Model.Dto.Custons
{
    public class ErroReportItemDTO
    {
        public ErroReportItemDTO()
        {
            ValidationErrors = new Dictionary<string, List<string>>();
        }

        public ErroReportItemDTO(params ModelStateDictionary[] models)
        {
            ValidationErrors = new Dictionary<string, List<string>>();

            if(models != null && models.Count() > 0)
            {
                foreach(var modelStat in models)
                {
                    var validations = modelStat.Where(kvp => kvp.Value.Errors.Count > 0).ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList());
                    ValidationErrors = ValidationErrors.Concat(validations).ToDictionary(x => x.Key, x => x.Value);
                }
            }
        }
        public string Contexto { get; set; }
        public string Mensagem { get; set; }
        public Dictionary<string, List<string>> ValidationErrors { get; set; }
    }
}
