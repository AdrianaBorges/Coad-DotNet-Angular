using COAD.CORPORATIVO.Service;
using GenericCrud.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service.Custons
{
    public class HistoricoFormatterSRV
    {
        public Dictionary<string, string> binds = new Dictionary<string, string>();

        public string usuario { get; set; }
        public int? CLI_ID { get; set; }
        public int? REP_ID { get; set; }
        public int? IPE_ID { get; set; }
        public int? PPI_ID { get; set; }
        public int? RLI_ID { get; set; }
        public string acao { get; set; }
        public int? acaId { get; set; }
        public int? PST_ID { get; set; }
        public string Observacoes { get; set; }
        public string Message { get; set; }

        public void MapToken(string key, string description)
        {
            binds.Add(key, description);
        }

        public string FormatMessage()
        {

            // Recuperando as informações dos representantes para registrar seus nomes no histórico
            var representanteQueExecutouAAcao = ServiceFactory.RetornarServico<RepresentanteSRV>().FindById(REP_ID);

            string nomeRepresentanteQueExecutouAAcao =
                                (representanteQueExecutouAAcao != null && !string.IsNullOrWhiteSpace(representanteQueExecutouAAcao.REP_NOME))
                                    ? representanteQueExecutouAAcao.REP_NOME : "(Nome indisponível)";

            var descricao = Message.Replace("{{usuario}}", usuario);
            descricao = descricao.Replace("{{CLI_ID}}", CLI_ID + "");
            descricao = descricao.Replace("{{REP_ID}}", REP_ID + "");
            descricao = descricao.Replace("{{IPE_ID}}", IPE_ID + "");
            descricao = descricao.Replace("{{PST_ID}}", PST_ID + "");
            descricao = descricao.Replace("{{PPI_ID}}", PPI_ID + "");
            descricao = descricao.Replace("{{RLI_ID}}", RLI_ID + "");
            descricao = descricao.Replace("{{OBS}}",  Observacoes);
            descricao = descricao.Replace("{{representante}}", nomeRepresentanteQueExecutouAAcao);

            foreach (var key in this.binds.Keys)
            {
                descricao = descricao.Replace("{{" + key + "}}", this.binds[key]);
            }

            return descricao;
        }
    }
}
