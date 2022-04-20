using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model
{
   
    public class TipoEvento 
    {
        public int Codigo { get; set; }
        public string Desc { get; set; }

        public static readonly TipoEvento ConfirmacaoDaOperacao = new TipoEvento(210200, "Confirmacao da Operacao");
        public static readonly TipoEvento CienciaDaOperacao = new TipoEvento(210210, "Ciencia da Operacao");
        public static readonly TipoEvento DesconhecimentoDaOperacao = new TipoEvento(210220, "Desconhecimento da Operacao");
        public static readonly TipoEvento OperacaoNaoRealizada = new TipoEvento(210240, "Operacao nao Realizada");
        public static readonly TipoEvento Cancelamento = new TipoEvento(110111, "Cancelamento");

        public TipoEvento()
        {

        }

        public TipoEvento(int Codigo, string Desc)
        {
            this.Codigo = Codigo;
            this.Desc = Desc;
        }


    };

    public class DetEvento 
    {
        public string DescEvento { get; set; }
    }
    
}
