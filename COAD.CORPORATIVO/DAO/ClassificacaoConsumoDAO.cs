using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{
    public class ClassificacaoConsumoDAO : RepositorioCorp<CLASSICACAO_CONSUMO>
    {
        public List<CLASSICACAO_CONSUMO> BuscarPorTipo(int id)
        {
            var classificacao = (from x in db.CLASSICACAO_CONSUMO where x.CLC_TIPO == id select x).ToList();
            return classificacao;
        }
    }
}
