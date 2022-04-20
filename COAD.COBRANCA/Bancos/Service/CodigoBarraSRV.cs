
using COAD.COBRANCA.Bancos.Config;
using COAD.COBRANCA.Bancos.Model.DTO;
using COAD.COBRANCA.Bancos.Model.DTO.Interfaces;
using COAD.SEGURANCA.Model;
using System;
using System.IO;

namespace COAD.COBRANCA.Bancos.Service
{
    public class CodigoBarraSRV
    {
        public IBanco Banco { get; set; }

        private void Init()
        {
            ConfigBoleto.Configurar();
        }

        public CodigoBarraSRV(IBanco banco)
        {
            this.Banco = banco;
        }

        public CodigoBarraSRV(string _banco)
        {
            Init();

            Banco = ConfigBoleto.GetBanco(_banco);

            if (Banco == null)
            {
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
                    case "422":
                        this.Banco = new BancoSafra();
                        break;
                    default:
                        this.Banco = new BancoBradesco();
                        break;
                }
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

            if(Banco != null)
            {
                _retorno = Banco.GerarCodigoBarras(DataVencimento, NossoNumero, Valor, Conta);
            }
            else
            {

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
                    case "756":
                        _retorno = new BancoSICOOB().GerarCodigoBarras(DataVencimento, NossoNumero, Valor, Conta);
                        break;
                    case "422":
                        _retorno = new BancoSafra().GerarCodigoBarras(DataVencimento, NossoNumero, Valor, Conta);
                        break;
                    default:
                        _retorno = new BancoBradesco().GerarCodigoBarras(DataVencimento, NossoNumero, Valor, Conta);
                        break;
                }

            }

            return _retorno;
        }

        private CamposLinhaDigitalDTO _seperarCamposVariaveisLinhaDigitavel(CodigoBarrasDTO cb)
        {
            if (cb != null)
            {
                if (Banco != null)
                {
                    // Processamento customizado    
                    var camposLinhaDigital = Banco.SepararCamposVariaveisLinhaDigitavel(cb);
                    if (camposLinhaDigital != null)
                        return camposLinhaDigital;
                }

                var campoLivre = cb.CampoLivre;

                if (!string.IsNullOrWhiteSpace(campoLivre))
                {

                    // Processamento Padrão
                    var campoLivreCampo1 = campoLivre.Substring(0, 5);
                    var campo1 = $@"{cb.IdentificacaoBanco}{cb.CodigoMoeda}{campoLivreCampo1}";
                    var campo2 = campoLivre.Substring(5, 10);
                    var campo3 = campoLivre.Substring(15, 10);

                    var result = new CamposLinhaDigitalDTO()
                    {
                        Campo1 = campo1,
                        Campo2 = campo2,
                        Campo3 = campo3
                    };

                    return result;
                }
            }
        
            return null;
        }


        public LinhaDigitavelDTO GerarLinhaDigitavel(CodigoBarrasDTO cb)
        {            
            if (cb != null)
            {
                LinhaDigitavelDTO linhaDigitavel = new LinhaDigitavelDTO();
                var campos = _seperarCamposVariaveisLinhaDigitavel(cb);

                if(campos != null)
                {

                    CodigoBarraSRV codigoBarraSRV = new CodigoBarraSRV(cb.IdentificacaoBanco);

                    linhaDigitavel.Campo1 = codigoBarraSRV.GerarCampoLinhaDigital(campos.Campo1);
                    linhaDigitavel.Campo2 = codigoBarraSRV.GerarCampoLinhaDigital(campos.Campo2);
                    linhaDigitavel.Campo3 = codigoBarraSRV.GerarCampoLinhaDigital(campos.Campo3);
                    linhaDigitavel.Campo4 = codigoBarraSRV.GerarCampoLinhaDigital(cb.DigitoVerificadorCodigoBarras, false);
                    linhaDigitavel.Campo5 = codigoBarraSRV.GerarCampoLinhaDigital($@"{cb.FatorVencimento}{cb.ValorStr}", false);

                    return linhaDigitavel;
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


            try
            {

                d.Save(curDir);

                return curDir;

            }
            catch ( Exception e)
            {

                throw new Exception(string.Format("Erro ao gerar imagem do codigo de barra."), e);

            }
            

        }


    }
}