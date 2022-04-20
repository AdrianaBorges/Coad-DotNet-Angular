using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Xml.Linq;
using GenericCrud.Util;
using System.Collections;
using COAD.SEGURANCA.Model;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.Security;
using System.Collections.Generic;
using Org.BouncyCastle.X509.Store;
using Coad.GenericCrud.Exceptions;
using COAD.COBRANCA.Bancos.Model.DTO;
using Org.BouncyCastle.Crypto.Operators;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography.X509Certificates;

namespace COAD.COBRANCA.Bancos.Service
{
    public class RegistrarBoletoSRV
    {
        public string ChaveSegura { get; private set; }
        public string UrlRequisicao { get; private set; }
      //  public object DotNetUtilities { get; private set; }

        public const string UrlHomologacao = "https://cobranca.bradesconetempresa.b.br/ibpjregistrotitulows/registrotitulohomologacao";
        public const string UrlProducao = "https://cobranca.bradesconetempresa.b.br/ibpjregistrotitulows/registrotitulo";

        public const string _mediaTypeSign = "application/pkcs7-signature";
        public const string _mediaType = "application/json";
        public const string _charSet = "UTF-8";
        public List<string> Criticas(BradescoCobrancaRequest RemessaCobranca)
        {

            var _erro = new List<string>();

            if (EstaVazio(RemessaCobranca.bairroPagador))
                _erro.Add("Campo bairroPagador é obrigatório");

            if (!EstaVazio(RemessaCobranca.bairroPagador) && RemessaCobranca.bairroPagador.Length > 40)
                _erro.Add("Campo bairroPagador não pode execeder tamanho igual a 40");

            if (EstaVazio(RemessaCobranca.bairroSacadorAvalista) && !EstaVazio(RemessaCobranca.nomeSacadorAvalista))
                _erro.Add("Campo bairroSacadorAvalista é obrigatório quando informado campo nomeSacadorAvalista");

            if (!EstaVazio(RemessaCobranca.bairroSacadorAvalista) && RemessaCobranca.bairroSacadorAvalista.Length > 40)
                _erro.Add("Campo bairroSacadorAvalista não pode execeder tamanho igual a 40");

            if (EstaVazio(RemessaCobranca.cdBanco))
                _erro.Add("Campo cdBanco é obrigatório");

            if (!EstaVazio(RemessaCobranca.cdBanco) && RemessaCobranca.cdBanco.Length != 3)
                _erro.Add("Campo cdBanco precisa ter tamanho igual a 3");

            if (EstaVazio(RemessaCobranca.cdEspecieTitulo))
                _erro.Add("Campo cdBanco é obrigatório");

            if (!EstaVazio(RemessaCobranca.cdEspecieTitulo) && RemessaCobranca.cdEspecieTitulo.Length != 2)
                _erro.Add("Campo cdEspecieTitulo precisa ter tamanho igual a 2");

            if (EstaVazio(RemessaCobranca.cdIndCpfcnpjPagador))
                _erro.Add("Campo cdIndCpfcnpjPagador é obrigatório");

            if (!EstaVazio(RemessaCobranca.cdIndCpfcnpjPagador) && RemessaCobranca.cdIndCpfcnpjPagador.Length != 1)
                _erro.Add("Campo cdIndCpfcnpjPagador precisa ter tamanho igual a 1");

            if (EstaVazio(RemessaCobranca.cdIndCpfcnpjSacadorAvalista) && !EstaVazio(RemessaCobranca.nomeSacadorAvalista))
                _erro.Add("Campo cdIndCpfcnpjSacadorAvalista é obrigatório");

            if (EstaVazio(RemessaCobranca.cdIndCpfcnpjSacadorAvalista) && RemessaCobranca.cdIndCpfcnpjSacadorAvalista.Length != 1)
                _erro.Add("Campo cdIndCpfcnpjSacadorAvalista precisa ter tamanho igual a 1");

            if (!EstaVazio(RemessaCobranca.cdPagamentoParcial) && RemessaCobranca.cdPagamentoParcial.Length != 1)
                _erro.Add("Campo cdPagamentoParcial precisa ter tamanho igual a 1");

            if (!EstaVazio(RemessaCobranca.cdPagamentoParcial) && RemessaCobranca.cdPagamentoParcial.ToUpper() == "S" && EstaVazio(RemessaCobranca.qtdePagamentoParcial))
                _erro.Add("Campo cdPagamentoParcial é obrigatório quando cdPagamentoParcial igual a 'S'");

            if (!EstaVazio(RemessaCobranca.qtdePagamentoParcial) && RemessaCobranca.qtdePagamentoParcial.Length > 3)
                _erro.Add("Campo cdPagamentoParcial precisa ter tamanho máximo de 3");

            if (!EstaVazio(RemessaCobranca.percentualJuros) && RemessaCobranca.percentualJuros.Length > 8)
                _erro.Add("Campo percentualJuros precisa ter tamanho máximo de 8");

            if (!EstaVazio(RemessaCobranca.vlJuros) && RemessaCobranca.vlJuros.Length > 17)
                _erro.Add("Campo vlJuros precisa ter tamanho máximo de 17");

            if (!EstaVazio(RemessaCobranca.vlJuros) && !EstaVazio(RemessaCobranca.percentualJuros))
                _erro.Add("Campo vlJuros não pode ser informado junto com o campo percentualJuros");

            if (!EstaVazio(RemessaCobranca.qtdeDiasJuros) && RemessaCobranca.qtdeDiasJuros.Length > 2)
                _erro.Add("Campo qtdeDiasJuros precisa ter tamanho máximo de 2");

            if (!EstaVazio(RemessaCobranca.percentualMulta) && RemessaCobranca.percentualMulta.Length > 8)
                _erro.Add("Campo percentualMulta precisa ter tamanho máximo de 8");

            if (!EstaVazio(RemessaCobranca.vlMulta) && RemessaCobranca.vlMulta.Length > 17)
                _erro.Add("Campo vlMulta precisa ter tamanho máximo de 17");

            if (!EstaVazio(RemessaCobranca.qtdeDiasMulta) && RemessaCobranca.qtdeDiasMulta.Length > 3)
                _erro.Add("Campo qtdeDiasMulta precisa ter tamanho máximo de 3");

            if (!EstaVazio(RemessaCobranca.percentualDesconto1) && RemessaCobranca.percentualDesconto1.Length > 8)
                _erro.Add("Campo percentualDesconto1 precisa ter tamanho máximo de 8");

            if (!EstaVazio(RemessaCobranca.vlDesconto1) && RemessaCobranca.vlDesconto1.Length > 17)
                _erro.Add("Campo vlDesconto1 precisa ter tamanho máximo de 17");

            if ((!EstaVazio(RemessaCobranca.vlDesconto1) || !EstaVazio(RemessaCobranca.percentualDesconto1)) && EstaVazio(RemessaCobranca.dataLimiteDesconto1))
                _erro.Add("Campo dataLimiteDesconto1 obrigatório quando valor ou percentual é informado");

            if (!EstaVazio(RemessaCobranca.dataLimiteDesconto1) && RemessaCobranca.dataLimiteDesconto1.Length > 10)
                _erro.Add("Campo dataLimiteDesconto1 precisa ter tamanho máximo de 10");

            if (!EstaVazio(RemessaCobranca.percentualDesconto2) && RemessaCobranca.percentualDesconto2.Length > 8)
                _erro.Add("Campo percentualDesconto2 precisa ter tamanho máximo de 8");

            if (!EstaVazio(RemessaCobranca.vlDesconto2) && RemessaCobranca.vlDesconto2.Length > 17)
                _erro.Add("Campo vlDesconto2 precisa ter tamanho máximo de 17");

            if ((!EstaVazio(RemessaCobranca.vlDesconto2) || !EstaVazio(RemessaCobranca.percentualDesconto2)) && EstaVazio(RemessaCobranca.dataLimiteDesconto2))
                _erro.Add("Campo dataLimiteDesconto2 obrigatório quando valor ou percentual é informado");

            if (!EstaVazio(RemessaCobranca.dataLimiteDesconto2) && RemessaCobranca.dataLimiteDesconto2.Length > 10)
                _erro.Add("Campo dataLimiteDesconto2 precisa ter tamanho máximo de 10");

            if (!EstaVazio(RemessaCobranca.percentualDesconto3) && RemessaCobranca.percentualDesconto3.Length > 8)
                _erro.Add("Campo percentualDesconto3 precisa ter tamanho máximo de 8");

            if (!EstaVazio(RemessaCobranca.vlDesconto3) && RemessaCobranca.vlDesconto3.Length > 17)
                _erro.Add("Campo vlDesconto3 precisa ter tamanho máximo de 17");

            if ((!EstaVazio(RemessaCobranca.vlDesconto3) || !EstaVazio(RemessaCobranca.percentualDesconto3)) && EstaVazio(RemessaCobranca.dataLimiteDesconto3))
                _erro.Add("Campo dataLimiteDesconto3 obrigatório quando valor ou percentual é informado");

            if (!EstaVazio(RemessaCobranca.dataLimiteDesconto2) && RemessaCobranca.dataLimiteDesconto2.Length > 10)
                _erro.Add("Campo dataLimiteDesconto3 precisa ter tamanho máximo de 10");

            if (!EstaVazio(RemessaCobranca.prazoBonificacao) && RemessaCobranca.prazoBonificacao.Length > 2)
                _erro.Add("Campo prazoBonificacao precisa ter tamanho máximo de 2");

            if (!EstaVazio(RemessaCobranca.percentualBonificacao) && RemessaCobranca.percentualBonificacao.Length > 8)
                _erro.Add("Campo percentualBonificacao precisa ter tamanho máximo de 8");

            if (!EstaVazio(RemessaCobranca.vlBonificacao) && RemessaCobranca.vlBonificacao.Length > 17)
                _erro.Add("Campo vlBonificacao precisa ter tamanho máximo de 17");

            if ((!EstaVazio(RemessaCobranca.percentualBonificacao) || !EstaVazio(RemessaCobranca.vlBonificacao)) && EstaVazio(RemessaCobranca.prazoBonificacao))
                _erro.Add("Campo prazoBonificacao é obrigatório quando vlBonificacao ou percentualBonificacao é informado");

            if (!EstaVazio(RemessaCobranca.dtLimiteBonificacao) && RemessaCobranca.dtLimiteBonificacao.Length > 10)
                _erro.Add("Campo dtLimiteBonificacao precisa ter tamanho máximo de 10");

            if ((!EstaVazio(RemessaCobranca.percentualBonificacao) || !EstaVazio(RemessaCobranca.vlBonificacao)) && EstaVazio(RemessaCobranca.dtLimiteBonificacao))
                _erro.Add("Campo percentualBonificacao é obrigatório quando vlBonificacao ou percentualBonificacao é informado");

            if (!EstaVazio(RemessaCobranca.vlAbatimento) && RemessaCobranca.vlAbatimento.Length > 17)
                _erro.Add("Campo vlAbatimento precisa ter tamanho máximo de 17");

            if (EstaVazio(RemessaCobranca.nomePagador) || RemessaCobranca.nomePagador.Length > 70)
                _erro.Add("Campo nomePagador precisa ter tamanho máximo de 70");

            if (EstaVazio(RemessaCobranca.logradouroPagador) || RemessaCobranca.logradouroPagador.Length > 40)
                _erro.Add("Campo logradouroPagador precisa ter tamanho máximo de 40");

            if (EstaVazioSemZero(RemessaCobranca.nuLogradouroPagador) && RemessaCobranca.nuLogradouroPagador.Length > 10)
                _erro.Add("Campo nuLogradouroPagador precisa ter tamanho máximo de 10");

            if (!EstaVazio(RemessaCobranca.complementoLogradouroPagador) && RemessaCobranca.nuLogradouroPagador.Length > 15)
                _erro.Add("Campo complementoLogradouroPagador precisa ter tamanho máximo de 15");

            if (EstaVazio(RemessaCobranca.cepPagador) || RemessaCobranca.cepPagador.Length > 5)
                _erro.Add("Campo cepPagador precisa ter tamanho máximo de 5");

            if (RemessaCobranca.complementoCepPagador != "000" && (EstaVazio(RemessaCobranca.complementoCepPagador) || RemessaCobranca.complementoCepPagador.Length != 3))
                _erro.Add("Campo complementoCepPagador precisa ter tamanho de 3");

            if (EstaVazio(RemessaCobranca.bairroPagador) || RemessaCobranca.bairroPagador.Length > 40)
                _erro.Add("Campo bairroPagador precisa ter tamanho máximo de 40");

            if (EstaVazio(RemessaCobranca.municipioPagador) || RemessaCobranca.municipioPagador.Length > 30)
                _erro.Add("Campo municipioPagador precisa ter tamanho máximo de 30");

            if (EstaVazio(RemessaCobranca.ufPagador) || RemessaCobranca.ufPagador.Length > 2)
                _erro.Add("Campo ufPagador precisa ter tamanho máximo de 2");

            if (EstaVazio(RemessaCobranca.cdIndCpfcnpjPagador) || RemessaCobranca.cdIndCpfcnpjPagador.Length > 1)
                _erro.Add("Campo cdIndCpfcnpjPagador precisa ter tamanho máximo de 1");

            if (EstaVazio(RemessaCobranca.nuCpfcnpjPagador) || RemessaCobranca.nuCpfcnpjPagador.Length > 14)
                _erro.Add("Campo nuCpfcnpjPagador precisa ter tamanho máximo de 14");

            if (!EstaVazio(RemessaCobranca.endEletronicoPagador) && RemessaCobranca.endEletronicoPagador.Length > 70)
                _erro.Add("Campo endEletronicoPagador precisa ter tamanho máximo de 14");

            if (!EstaVazio(RemessaCobranca.nomeSacadorAvalista) && RemessaCobranca.nomeSacadorAvalista.Length > 40)
                _erro.Add("Campo nomeSacadorAvalista precisa ter tamanho máximo de 40");

            if (!EstaVazio(RemessaCobranca.logradouroSacadorAvalista) && RemessaCobranca.logradouroSacadorAvalista.Length > 40)
                _erro.Add("Campo logradouroSacadorAvalista precisa ter tamanho máximo de 40");

            if (!EstaVazio(RemessaCobranca.nomeSacadorAvalista) && EstaVazio(RemessaCobranca.logradouroSacadorAvalista))
                _erro.Add("Campo logradouroSacadorAvalista é obrigatório quando nomeSacadorAvalista é informado");

            if (!EstaVazio(RemessaCobranca.nuLogradouroSacadorAvalista) && RemessaCobranca.nuLogradouroSacadorAvalista.Length > 10)
                _erro.Add("Campo nuLogradouroSacadorAvalista precisa ter tamanho máximo de 10");

            if (!EstaVazio(RemessaCobranca.nomeSacadorAvalista) && EstaVazio(RemessaCobranca.nuLogradouroSacadorAvalista))
                _erro.Add("Campo nuLogradouroSacadorAvalista é obrigatório quando nomeSacadorAvalista é informado");

            if (!EstaVazio(RemessaCobranca.complementoLogradouroSacadorAvalista) && RemessaCobranca.complementoLogradouroSacadorAvalista.Length > 15)
                _erro.Add("Campo complementoLogradouroSacadorAvalista precisa ter tamanho máximo de 15");

            if (!EstaVazio(RemessaCobranca.cepSacadorAvalista) && RemessaCobranca.cepSacadorAvalista.Length > 5)
                _erro.Add("Campo cepSacadorAvalista precisa ter tamanho máximo de 5");

            if (!EstaVazio(RemessaCobranca.nomeSacadorAvalista) && EstaVazio(RemessaCobranca.cepSacadorAvalista))
                _erro.Add("Campo cepSacadorAvalista é obrigatório quando nomeSacadorAvalista é informado");

            if (!EstaVazio(RemessaCobranca.complementoCepSacadorAvalista) && RemessaCobranca.complementoCepSacadorAvalista.Length > 3)
                _erro.Add("Campo complementoCepSacadorAvalista precisa ter tamanho máximo de 3");

            if (!EstaVazio(RemessaCobranca.nomeSacadorAvalista) && EstaVazio(RemessaCobranca.complementoCepSacadorAvalista))
                _erro.Add("Campo complementoCepSacadorAvalista é obrigatório quando nomeSacadorAvalista é informado");

            if (!EstaVazio(RemessaCobranca.bairroSacadorAvalista) && RemessaCobranca.bairroSacadorAvalista.Length > 40)
                _erro.Add("Campo bairroSacadorAvalista precisa ter tamanho máximo de 40");

            if (!EstaVazio(RemessaCobranca.nomeSacadorAvalista) && EstaVazio(RemessaCobranca.bairroSacadorAvalista))
                _erro.Add("Campo bairroSacadorAvalista é obrigatório quando nomeSacadorAvalista é informado");

            if (!EstaVazio(RemessaCobranca.municipioSacadorAvalista) && RemessaCobranca.municipioSacadorAvalista.Length > 40)
                _erro.Add("Campo municipioSacadorAvalista precisa ter tamanho máximo de 40");

            if (!EstaVazio(RemessaCobranca.nomeSacadorAvalista) && EstaVazio(RemessaCobranca.municipioSacadorAvalista))
                _erro.Add("Campo municipioSacadorAvalista é obrigatório quando nomeSacadorAvalista é informado");

            if (!EstaVazio(RemessaCobranca.ufSacadorAvalista) && RemessaCobranca.ufSacadorAvalista.Length > 2)
                _erro.Add("Campo ufSacadorAvalista precisa ter tamanho máximo de 2");

            if (!EstaVazio(RemessaCobranca.nomeSacadorAvalista) && EstaVazio(RemessaCobranca.ufSacadorAvalista))
                _erro.Add("Campo ufSacadorAvalista é obrigatório quando nomeSacadorAvalista é informado");

            if (!EstaVazio(RemessaCobranca.cdIndCpfcnpjSacadorAvalista) && RemessaCobranca.cdIndCpfcnpjSacadorAvalista.Length > 1)
                _erro.Add("Campo cdIndCpfcnpjSacadorAvalista precisa ter tamanho máximo de 1");

            if (!EstaVazio(RemessaCobranca.nomeSacadorAvalista) && EstaVazio(RemessaCobranca.cdIndCpfcnpjSacadorAvalista))
                _erro.Add("Campo cdIndCpfcnpjSacadorAvalista é obrigatório quando nomeSacadorAvalista é informado");

            if (!EstaVazio(RemessaCobranca.nuCpfcnpjSacadorAvalista) && RemessaCobranca.nuCpfcnpjSacadorAvalista.Length > 14)
                _erro.Add("Campo nuCpfcnpjSacadorAvalista precisa ter tamanho máximo de 14");

            if (!EstaVazio(RemessaCobranca.nomeSacadorAvalista) && EstaVazio(RemessaCobranca.nuCpfcnpjSacadorAvalista))
                _erro.Add("Campo nuCpfcnpjSacadorAvalista é obrigatório quando nomeSacadorAvalista é informado");

            if (!EstaVazio(RemessaCobranca.endEletronicoSacadorAvalista) && RemessaCobranca.endEletronicoSacadorAvalista.Length > 70)
                _erro.Add("Campo endEletronicoSacadorAvalista precisa ter tamanho máximo de 70");

            return _erro;

        }

