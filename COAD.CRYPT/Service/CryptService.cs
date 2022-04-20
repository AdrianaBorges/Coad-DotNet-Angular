using COAD.CRYPT.XmlUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace COAD.CRYPT
{
    public class CryptService
    {
        //Diego Andrade da Silva
        private string path { get; set; }
        const string senha = @"C@adK3y!";
        public CryptService()
        {
            path = @"C:\chaves_criptografia";
        }

        public string Criptografar(string descricao)
        {
            XmlDocument chavePublica = new XmlDocument();
            chavePublica.Load(path + @"\chave_publica.xml");

            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            StringWriter sw = new StringWriter();
            XmlTextWriter tx = new XmlTextWriter(sw);
            chavePublica.WriteTo(tx);

            string xmlValue = sw.ToString();

            RSA.FromXmlString(xmlValue);

            byte[] byteParcial = Encoding.Default.GetBytes(descricao);
            byte[] mensagemEncriptadaByte = RSA.Encrypt(byteParcial, false);
            string encriptedMessage = Encoding.Default.GetString(mensagemEncriptadaByte);

            return encriptedMessage;


        }

        public string Desencriptar(string descricao)
        {
            XmlDocument chavePublica = new XmlDocument();
            chavePublica.Load(path + @"\chave_privada.xml");

            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            StringWriter sw = new StringWriter();
            XmlTextWriter tx = new XmlTextWriter(sw);
            chavePublica.WriteTo(tx);

            string xmlValue = sw.ToString();

            RSA.FromXmlString(xmlValue);

            byte[] byteParcial = Encoding.UTF8.GetBytes(descricao);
            byte[] mensagemEncriptadaByte = RSA.Decrypt(byteParcial, false);
            string descriptedMessage = Encoding.UTF8.GetString(mensagemEncriptadaByte);

            return descriptedMessage;


        }

        public void GerarParDeChaves()
        {
            CspParameters cspParam = new CspParameters();
            cspParam.Flags = CspProviderFlags.UseMachineKeyStore;
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(cspParam);

            string publicKey = RSA.ToXmlString(false);
            string privateKey = RSA.ToXmlString(true);

            XmlDocument xmlPublicKey = new XmlDocument();
            xmlPublicKey.LoadXml(publicKey);

            XmlDocument xmlPrivateKey = new XmlDocument();
            xmlPrivateKey.LoadXml(privateKey);

            XmlUtil.SerializeXml(xmlPublicKey, path + @"\chave_publica.xml");
            XmlUtil.SerializeXml(xmlPrivateKey, path +  @"\chave_privada.xml");

        }

        public string CriptografarTripleDES(string mensagem)
        {
            byte[] Results;

            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();

            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(senha));

            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            TDESAlgorithm.Key = TDESKey;

            TDESAlgorithm.Mode = CipherMode.ECB;

            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            byte[] DataToEncrypt = UTF8.GetBytes(mensagem);

            try
            {

                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();

                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);

            }

            finally
            {

                TDESAlgorithm.Clear();

                HashProvider.Clear();

            }

            return Convert.ToBase64String(Results);
        }

        public string DescriptografarTripleDES(string mensagem)
        {
            byte[] Results;

            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();

            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(senha));

            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            byte[] DataToDecrypt = Convert.FromBase64String(mensagem);

            try

            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }

            finally

            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            return UTF8.GetString(Results);

       }

        public string HashMD5(string input)
        {
            using (System.Security.Cryptography.MD5 md5Hash = System.Security.Cryptography.MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder sBuilder = new StringBuilder();

                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                return sBuilder.ToString();
            }
        }


    }

}
