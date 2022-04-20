using COAD.CORPORATIVO.Util;
using GenericCrud.Models.SqlDinamico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.GeradorQuery
{
    public class MontagemQuerySelectDTO : IQueryFromExpression
    {
        public class MontagemQuerySelectItemDTO
        {
            public string AliasTabela { get; set; }
            public string NomeColuna { get; set; }
            public string AliasColuna { get; set; }
            public Type tipoColuna { get; set; }
            public string NomeTipoDado { get; set; }
            public bool IsNullable { get; set; }
        }

        private IList<MontagemQuerySelectItemDTO> LstMontagemQuerySelectItem = new List<MontagemQuerySelectItemDTO>();

        public void AdicionarColuna(string aliasTabela, string nomeColuna, string aliasColuna, string NomeTipoDado, bool? isNullable)
        {
            if (isNullable == null)
                isNullable = false;

            var itemColuna = new MontagemQuerySelectItemDTO()
            {
                AliasColuna = aliasColuna,
                AliasTabela = aliasTabela,
                NomeColuna = nomeColuna,
                NomeTipoDado = NomeTipoDado,
                IsNullable = (bool) isNullable
            };

            itemColuna.tipoColuna = MontagemSQLUtil.MapearTipoDado(NomeTipoDado, itemColuna.IsNullable);
            LstMontagemQuerySelectItem.Add(itemColuna);
        }

        public IList<ColunaSqlDinamicoDTO> ListarColunas()
        {
            return LstMontagemQuerySelectItem
                .Select(x => new ColunaSqlDinamicoDTO(){
                
                Nome = x.NomeColuna,
                TipoDeDados = x.tipoColuna,
                Alias = x.AliasColuna
            })
            .ToList();
        }

        public IList<MetadataColuna> ListarNomesDasColunasMetadata()
        {
            if (LstMontagemQuerySelectItem != null)
            {
                return LstMontagemQuerySelectItem
                .Select(x =>
                        new MetadataColuna(){
                                Name = (!string.IsNullOrEmpty(x.AliasColuna)) ? x.AliasColuna : x.NomeColuna,
                                Tipo = MontagemSQLUtil.MapearTipoDadoInterfaceUsuario(x.NomeTipoDado)                          
                        }).ToList();
            }

            return new List<MetadataColuna>();
        }

        public IList<string> ListarNomesDasColunas()
        {
            if (LstMontagemQuerySelectItem != null)
            {
                return LstMontagemQuerySelectItem
                .Select(x =>
                    (!string.IsNullOrEmpty(x.AliasColuna)) ? x.AliasColuna : x.NomeColuna)
                    .ToList();
            }

            return new List<string>();
        }

        public string GerarExpressao()
        {
            int index = 0;
            int count = LstMontagemQuerySelectItem.Count();

            StringBuilder sb = new StringBuilder();
            foreach (var col in LstMontagemQuerySelectItem)
            {
                var colunas = "\t[{0}].[{1}]";
                sb.Append(string.Format(colunas, col.AliasTabela, col.NomeColuna));

                
                if (!string.IsNullOrEmpty(col.AliasColuna))
                {
                    var alias = " AS [{0}]";
                    sb.Append(string.Format(alias, col.AliasColuna));
                }

                if (index < (count - 1))
                {
                    sb.Append(", \r\n");
                }
                index++;
            }

            return sb.ToString();
        }
    }
}