        private bool EstaVazio(string param)
        {
            if (string.IsNullOrWhiteSpace(param))
                return true;
            long resultado;
            var converteu = long.TryParse(param, out resultado);
            return converteu && resultado == 0;
        }
        private bool EstaVazioSemZero(string param)
        {
            if (string.IsNullOrWhiteSpace(param))
                return true;
            long resultado;
            var converteu = long.TryParse(param, out resultado);
            return converteu && resultado > 0;
        }
        private byte[] AssinarCriptografar(BradescoCobrancaRequest model)
        {
            var _erro = this.Criticas(model);

            if (_erro.Count() > 0)
                throw new ValidacaoException("Erro de validação!!", "Registro_Boleto", _erro.ToArray());
            

            var data = ConverterParaJsonAspasSimples(model);
            var encoding = new UTF8Encoding();
            var messageBytes = encoding.GetBytes(data);

            // certificado precisa ser instalado na máquina local e na pasta pessoal, diferente disso alterar linha abaixo
            
            var certPath = "";
            var password = "279229";

            string curDir = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString());
            certPath = curDir + "\\certificados\\17184148_ATUALIZACAO_PROFISSIONAL_CONTINUADA_LTDA27922913000111.p12";

            var privateCert = new X509Certificate2(certPath, password, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet);


