
using COAD.COBRANCA.Bradesco.Config;
using COAD.COBRANCA.Bradesco.Model.DTO;
using COAD.COBRANCA.Bradesco.Model.DTO.Interfaces;
using COAD.SEGURANCA.Model;
using System;
using System.IO;

using COAD.CORPORATIVO.Service;

namespace COAD.COBRANCA.Bradesco.Service
{
    public class CodigoBarraSRV
    {
        public IBanco Banco { get; set; }

        private void Init()
        {
            ConfigBoleto.Configurar();
        }

        public CodigoBarraSRV(string _banco)
        {
            Init();

            switch (_banco)
            {
                case "033":
                    this.Banco = new BancoSantander();
                    break;
                case "041":
                    this.Banco = new BancoBanrisul();
                    break;
                case "104":
                    this.Banco = new BancoCaixa();
                    break;
                case "237":
                    this.Banco = new BancoBradesco();
                    break;
                case "341":
                    this.Banco = new BancoItau();
                    break;
                default:
                    this.Banco = new BancoBradesco();
                    break;

            }
        }

        public LinhaDigitavelCampoDTO GerarCampoLinhaDigital(string campo, bool calcularDigito = true)
        {
            if (!string.IsNullOrWhiteSpace(campo) && Banco != null)
            {
                if (calcularDigito)
                {
                    var dvCampo = Banco.CalcularDVLinhaDigitavel(campo);
                    var primeiraMetade = (campo.Length >= 5) ? campo.Substring(0, 5) : null;
                    var segundaMetade = (campo.Length >= 5) ? campo.Substring(5, campo.Length - 5) : null;
                    var ldCampo = new LinhaDigitavelCampoDTO()
                    {
                        Valor = campo + dvCampo,
                        DV = dvCampo,
                        PrimeiraMetade = primeiraMetade,
                        SegundaMetade = segundaMetade,
                    };
                    return ldCampo;
                }
                else
                {
                    return new LinhaDigitavelCampoDTO()
                    {
                        Valor = campo
                    };
                }
            }

             return null;
        }
       
        public CodigoBarrasDTO GerarCodigoBarras(DateTime? DataVencimento, String NossoNumero, decimal? Valor, ContaDTO Conta)
        {

            var _retorno = new CodigoBarrasDTO();

            switch (Conta.BAN_ID)
            {
                case "033":
                    _retorno = new BancoSantander().GerarCodigoBarras(DataVencimento, NossoNumero, Valor, Conta);
                    break;
                case "041":
                    _retorno = new BancoBanrisul().GerarCodigoBarras(DataVencimento, NossoNumero, Valor, Conta);
                    break;
                case "104":
                    _retorno = new BancoCaixa().GerarCodigoBarras(DataVencimento, NossoNumero, Valor, Conta);
                    break;
                case "237":
                    _retorno = new BancoBradesco().GerarCodigoBarras(DataVencimento, NossoNumero, Valor, Conta);
                    break;
                case "341":
                    _retorno = new BancoItau().GerarCodigoBarras(DataVencimento, NossoNumero, Valor, Conta);
                    break;
                default:
                    _retorno = new BancoBradesco().GerarCodigoBarras(DataVencimento, NossoNumero, Valor, Conta);
                    break;

            }

            return _retorno;
        }

