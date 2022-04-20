using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Config.DataAttributes.Maps
{
    public enum MappingDirection
    {

        /// <summary>
        /// Entity => DTO (Consultas)
        /// </summary>
        /// 
        SourceToDestiny = 1,

        /// <summary>
        /// DTO => Entity (Atualizações, Deleções e etc)
        /// </summary>
        DestinyToSource = 2,

       /// <summary>
       /// Entity => DTO, DTO => Entity (Ignora nos dois sentidos)
       /// </summary>
        Both = 3
    }
}
