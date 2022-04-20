using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO
{
    public partial class NoticiaDTO
    {
        public int NOT_ID { get; set; }
        public string NOT_MANCHETE { get; set; }
        public string NOT_TEXTO { get; set; }
        public Nullable<bool> NOT_DESTAQUE_HOME { get; set; }
        public Nullable<bool> NOT_DESTAQUE_PERFIL { get; set; }
        public Nullable<int> NGR_ID { get; set; }
        public Nullable<System.DateTime> DATA_CADASTRO { get; set; }
        public string USU_LOGIN { get; set; }
        public Nullable<System.DateTime> DATA_ALTERA { get; set; }
        public Nullable<int> TIT_ID { get; set; }
        public Nullable<int> PUB_ID { get; set; }
        public Nullable<System.DateTime> DATA_PUBLICACAO { get; set; }
        public string USU_LOGIN_PUB { get; set; }
        public Nullable<bool> NEWSLETTER { get; set; }
        public Nullable<bool> NOT_NEWS_LETTER { get; set; }

        public virtual NoticiaGrupoDTO NOTICIA_GRUPO { get; set; }
        public virtual PublicacaoDTO PUBLICACAO { get; set; }
        public virtual TitulacaoDTO TITULACAO { get; set; }


    }
}