            if (privateCert == null)
                throw new Exception("Certificado não localizado.");
            if (privateCert.PrivateKey == null)
                throw new Exception("chave privada não localizada no certificado.");

            //convertendo certificado para objeto que o bouncycastle conhece
            var bouncyCastleKey = DotNetUtilities.GetKeyPair(privateCert.PrivateKey).Private;
            var x5091 = new X509Certificate(privateCert.RawData);
            var x509CertBouncyCastle = DotNetUtilities.FromX509Certificate(x5091);

            var generator = new CmsSignedDataGenerator();
            var signerInfoGeneratorBuilder = new SignerInfoGeneratorBuilder();
            var assinatura = signerInfoGeneratorBuilder.Build(new Asn1SignatureFactory("SHA256WithRSA", bouncyCastleKey), x509CertBouncyCastle);
            generator.AddSignerInfoGenerator(assinatura);

            //criando certstore que o bouncycastle conhece
            IList certList = new ArrayList();
            certList.Add(x509CertBouncyCastle);
            var store509BouncyCastle = X509StoreFactory.Create("Certificate/Collection", new X509CollectionStoreParameters(certList));
            generator.AddCertificates(store509BouncyCastle);

            var cmsdata = new CmsProcessableByteArray(messageBytes);
            var signeddata = generator.Generate(cmsdata, true);
            var mensagemFinal = signeddata.GetEncoded();
            //converte para base64 que eh o formato que o serviço espera
            var mensagemConvertidaparaBase64 = Convert.ToBase64String(mensagemFinal);

