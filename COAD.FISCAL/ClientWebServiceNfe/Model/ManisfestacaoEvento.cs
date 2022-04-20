using COAD.FISCAL.Model.Servicos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model
{
    public class EventoRequest
    {
        public string idLote { get; set; }
        public IList<Evento> Eventos { get; set; }

        public EventoRequest()
        {
            Eventos = new List<Evento>();
        }
    }
}
