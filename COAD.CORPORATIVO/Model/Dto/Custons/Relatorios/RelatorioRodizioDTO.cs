using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Relatorios
{
    public class RelatorioRodizioDTO
    {
        public int? REP_ID { get; set; }
        public string REP_NOME { get; set; }
        public int? CLI_ID { get; set; }
        public DateTime Data { get; set; }
        public int Ordem { get; set; }
        public int PassivosRecebidos { get; set; }
        public int PreReservasRecebidas { get; set; }
        public int Encaminhados { get; set; }
        public int EncaminhadosPR { get; set; }
        public int Importados { get; set; }
        public int? RG_ID { get; set; } 
        public string RG_DESCRICAO { get; set; }
    }
}
