using GenericCrud.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Model
{
    public class PerfilUsuarioModel : IPrototype<PerfilUsuarioModel>
    {
        public int? EMP_ID { get; set; }
        public string PER_ID { get; set; }
        public string USU_LOGIN { get; set; }
        public Nullable<int> PUS_DEFAULT { get; set; }
        public Nullable<int> PUS_ATIVO { get; set; }
        public Nullable<bool> PERFIL_CLONAVEL { get; set; }
        public Nullable<bool> PUS_INSERIDO_EXTERNAMENTO { get; set; }

        public virtual PerfilModel PERFIL { get; set; }
        public virtual UsuarioModel USUARIO { get; set; }



        public PerfilUsuarioModel Clone()
        {
            PerfilUsuarioModel perfil_usuario = new PerfilUsuarioModel(){
                EMP_ID = this.EMP_ID,
                PER_ID = this.PER_ID,
                USU_LOGIN = this.USU_LOGIN,
                PERFIL = this.PERFIL,
                PUS_DEFAULT = this.PUS_DEFAULT,
                PUS_ATIVO = this.PUS_ATIVO,           
                USUARIO = this.USUARIO
            };

            return perfil_usuario;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}