using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COADWEBAPI.Models
{
    public class ClienteModelMob
    {
        public string nome { get; set; }
        public string cpf { get; set; }
        public string email { get; set; }
        public string telefone { get; set; }
        public string uf { get; set; }
        public string perfil { get; set; }
    }
}