            //chama serviço convertendo a string na base64 em bytes
            return encoding.GetBytes(mensagemConvertidaparaBase64);

        }
        
        public object Request(bool homologacao, object JSONRequest, string JSONmethod, string JSONContentType, Type JSONResponseType)
        {

            var requestUrl = "";


//            if (SysUtils.InHomologation())
//                requestUrl = "https://cobranca.bradesconetempresa.b.br/ibpjregistrotitulows/registrotitulohomologacao";
//            else
                requestUrl = "https://cobranca.bradesconetempresa.b.br/ibpjregistrotitulows/registrotitulo";

            //TLS 1.2
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

            var request = (HttpWebRequest)WebRequest.Create(requestUrl);

            var sig = this.AssinarCriptografar((BradescoCobrancaRequest)JSONRequest);

            request.Method = JSONmethod;// "POST";
            request.ContentType = _mediaType; // _mediaTypeSign; 
            request.ContentLength = sig.Length;

            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(sig, 0, sig.Length);
            }

            var response = request.GetResponse();

            var stream = response.GetResponseStream();
            
            if (stream == null)
                throw new ApplicationException("erro ao obter resposta");

            StreamReader sr = new StreamReader(stream);

            string strsb = sr.ReadToEnd();

            var objResponse = JsonConvert.DeserializeObject(XDocument.Parse(strsb).Root.Value, JSONResponseType);
            //var objResponse = JsonConvert.DeserializeObject(strsb, JSONResponseType);

