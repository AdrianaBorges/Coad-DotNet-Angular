using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Model
{
    [Mapping(typeof(TEMPLATE_HTML))]
    public class TemplateHTMLDTO
    {
        public TemplateHTMLDTO()
        {
            this.TEMPLATE_HTML1 = new HashSet<TemplateHTMLDTO>();
            this.FUNCIONALIDADE_SISTEMA = new HashSet<FuncionalidadeSistemaDTO>();
        }

        public int? TPL_ID { get; set; }


        [Required(ErrorMessage = "Informe a descrição do Template")]
        public string TPL_DESCRICAO { get; set; }
        public string TPL_HTML { get; set; }

        [Required(ErrorMessage = "Informe o Grupo do Template")]
        public Nullable<int> TGR_ID { get; set; }
        public Nullable<int> TPL_ID_LAYOUT { get; set; }
        public Nullable<bool> LAYOUT { get; set; }
        public Nullable<System.DateTime> DATA_EXCLUIR { get; set; }
        public Nullable<int> FDA_ID { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual TemplateGrupoDTO TEMPLATE_GRUPO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public FonteDadosTemplateDTO FONTE_DADOS_TEMPLATE { get; set; }

        //Templates usados pelo layout
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<TemplateHTMLDTO> TEMPLATE_HTML1 { get; set; }

        // Layout que esse template utilize
        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual TemplateHTMLDTO TEMPLATE_HTML2 { get; set; }


        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<FuncionalidadeSistemaDTO> FUNCIONALIDADE_SISTEMA { get; set; }

        public FuncionalidadeSistemaDTO Funcionalidade { get; set; }
        public string HTMLCompleto {
            get {

                var html = (!string.IsNullOrWhiteSpace(TPL_HTML)) ? TPL_HTML : string.Empty;

                if(TEMPLATE_HTML2 != null)
                {
                    if (!string.IsNullOrWhiteSpace(TEMPLATE_HTML2.TPL_HTML))
                    {
                        var htmlTemplate = TEMPLATE_HTML2.TPL_HTML;
                        html = htmlTemplate.Replace("{{corpo}}", html);
                    }
                }
                return html;
            }
            set { }
        }

    }
}
