
using System.Text;

namespace COAD.COBRANCA.Bancos.Model.DTO
{
    public class LinhaDigitavelDTO
    {
        public LinhaDigitavelCampoDTO Campo1 { get; set; }
        public LinhaDigitavelCampoDTO Campo2 { get; set; }
        public LinhaDigitavelCampoDTO Campo3 { get; set; }
        public LinhaDigitavelCampoDTO Campo4 { get; set; }
        public LinhaDigitavelCampoDTO Campo5 { get; set; }

        public string Valor {
            get {

                StringBuilder sb = new StringBuilder();
                if(Campo1 != null)
                {
                    sb.Append(Campo1.Valor);
                }

                if (Campo2 != null)
                {
                    sb.Append(Campo2.Valor);
                }

                if (Campo3 != null)
                {
                    sb.Append(Campo3.Valor);
                }

                if (Campo4 != null)
                {
                    sb.Append(Campo4.Valor);
                }

                if (Campo4 != null)
                {
                    sb.Append(Campo5.Valor);
                }

                return sb.ToString();
            }
        }


        public string ValorFormatado
        {
            get
            {

                StringBuilder sb = new StringBuilder();
                if (Campo1 != null)
                {
                    sb.Append(Campo1.PrimeiraMetade);
                    sb.Append(".");
                    sb.Append(Campo1.SegundaMetade);
                    sb.Append(Campo1.DV);
                    sb.Append(" ");
                }

                if (Campo2 != null)
                {
                    sb.Append(Campo2.PrimeiraMetade);
                    sb.Append(".");
                    sb.Append(Campo2.SegundaMetade);
                    sb.Append(Campo2.DV);
                    sb.Append(" ");
                }

                if (Campo3 != null)
                {
                    sb.Append(Campo3.PrimeiraMetade);
                    sb.Append(".");
                    sb.Append(Campo3.SegundaMetade);
                    sb.Append(Campo3.DV);
                    sb.Append(" ");
                }

                if (Campo4 != null)
                {
                    sb.Append(" ");
                    sb.Append(Campo4.Valor);
                    sb.Append(" ");
                }

                if (Campo4 != null)
                {
                    sb.Append(Campo5.Valor);
                }

                return sb.ToString();
            }
        }

        public override string ToString()
        {
            return ValorFormatado;
        }
    }
}