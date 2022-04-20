using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.COADGED.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;

namespace COAD.COADGED.Model.DTO
{
    [Mapping(Source = typeof(NOTICIAS_GUARDADAS))]
    public class NoticiasGuardadasDTO
    {
        public int ID_NOTICIA { get; set; }
        public int ID_CLIENTE { get; set; }
        public string TEXTO { get; set; }
        public Nullable<int> ID { get; set; }
    }
}
