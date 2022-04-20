using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
using COAD.FISCAL.Model.Danfe.Enum;


namespace COAD.FISCAL.Model.Danfe.Anotacao
{

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class CustomFieldPDF : Attribute
    {
        public CustomFieldPDF()
        {
        }
        public CustomFieldPDF(string Texto
                            , double Altura
                            , double Largura
                            , double MargemEsquerda
                            , double MargemSuperior
                            , double Tamanho = 0
                            , bool Box = true
                            , int Tipo = 0)
        {
             this.Texto = Texto;
             this.Altura = Altura;
             this.Largura = Largura;
             this.MargemEsquerda = MargemEsquerda;
             this.MargemSuperior = MargemSuperior;
             this.Tamanho = Tamanho;
             this.Box = Box;
             this.Tipo = Tipo;
        }
        public string Texto { get; set; }
        public double Altura {get; set;}
        public double Largura { get; set; }
        public double MargemEsquerda { get; set; }
        public double MargemSuperior { get; set; }
        public double Tamanho { get; set; }
        public bool Box { get; set; }
        public int Tipo { get; set; }
        public string Nome { get; set; }

    }


    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class CustomFieldAlinPDF : Attribute
    {
        public CustomFieldAlinPDF()
        {
        }
        public CustomFieldAlinPDF( int Tamanho
                                 , string Alinhamento = "D"
                                 , char Conteudo =' ')
        {
            this.Tamanho = Tamanho;
            this.Conteudo = Conteudo;
            this.Alinhamento = Alinhamento;
        }
        public int Tamanho { get; set; }
        public char Conteudo { get; set; }
        public string Alinhamento { get; set; }
        public string Nome { get; set; }


    }
}