        public LinhaDigitavelDTO GerarLinhaDigitavel(CodigoBarrasDTO cb)
        {
            
            if (cb != null)
            {
                LinhaDigitavelDTO linhaDigitavel = new LinhaDigitavelDTO();
                var campoLivre = cb.CampoLivre;

                if (cb.IdentificacaoBanco == "033")
                {
                    
                    var campo1 = $@"{cb.IdentificacaoBanco}{cb.CodigoMoeda}9{cb.CodBeneficiario.Substring(0,4)}";
                    var campo2 = $@"{cb.CodBeneficiario.Substring(4, 3)}{cb.NossoNumero.Substring(0, 7)}";
                    var campo3 = $@"{cb.NossoNumero.Substring(7, 6)}0104";


                    CodigoBarraSRV codigoBarraSRV = new CodigoBarraSRV(cb.IdentificacaoBanco);
                    //codigoBarraSRV.Banco = Banco;

                    linhaDigitavel.Campo1 = codigoBarraSRV.GerarCampoLinhaDigital(campo1);
                    linhaDigitavel.Campo2 = codigoBarraSRV.GerarCampoLinhaDigital(campo2);
                    linhaDigitavel.Campo3 = codigoBarraSRV.GerarCampoLinhaDigital(campo3);
                    linhaDigitavel.Campo4 = codigoBarraSRV.GerarCampoLinhaDigital(cb.DigitoVerificadorCodigoBarras, false);
                    linhaDigitavel.Campo5 = codigoBarraSRV.GerarCampoLinhaDigital($@"{cb.FatorVencimento}{cb.ValorStr}", false);

                    return linhaDigitavel;
                }
                else if (cb.IdentificacaoBanco == "041") //Banrisul
                {

                    var campo1 = $@"{cb.IdentificacaoBanco}921{cb.CampoLivre.Substring(2, 3)}";
                    var campo2 = $@"{cb.CampoLivre.Substring(5, 8)}{cb.NossoNumero.Substring(0, 2)}";
                    var campo3 = $@"{cb.NossoNumero.Substring(2, 6)}40{cb.CampoLivre.Substring(23, 2)}";

                    CodigoBarraSRV codigoBarraSRV = new CodigoBarraSRV(cb.IdentificacaoBanco);

                    linhaDigitavel.Campo1 = codigoBarraSRV.GerarCampoLinhaDigital(campo1);
                    linhaDigitavel.Campo2 = codigoBarraSRV.GerarCampoLinhaDigital(campo2);
                    linhaDigitavel.Campo3 = codigoBarraSRV.GerarCampoLinhaDigital(campo3);
                    linhaDigitavel.Campo4 = codigoBarraSRV.GerarCampoLinhaDigital(cb.DigitoVerificadorCodigoBarras, false);
                    linhaDigitavel.Campo5 = codigoBarraSRV.GerarCampoLinhaDigital($@"{cb.FatorVencimento}{cb.ValorStr}", false);

                    return linhaDigitavel;

                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(campoLivre))
                    {
                        var campoLivreCampo1 = campoLivre.Substring(0, 5);
                        var campo1 = $@"{cb.IdentificacaoBanco}{cb.CodigoMoeda}{campoLivreCampo1}";
                        var campo2 = campoLivre.Substring(5, 10);
                        var campo3 = campoLivre.Substring(15, 10);


                        CodigoBarraSRV codigoBarraSRV = new CodigoBarraSRV(cb.IdentificacaoBanco);
                        //codigoBarraSRV.Banco = Banco;

                        linhaDigitavel.Campo1 = codigoBarraSRV.GerarCampoLinhaDigital(campo1);
                        linhaDigitavel.Campo2 = codigoBarraSRV.GerarCampoLinhaDigital(campo2);
                        linhaDigitavel.Campo3 = codigoBarraSRV.GerarCampoLinhaDigital(campo3);
                        linhaDigitavel.Campo4 = codigoBarraSRV.GerarCampoLinhaDigital(cb.DigitoVerificadorCodigoBarras, false);
                        linhaDigitavel.Campo5 = codigoBarraSRV.GerarCampoLinhaDigital($@"{cb.FatorVencimento}{cb.ValorStr}", false);

                        return linhaDigitavel;
                    }
                }
            }
            return null;
        }

        public string GerarImagemBarcode(string data)
        {
            var b = new BoletoNet.Boleto();

            b.CodigoBarra.Codigo = data;

            var d = new BoletoNet.BoletoBancario().GeraImagemCodigoBarras(b);

            string curDir = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString());

            var arq = DateTime.Now.Millisecond.ToString() +
                      DateTime.Now.Second.ToString() +
                      DateTime.Now.Minute.ToString();

            curDir = curDir + "\\temp\\barcode" + arq + ".png";

            var imgPath = "../temp/barcode" + arq + ".png";

            d.Save(curDir);
            
            return curDir;

        }


    }
}