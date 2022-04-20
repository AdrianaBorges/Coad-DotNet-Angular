using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Validacoes
{
    public class ValidacaoRegraDetalhesDTO
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("Nome: ");
            sb.Append(Nome);
            sb.Append("<br /> Descrição: <br /> ");
            sb.Append(Descricao);
            sb.Append("");

            return sb.ToString();
        }
    }
}
