using COAD.SEGURANCA.Model;
using GenericCrud.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.FiltersInfo
{
    [QueryFilter(typeof(NotaFiscalDTO))]
    public class PesquisaNotaFiscalDTO
    {
        public PesquisaNotaFiscalDTO()
        {
            Pagina = 1;
            RegistrosPorPagina = 15;
        }

        public DateTime? DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }


        [QueryFilterAgrupamento("Padrão")]
        [QueryFilterTexto("NF_NUMERO", "Número da Nota")]
        [QueryFilterOrdem(0)]
        public int? NumeroNota { get; set; }

        [QueryFilterAgrupamento("Padrão")]
        [QueryFilterTexto("CLIENTES.CLI_CPF_CNPJ", "CNPJ")]
        [QueryFilterOrdem(1)]
        public string CNPJ { get; set; }

        [QueryFilterAgrupamento("Padrão")]
        [QueryFilterSelect(typeof(EmpresaModel), "EMP_ID", "EMP_NOME_FANTASIA", "EMP_ID", "Empresa")]
        [QueryFilterOrdem(2)]
        public int? EmpID { get; set; }

        public int? Tipo { get; set; }

        [QueryFilterAgrupamento("Avançado")]
        [QueryFilterTexto("NF_CHAVE", "Chave")]
        [QueryFilterOrdem(4)]
        public string ChaveNota { get; set; }
        public int Pagina { get; set; }
        public int RegistrosPorPagina { get; set; }


    }
}
