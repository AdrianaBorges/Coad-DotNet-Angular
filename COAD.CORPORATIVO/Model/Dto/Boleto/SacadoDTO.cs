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
    /// ALT: 11/10/2016 - Trata-se do Cliente da COAD.
    /// </summary>
    public class SacadoDTO : Sacado
    {
        public SacadoDTO() { }
        public SacadoDTO(string nome) { base.Nome = nome; }
        public SacadoDTO(string cpfcnpj, string nome) { base.CPFCNPJ = cpfcnpj; base.Nome = nome; }
        public SacadoDTO(string cpfcnpj, string nome, Endereco endereco) { base.CPFCNPJ = cpfcnpj; base.Nome = nome; base.Endereco = new Endereco(); base.Endereco = endereco; }
    }
}
