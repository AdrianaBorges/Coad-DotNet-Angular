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
        private string path { get; set; }

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

            byte[] byteParcial = Encoding.Default.GetBytes(descricao);
            byte[] mensagemEncriptadaByte = RSA.Decrypt(byteParcial, false);
            string descriptedMessage = Encoding.Default.GetString(mensagemEncriptadaByte);

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

        
    }

}
