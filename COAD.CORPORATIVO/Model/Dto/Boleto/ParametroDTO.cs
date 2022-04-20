using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoNet;

namespace COAD.CORPORATIVO.Model.Dto.Boleto
{
    /// <summary>
    /// ALT: 14/10/2016 - Parâmetros obrigatórios para gerar um boleto.
    /// [msg] - é uma mensagem definida pela empresa; exemplo: "Bom pagador tem até 35% de desconto"
    /// [preAlocado] - usado apenas para boletos avulsos, gerados sem alocação prévia do financeiro; agenda do Diego é um exemplo.
    /// [idRemessa] - é obrigatório para todo boleto [preAlocado]; fica na tabela {OCORRENCIA_REMESSA.OCM_CODIGO}
    /// </summary>
    public class ParametroDTO
    {
        private int? id_cliente;

        public int? idEmpresa { get; set; }
        public int? idConta { get; set; }
        public int? idCliente
        {
            get
            {
                return (id_cliente != null) ? 
                    id_cliente : idClienteNoContrato;
            }
          set
            {
                id_cliente = value;
            }
        }
        public int? idClienteNoContrato { get; set; }
        public string idTitulo { get; set; }
        public DateTime? dtVencimento { get; set; }
        public Decimal? vlrBoleto { get; set; }
        public string msg { get; set; }
        public bool preAlocado { get; set; }
        public string CodContrato { get; set; }
        public string idRemessa { get; set; }
        public bool? segVia { get; set; }
        public string BANCO { get; set; }
    }
}
