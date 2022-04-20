using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using COAD.COADGED.Service;
using System.Web.Script.Serialization;

namespace COADSERVICE
{
    /// <summary>
    /// Summary description for NoticiasService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class NoticiasService : System.Web.Services.WebService
    {
        public class ola{
            public int MyProperty { get; set; }
            public string MyProperty2 { get; set; }
        }

        [WebMethod]
        public string TrazerNoticiasPorQuantidadeEArea(string qte, string area)
        {
            NoticiaSRV ns = new NoticiaSRV();
            //ns.TrazerNoticiasPorQuantidadeEArea(qte, area);
            JavaScriptSerializer jss = new JavaScriptSerializer();

            string output = jss.Serialize(ns.TrazerNoticiasPorQuantidadeEArea(int.Parse(qte), int.Parse(area)));
            return output;
        }

        [WebMethod]
        public string TrazerNoticiasPorID(int id)
        {
            return "Hello World";
        }

        [WebMethod]
        public string TrazerNoticiasPaginadas(int id)
        {
            return "Hello World";
        }

        [WebMethod]
        public ola HelloWorld()
        {
            ola teste = new ola();
            teste.MyProperty = 1;
            teste.MyProperty2 = "Teste";
            return teste;
        }
    }
}
