using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Mvc;

namespace COAD.UTIL.Ferramentas
{
    /// <summary>
    /// ALT: 07/11/2017 -
    /// Classe contendo métodos para interagir com qualquer FTP
    /// </summary>
    public class Ftp
    {
        public string NomeArquivo { get; set; }
        public string ConteudoArquivo { get; set; }
        public string IPServidorFTP { get; set; }
        public string PastaFTP { get; set; }
        public string UsuarioFTP { get; set; }
        public string SenhaFTP { get; set; }

        /// <summary>
        /// ALT: 07/11/2017 - 
        /// Apenas conecta ao FTP da COAD/CORREIOS Entrega Direta: {ftp.correios.com.br}
        /// </summary>
        public Ftp()
        {
            this.IPServidorFTP = "ftp.correios.com.br";
            this.PastaFTP = "ENTREGADIRETA/68/COAD";
            this.UsuarioFTP = "coad";
            this.SenhaFTP = "c04dxky=ftp";
        }

        /// <summary>
        /// ALT: 07/11/2017 -
        /// Apenas conecta ao FTP de determinado servidor
        /// </summary>
        /// <param name="IPServidorFTP"></param>
        /// <param name="PastaFTP"></param>
        /// <param name="UsuarioFTP"></param>
        /// <param name="SenhaFTP"></param>
        public Ftp(string IPServidorFTP, string PastaFTP, string UsuarioFTP, string SenhaFTP)
        {
            this.IPServidorFTP = IPServidorFTP;
            this.PastaFTP = PastaFTP;
            this.UsuarioFTP = UsuarioFTP;
            this.SenhaFTP = SenhaFTP;
        }

        /// <summary>
        /// ALT: 07/11/2017 -
        /// Conecta e posta arquivos em determinado servidor
        /// </summary>
        /// <param name="NomeArquivo"></param>
        /// <param name="ConteudoArquivo"></param>
        /// <param name="IPServidorFTP"></param>
        /// <param name="PastaFTP"></param>
        /// <param name="UsuarioFTP"></param>
        /// <param name="SenhaFTP"></param>
        public Ftp(string NomeArquivo, string ConteudoArquivo, string IPServidorFTP, string PastaFTP, string UsuarioFTP, string SenhaFTP)
        {
            this.NomeArquivo = NomeArquivo;
            this.ConteudoArquivo = ConteudoArquivo;
            this.IPServidorFTP = IPServidorFTP;
            this.PastaFTP = PastaFTP;
            this.UsuarioFTP = UsuarioFTP;
            this.SenhaFTP = SenhaFTP;

            this.Postar();
        }

        /// <summary>
        /// ALT: 07/11/2017 -
        /// Apenas posta arquivos em servidor já conectado
        /// </summary>
        /// <param name="NomeArquivo"></param>
        /// <param name="ConteudoArquivo"></param>
        public void Postar(string NomeArquivo, string ConteudoArquivo)
        {
            this.NomeArquivo = NomeArquivo;
            this.ConteudoArquivo = ConteudoArquivo;

            this.Postar();
        }

        /// <summary>
        /// ALT: 07/11/2017 -
        /// Apenas posta arquivos. Acionado localmente
        /// </summary>
        private void Postar()
        {
            byte[] _bytesConteudoArquivo = Encoding.Default.GetBytes(this.ConteudoArquivo);

            FtpWebRequest objFTPRequest;

            objFTPRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + this.IPServidorFTP + "/" + this.PastaFTP + "/" + this.NomeArquivo));
            objFTPRequest.Credentials = new NetworkCredential(this.UsuarioFTP, this.SenhaFTP);
            objFTPRequest.KeepAlive = false;
            objFTPRequest.UseBinary = true;
            objFTPRequest.ContentLength = _bytesConteudoArquivo.Length;
            objFTPRequest.Method = WebRequestMethods.Ftp.UploadFile;

            Stream requestStream = objFTPRequest.GetRequestStream();
            requestStream.Write(_bytesConteudoArquivo, 0, _bytesConteudoArquivo.Length);
            requestStream.Close();

            FtpWebResponse response = (FtpWebResponse)objFTPRequest.GetResponse();

            Console.WriteLine("Upload completo do arquivo, status {0}", response.StatusDescription);

            response.Close();
        }
    }
}