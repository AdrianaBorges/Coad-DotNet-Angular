using COAD.CORPORATIVO.Model.Dto.Cep;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Script.Serialization;

namespace COADAGENDA.Components.WebServices
{
    public class CepWebService
    {
        private const string URL = "http://api.postmon.com.br/v1/cep/";

        public CepResponseDTO GetSet(string cep)
        {
            if (cep != null)
            {
                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri(URL);

                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync(cep).Result;

                if (response.IsSuccessStatusCode)
                {
                  // string resp = await response.Content.ReadAsStringAsync();

                   JavaScriptSerializer serializer = new JavaScriptSerializer();
                   CepResponseDTO cepResponse = serializer.Deserialize<CepResponseDTO>(null);
                   //var serializar = new JavaScriptSerializer();
                }
               
            }
            return null;
        }
    }
}