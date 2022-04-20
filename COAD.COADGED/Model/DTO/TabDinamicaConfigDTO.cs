using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO
{
    public class TabDinamicaConfigDTO
    {
        public TabDinamicaConfigDTO()
        {
            this.TAB_DINAMICA_CONFIG_ITEM = new HashSet<TabDinamicaConfigItemDTO>();
            this.TAB_DINAMICA_LINK = new HashSet<TabDinamicaLinkDTO>();
            this.TAB_DINAMICA_ORIGEM = new HashSet<TabDinamicaOrigemDTO>();
            this.TAB_DINAMICA_PUBLICACAO = new HashSet<TabDinamicaPublicacaoDTO>();
        }

        //------
        public string TDC_ID { get; set; }
        public bool TDC_INTRODUCAO { get; set; }
        public bool TDC_OBS_LAYOUT { get; set; }
        public bool TDC_LEGENDA_LAYOUT { get; set; }
        public bool TDC_INF_PREENCHIMENTO { get; set; }
        public bool TDC_INF_GERAIS { get; set; }
        public Nullable<System.DateTime> TDC_DATA_ALTERA { get; set; }
        public Nullable<System.DateTime> TDC_DATA_INCLUSAO { get; set; }
        public string USU_LOGIN { get; set; }
        [Required(ErrorMessage = "Informe o Nome da Tabela (Campo obrigatório)")]
        [MaxLength(33, ErrorMessage = "Nome da Tabela (Campo obrigatório. max. 33 caracteres)"), MinLength(1)]
        public string TDC_NOME_TABELA { get; set; }
        public Nullable<int> TDC_TIPO { get; set; }
        public string TDC_ID_TAB_REF { get; set; }
        public Nullable<System.DateTime> TDC_DATA_PUBLICACAO { get; set; }
        public Nullable<int> TGR_ID { get; set; }
        public Nullable<int> TTA_ID { get; set; }
        public Nullable<int> TIT_ID { get; set; }
        public string TDC_LAYOUT_TABELA { get; set; }
        public Nullable<bool> TDC_PALAVRA_CHAVE { get; set; }
        public string USU_LOGIN_PUB { get; set; }
        public string TDC_NOME_CHAVEDB { get; set; }
        public string TDC_NOME_LISTADB { get; set; }
        [Required(ErrorMessage = "Informe o Nome da Tabela (Campo obrigatório)")]
        [MaxLength(200, ErrorMessage = "Nome da Tabela (Campo obrigatório. max. 200 caracteres)"), MinLength(1)]
        public string TDC_NOME_OBS { get; set; }
        public string TDC_NOME_IDENTIFICADOR { get; set; }

        public virtual TabDinamicaDTO TAB_DINAMICA { get; set; }
        public virtual ICollection<TabDinamicaConfigItemDTO> TAB_DINAMICA_CONFIG_ITEM { get; set; }
        public virtual TabDinamicaGrupoDTO TAB_DINAMICA_GRUPO { get; set; }
        public virtual ICollection<TabDinamicaLinkDTO> TAB_DINAMICA_LINK { get; set; }
        public virtual TitulacaoDTO TITULACAO { get; set; }
        public virtual ICollection<TabDinamicaOrigemDTO> TAB_DINAMICA_ORIGEM { get; set; }
        public virtual ICollection<TabDinamicaPublicacaoDTO> TAB_DINAMICA_PUBLICACAO { get; set; }


    }
}
