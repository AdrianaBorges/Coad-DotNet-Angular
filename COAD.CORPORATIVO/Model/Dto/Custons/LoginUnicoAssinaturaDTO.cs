using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class LoginUnicoAssinaturaDTO
    {
        public string CodAssinatura { get; set; }
        public string SenhaAssinatura { get; set; }
        public int? ProId { get; set; }
        public string ProNome { get; set; }
        private DateTime? _dataVigencia;
        public bool AssinaturaNativaCliente { get; set; }

        /// <summary>
        /// Esse campo foi criado para o cliente visualizar apenas a vigência atual. 
        /// E ocultar datas de contratos futuros;
        /// </summary>
        public DateTime? DataVigencia { 
            get
            {
                if (_dataVigencia != null)
                {
                    var dataAtual = DateTime.Now;

                    //if (dataAtual <= _dataVigencia) // só retorna a data do contrato se ele estiver vigente
                        return _dataVigencia;
                }

                return null;
            } 
             set {
                 _dataVigencia = value;
             } 
        }

        public int SituacaoAssinatura {

            get {

                if (_dataVigencia != null)
                {
                    int situacao = 0;
                    var dataAtual = DateTime.Now;

                    if (dataAtual <= _dataVigencia)
                        situacao = 1;
                    else
                    {
                        situacao = 2;
                    }

                    return situacao;
                }
                return 0;
            }
            private set { }
        }
    }
}
