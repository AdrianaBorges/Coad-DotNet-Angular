using COAD.CORPORATIVO.Model.Dto.Custons.Mundipagg;
using COAD.CORPORATIVO.Settings.Mundipagg;
using COAD.SEGURANCA.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service.Base
{
    public abstract class MundipaggSRV
    {

        public ParametrosSRV _parametrosSRV = new ParametrosSRV();


        public abstract MundipaggStt MundipaggSTT { get; set; }

        public ParametrosChaveMundipaggDTO ParametrosChaveMundipaggDTO { get { return BuscarParametrosMundipagg(); } }


        private ParametrosChaveMundipaggDTO BuscarParametrosMundipagg()
        {

            string value = System.Configuration.ConfigurationManager.AppSettings["COADCORP.AMBIENTE"];
            var produtoChave = new ParametrosChaveMundipaggDTO();
            if (string.Equals(value.ToUpper(), "PROD"))
            {
                produtoChave.MerchantKey = _parametrosSRV.BuscarValorString("MUNDIPAGG.PRO.COADPAG.MERCHANTKEY");
                produtoChave.PublicMerchantKey = _parametrosSRV.BuscarValorString("MUNDIPAGG.PRO.COADPAG.PUBLICMERCHANTKEY");
                produtoChave.SecretKey = _parametrosSRV.BuscarValorString("MUNDIPAGG.PRO.SECRETKEY");
                produtoChave.PublicKey = _parametrosSRV.BuscarValorString("MUNDIPAGG.PRO.PUBLICKEY");
            }
            else
            {
                produtoChave.MerchantKey = _parametrosSRV.BuscarValorString("MUNDIPAGG.HOM.COADPAG.MERCHANTKEY");
                produtoChave.PublicMerchantKey = _parametrosSRV.BuscarValorString("MUNDIPAGG.HOM.COADPAG.PUBLICMERCHANTKEY");
                produtoChave.SecretKey = _parametrosSRV.BuscarValorString("MUNDIPAGG.HOM.SECRETKEY");
                produtoChave.PublicKey = _parametrosSRV.BuscarValorString("MUNDIPAGG.HOM.PUBLICKEY");
            }
            return produtoChave;
        }






    }
}
