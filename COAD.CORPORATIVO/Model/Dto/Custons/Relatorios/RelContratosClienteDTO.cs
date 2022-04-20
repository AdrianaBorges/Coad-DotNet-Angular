using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Relatorios
{
    public partial class RelContratosClienteAreaDTO
    {
        public RelContratosClienteAreaDTO()
        {
            this.LISTAUF = new HashSet<RelContratosClienteDTO>();
        }

        public int AREA_ID { get; set; }
        public string AREA_NOME { get; set; }
        public virtual ICollection<RelContratosClienteDTO> LISTAUF { get; set; }
    }

    public class RelContratosClienteDTO
    {
        public int PRO_ID { get; set; }
        public string PRO_NOME { get; set; }
        public int AC { get; set; }
        public int AL { get; set; }
        public int AM { get; set; }
        public int AP { get; set; }
        public int BA { get; set; }
        public int CE { get; set; }
        public int DF { get; set; }
        public int ES { get; set; }
        public int FR { get; set; }
        public int GO { get; set; }
        public int MA { get; set; }
        public int MG { get; set; }
        public int MS { get; set; }
        public int MT { get; set; }
        public int PA { get; set; }
        public int PB { get; set; }
        public int PE { get; set; }
        public int PI { get; set; }
        public int PR { get; set; }
        public int RJ { get; set; }
        public int RN { get; set; }
        public int RO { get; set; }
        public int RR { get; set; }
        public int RS { get; set; }
        public int SC { get; set; }
        public int SE { get; set; }
        public int SP { get; set; }
        public int TO { get; set; }
        
    }
}
