using COAD.FISCAL.Model.Integracoes.Interfaces;
using COAD.SEGURANCA.Service.Custons.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.Servicos.Retornos
{
    public class ProcessamentoEventoRetorno
    {
        public ProcessamentoEventoRetorno()
        {
            ProcessamentoEventoItens = new HashSet<ProcessamentoEventoItem>();
            BatchContext = new BatchContext();
        }

        public ICollection<ProcessamentoEventoItem> ProcessamentoEventoItens { get; set; }
        public BatchContext BatchContext { get; set; } 
    }
}
