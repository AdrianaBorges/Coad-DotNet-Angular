using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO
{
    public class TabDinamicaDTO
    {
        public TabDinamicaDTO()
        {
            this.TAB_DINAMICA_ITEM = new HashSet<TabDinamicaItemDTO>();
        }

        public string TDC_ID { get; set; }

        [Required(ErrorMessage = "Informe a descrição (Campo obrigatório)")]
        [MaxLength(200, ErrorMessage="Descrição (Campo obrigatório. max. 200 caracteres)"),MinLength(1)]
        public string TAB_DESCRICAO { get; set; }
        public string TAB_INTRODUCAO { get; set; }
        public string TAB_OBS_LAYOUT { get; set; }
        public string TAB_LEGENDA_LAYOUT { get; set; }
        public string TAB_INF_PREENCHIMENTO { get; set; }
        public string TAB_INF_GERAIS { get; set; }
        public Nullable<System.DateTime> TAB_DATA_ALTERA { get; set; }
        public Nullable<System.DateTime> TAB_DATA_INCLUSAO { get; set; }
        public string USU_LOGIN { get; set; }
        public string TAB_ERRO_MSG { get; set; }
        public string TAB_NOME_CHAVEDB { get; set; }
        public string TAB_NOME_LISTADB { get; set; }
        //public Nullable<System.DateTime> TAB_DATA_PUBLICACAO { get; set; }

        public virtual ICollection<TabDinamicaItemDTO> TAB_DINAMICA_ITEM { get; set; }
        public virtual TabDinamicaConfigDTO TAB_DINAMICA_CONFIG { get; set; }


    }
}
