using Newtonsoft.Json;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Mundipagg
{
    public class EnderecoMundipaggDTO
    {
        [JsonProperty("number")]
        public string Number { get; set; }
        [JsonProperty("street")]
        public string Street{ get; set; }
        [JsonProperty("neighborhood")]
        public string Neighborhood { get; set; }

        [JsonProperty("line_1")]
        public string LineOne { get { return $"{this.Number}, {this.Street}, {this.Neighborhood}"; } set { } }
        [JsonProperty("line_2")]
        public string LineTwo { get; set; }
        [JsonProperty("zip_code")]
        public string ZipCode { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("city_id")]
        public string CityId { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("country")]
        public string country { get; set; }

    }
}