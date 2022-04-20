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
    public class PublicacaoConfigDTO
    {
        // Publicar a Ementa \\
        public Boolean publicarEmenta { get; set; }

        [DisplayName("ID")]
        public Nullable<int> PCF_ID { get; set; }

        [DisplayName("Nº Matéria")]
        public Nullable<int> PUB_ID { get; set; }

        [DisplayName("Colecionador")]
        [Required(ErrorMessage = "Por favor, informe o Colecionador!")]
        public Nullable<int> ARE_CONS_ID { get; set; }

        [DisplayName("Tipo de alteração")]
        public Nullable<int> PCF_TIPO_ALTERACAO { get; set; }

        [DisplayName("Indice alfabético")]
        public Nullable<int> PCF_INDICE_ALFA { get; set; }

        [DisplayName("Indice numérico dos atos")]
        public Nullable<int> PCF_INDICE_NUM { get; set; }

        [DisplayName("Indice revogação/alteração")]
        public Nullable<int> PCF_INDICE_REVOGADO { get; set; }

        [DisplayName("Publicar na Web?")]
        public Nullable<int> PCF_PUB_WEB { get; set; }

        [DisplayName("Publicar no Informativo?")]
        public Nullable<int> PCF_PUB_IMPRESSO { get; set; }

        [DisplayName("Publicar na NEWS?")]
        public Nullable<int> PCF_PUB_NEWS { get; set; }

        [DisplayName("Publicar em Mobile?")]
        public Nullable<int> PCF_PUB_MOBILE { get; set; }

        [DisplayName("Publicar na Consultoria?")]
        public Nullable<int> PCF_PUB_CONSULTORIA { get; set; }

        [DisplayName("Publicar no Webservice?")]
        public Nullable<int> PCF_PUB_WEBSERVICE { get; set; }

        [DisplayName("Publicou na Web")]
        public Nullable<System.DateTime> PCF_DATA_PUB_WEB { get; set; }

        [DisplayName("Publicou no Informativo")]
        public Nullable<System.DateTime> PCF_DATA_PUB_IMPRESSO { get; set; }

        [DisplayName("Publicou na NEWS")]
        public Nullable<System.DateTime> PCF_DATA_PUB_NEWS { get; set; }

        [DisplayName("Publicou no Mobile")]
        public Nullable<System.DateTime> PCF_DATA_PUB_MOBILE { get; set; }

        [DisplayName("Publicou na Consultoria")]
        public Nullable<System.DateTime> PCF_DATA_PUB_CONSULTORIA { get; set; }

        [DisplayName("Publicou no Webservice")]
        public Nullable<System.DateTime> PCF_DATA_PUB_WEBSERVICE { get; set; }

        [DisplayName("Publicou a Manchete")]
        public Nullable<System.DateTime> PCF_DATA_PUB_MANCHETE { get; set; }

        [DisplayName("Publicou a Ementa")]
        public Nullable<System.DateTime> PCF_DATA_PUB_EMENTA { get; set; }

        [DisplayName("Cadastrado")]
        public Nullable<System.DateTime> DATA_CADASTRO { get; set; }

        [DisplayName("Alterado")]
        public Nullable<System.DateTime> DATA_ALTERACAO { get; set; }

        [DisplayName("Usuário")]
        public string USU_LOGIN { get; set; }

        public virtual PublicacaoAreaConsultoriaDTO PUBLICACAO_AREAS_CONSULTORIA { get; set; }
    }
}