            return objResponse;

        }
        public string ConverterParaJsonAspasSimples(BradescoCobrancaRequest data)
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            using (var writer = new JsonTextWriter(sw))
            {
                writer.QuoteChar = '\"';

                var ser = new JsonSerializer();
                ser.Serialize(writer, data);
            }
            return sb.ToString();
        }
        public int? CalcularDigitoVerificador(string codigo43Digitos)
        {
            //if (string.IsNullOrWhiteSpace(codigo43Digitos))
            //{
            //    throw new ArgumentException("O código de 43 dígitos não pode ser nullo ou vazio.");
            //}

            //if (codigo43Digitos.Length != 43)
            //{
            //    var msg = "O código passado deve conter 43 dígitos.  Tamanho do código passado ({0}).";
            //    var tamanho = codigo43Digitos.Length;
            //    msg = string.Format(msg, tamanho);

            //    throw new ArgumentException(msg);
            //}

            char[] arrayChar = codigo43Digitos.ToCharArray();
            int length = arrayChar.Length;

            int multiplicador = 2;
            int somatorio = 0;

            for (var index = (length - 1); index >= 0; index--)
            {
                int valorInteiro = int.Parse(arrayChar[index].ToString());
                int valorMultiplicado = valorInteiro * multiplicador;

                somatorio += valorMultiplicado;

                multiplicador++;
                if (multiplicador > 9)
                {
                    multiplicador = 2;
                }
            }

            var modulo = somatorio % 11;

            if (modulo == 0 || modulo == 1)
            {
                return 0;
            }

            var digitoVerificador = 11 - modulo;

            if (digitoVerificador > 9)
            {
                throw new Exception("Ocorreu um erro ao gerar o dígito verificador, o resultado obtido possui mais de 1 dígito");
            }

            return digitoVerificador;

        }
        public string ConverteObjectParaJSon<T>(T obj)
        {
            try
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                MemoryStream ms = new MemoryStream();
                ser.WriteObject(ms, obj);
                string jsonString = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return jsonString;
            }
            catch
            {
                throw;
            }
        }
        public T ConverteJSonParaObject<T>(string jsonString)
        {
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                T obj = (T)serializer.ReadObject(ms);
                return obj;
            }
            catch
            {
                throw;
            }
        }
    


    }
}