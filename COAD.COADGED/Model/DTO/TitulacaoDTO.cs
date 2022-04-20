using COAD.COADGED.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO
{
    [Mapping(Source = typeof(TITULACAO))]
    public class TitulacaoDTO
    {

        public TitulacaoDTO()
        {
            this.NOTICIA = new HashSet<NoticiaDTO>();
            this.TAB_DINAMICA_CONFIG = new HashSet<TabDinamicaConfigDTO>();
            this.TITULACAO1 = new HashSet<TitulacaoDTO>();
        }

    
        [DisplayName("ID")]
        public Nullable<int> TIT_ID { get; set; }

        [DisplayName("Descrição")]
        [Required(ErrorMessage = "Por favor, informe uma descrição para esta titulação!")]
        [MaxLength(200, ErrorMessage = "Por favor, informe no máximo 50 caracteres para a titulação!")]
        public string TIT_DESCRICAO { get; set; }

        [DisplayName("Ativo")]
        [Required(ErrorMessage = "Por favor, informe se esta titulação está ativa escolhendo [Sim] ou [Não]!")]
        public Nullable<int> TIT_ATIVO { get; set; }
        
        [DisplayName("Tipo")]
        [Required(ErrorMessage = "Por favor, informe se esta titulação é do tipo [Grande Grupo, Verbete ou Subverbete]!")]
        public string TIT_TIPO { get; set; }

        [DisplayName("Titulação Pai")]
        public Nullable<int> TIT_ID_REFERENCIA { get; set; }

        [DisplayName("Colecionador")]
        [Required(ErrorMessage = "Por favor, informe o colecionador!")]
        public Nullable<int> ARE_CONS_ID { get; set; }

        [DisplayName("UF")]
        public string UF_ID { get; set; }

        public virtual AreasDTO AREAS_CONSULTORIA { get; set; }
        public virtual ICollection<NoticiaDTO> NOTICIA { get; set; }
        public virtual ICollection<TabDinamicaConfigDTO> TAB_DINAMICA_CONFIG { get; set; }
        public virtual ICollection<TitulacaoDTO> TITULACAO1 { get; set; }
        public virtual TitulacaoDTO TITULACAO2 { get; set; }
        public virtual UfDTO UF_REF { get; set; }
    }
}
