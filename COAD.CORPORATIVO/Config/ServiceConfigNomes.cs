using GenericCrud.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Config
{
    public class ServiceConfigNomes
    {
        public static readonly ConfigName COOPORATIVO = new ConfigName() { Name = "default" };
        public static readonly ConfigName COADSYS = new ConfigName() { Name = "coadsys" };
        public static readonly ConfigName COORPORATIVO_LEGADO = new ConfigName() { Name = "corp_old" };
        public static readonly ConfigName COORPORATIVO_PROSPECT = new ConfigName() { Name = "prospectados" };
    }
}
