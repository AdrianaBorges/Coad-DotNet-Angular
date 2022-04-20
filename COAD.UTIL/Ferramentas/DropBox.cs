using Dropbox.Api;
using Dropbox.Api.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.UTIL.Ferramentas
{
    /// <summary>
    /// ALT: 07/11/2017 - 
    /// Classe contendo métodos para interagir com Dropbox
    /// </summary>
    public class DropBox
    {
        private DropboxClient dbx { get; set; }

        /// <summary>
        /// ALT: 07/11/2017 -
        /// Efetua o upload conforme parametros informados
        /// </summary>
        /// <param name="hash"></param>
        public async Task DropboxUpload(string pasta, string arquivo, string conteudo)
        {
            using (var mem = new MemoryStream(Encoding.Default.GetBytes(conteudo)))
            {
                var updated = await this.dbx.Files.UploadAsync(pasta + "/" + arquivo, WriteMode.Overwrite.Instance, body: mem);
            }
        }

        /// <summary>
        /// ALT: 07/11/2017 -
        /// Conecta diretamente ao Dropbox da COAD/GRÁFICA
        /// </summary>
        public DropBox()
        {
            this.dbx = new DropboxClient("whRYLm8-24MAAAAAAAAHNl_BWFEp6YH43ykASLu3qFfBP6Ia2-3ovA4VUtTfQxzO");
        }

        /// <summary>
        /// ALT: 07/11/2017 -
        /// Conecta ao Dropbox com o hash de login informado
        /// </summary>
        /// <param name="hash"></param>
        public DropBox(string hash)
        {
            this.dbx = new DropboxClient(hash);
        }

        /// <summary>
        /// ALT: 07/11/2017 -
        /// Verifica se a pasta já existe
        /// </summary>
        /// <param name="hash"></param>
        public bool PastaExiste(string pasta)
        {
            try
            {
                var pastas = this.dbx.Files.ListFolderAsync(pasta);
                var result = pastas.Result;
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// ALT: 07/11/2017 -
        /// Cria uma pasta com o nome informado
        /// </summary>
        /// <param name="hash"></param>
        public bool CriarPasta(string pasta)
        {
            try
            {
                var argumentosDaPasta = new CreateFolderArg(pasta);
                var pastas = this.dbx.Files.CreateFolderV2Async(argumentosDaPasta);
                var result = pastas.Result;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
