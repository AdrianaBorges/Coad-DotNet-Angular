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
    /// ALT: 14/10/2016 - Parâmetros do boleto.
    /// </summary>
    public class Parametro2DTO
    {
        // declarando os atributos necessários
        private string dvA { get; set; }
        private string dvC { get; set; }

        public string cnpjCedente { get; set; }
        public string bco { get; set; }
        public string age { get { return dvA; } 
                            set { 
                                    // retornando apenas os 4 primeiros dígitos
                                    dvA = value.Substring(0, 4);
                                    if (value.Length > 4)
                                        ageDv = value.Substring(4, value.Length - 4);
                                }
                          }
        public string ageDv { get; set; }
        public string cta { get; set; }
        public string ctaDv { get; set; }
        public string carteira { get; set; }
        public string operCta { get; set; }
        public string esp { get; set; }
        public string msg { get; set; }
        public int idCliente { get; set; }
        public string vencimento { get; set; }
        private string vlrBoleto;
        public string valorBoleto { get {return vlrBoleto;}
                                    set {
                                            // acertando o valor
                                            vlrBoleto = value;
                                            if (vlrBoleto.IndexOf('.') == -1)
                                            {
                                                if (vlrBoleto.IndexOf(',') > -1)
                                                    vlrBoleto = vlrBoleto.Replace(',', '.');
                                            }
                                        } 
                                  }
        public string numeroDocumento { get; set; }
    }
}
