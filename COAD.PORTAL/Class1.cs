using COAD.PORTAL.Config;
using COAD.PORTAL.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PORTAL
{
    public class Class1
    {
        public static void Main(string[] args)
        {
            PortalConfig.Configurar();

            var _srv = new CoAreasSRV();
            var lstAreas = _srv.FindAll();

            Console.WriteLine("Listando Areas ... ");

            if (lstAreas != null)
            {
                foreach (var area in lstAreas)
                {
                    Console.WriteLine(area.NOME_AREA);
                }
            }
           
            Console.ReadLine();
        }
    }
}
