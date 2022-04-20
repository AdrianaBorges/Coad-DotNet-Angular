using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Cep
{
    public class CepResponseDTO
    {
        public string bairro { get; set; }
        public string cidade { get; set; }
        public CidadeInfoDTO cidade_info { get; set; }
        public string estado { get; set; }
        public EstadoInfoDTO estado_info { get; set; }
        public string logradouro { get; set; }
    }
}

/**
 * 
 * 
 * 
 * bairro: "Jardim Metrópole"
cep: "25575414"
cidade: "São João de Meriti"
cidade_info: {area_km2: "35,216", codigo_ibge: "3305109"}
estado: "RJ"
estado_info: {area_km2: "43.777,954", codigo_ibge: "33", nome: "Rio de Janeiro"}
logradouro: "Avenida Portugal"
*/