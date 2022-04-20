using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using COAD.SEGURANCA.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;

namespace COAD.SEGURANCA.Model
{
    [Mapping(Source = typeof(BANCOS_REF))]
    public partial class BancosRefDTO
    {
        public BancosRefDTO()
        {
            this.CONTA = new HashSet<ContaDTO>();
        }

        public string BAN_ID { get; set; }

        public virtual ICollection<ContaDTO> CONTA { get; set; }
    }
}