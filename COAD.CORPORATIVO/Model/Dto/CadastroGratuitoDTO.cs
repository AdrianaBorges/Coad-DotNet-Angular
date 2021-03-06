using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(Source = typeof(CADASTRO_GRATUITO))]
    public class CadastroGratuitoDTO
    {
        public int CGR_ID { get; set; }
        public string CGR_TELEFONE { get; set; }
        public string CGR_CELULAR { get; set; }
        public string UF_SIGLA { get; set; }
        public string CGR_PERFIL { get; set; }
        public Nullable<System.DateTime> CGR_DATA_EXPIRA { get; set; }
        public string CGR_SENHA { get; set; }
        public Nullable<System.DateTime> CGR_DATA_CADASTRO { get; set; }
        public string CGR_EMAIL { get; set; }
        public string CGR_NOME { get; set; }
        public string CGR_PERMISSAO { get; set; }
        public string CGR_CPF_CNPJ { get; set; }
        public string CGR_ENDERECO { get; set; }
        public string CGR_NUMERO { get; set; }
        public string CGR_COMPLEMENTO { get; set; }
        public string CGR_BAIRRO { get; set; }
        public string CGR_CEP { get; set; }
        public string CGR_MUNICIPIO { get; set; }
        public Nullable<System.DateTime> CGR_EXPIRACAO { get; set; }
        public string CGR_OAB_NRINSCRICAO { get; set; }
        public string CGR_OAB_NRFICHA { get; set; }
        public string CGR_OAB_STATUS { get; set; }
        public Nullable<int> CGR_OAB_FLAG { get; set; }
        public string CGR_LOGIN { get; set; }
        public string CRG_TIPO_USUARIO { get; set; }

        [IgnoreMemberMapping(MappingDirection.Both)]
        public virtual UFDTO UF { get; set; }
    }

}
