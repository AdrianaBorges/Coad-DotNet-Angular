using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Coad.GenericCrud.Service.Base;
using Coad.GenericCrud.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Base;
using System.Transactions;
using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.Config.DataAttributes;
using COAD.CORPORATIVO.Exceptions;
using System.IO;
using GenericCrud.Util;
using System.Web;
using COAD.FISCAL.Model;
using COAD.FISCAL.XmlUtils;
using COAD.CORPORATIVO.Util;
using COAD.CORPORATIVO.Model.Dto.Custons.Batch;
using COAD.SEGURANCA.Model.Custons;
using GenericCrud.Service;
using COAD.SEGURANCA.Service;
using GenericCrud.Exceptions;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Service.Custons;
using COAD.SEGURANCA.Service.Custons;
using COAD.SEGURANCA.Model.Dto.Custons.Batch;
using COAD.SEGURANCA.Service.Custons.Context;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("NFX_ID")]
    public class NFeXmlSRV : GenericService<NFE_XML, NfeXmlDTO, int>
    {
        private NFeXmlDAO _dao;

        public NFeXmlSRV()
        {
            this._dao = new NFeXmlDAO();
            this.Dao = _dao;
        }

        public NFeXmlSRV(NFeXmlDAO _dao)
        {
            this._dao = _dao;
            this.Dao = _dao;
        }

        public IList<NfeXmlDTO> ListarNFeXmlPorItemPedido(int? ipeId)
        {
            return _dao.ListarNFeXmlPorItemPedido(ipeId);
        }
        
        public string RetornarPath(int? NFX_ID)
        {
            return _dao.RetornarPath(NFX_ID);
        }

        public string ChecarERetornarFileName(string serverPath, int? NFX_ID)
        {
            string fileName = RetornarPath(NFX_ID);

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                if (File.Exists(fileName))
                {
                    return fileName;
                }

                fileName = fileName.Replace(@"\", "");
            }

            string serverFolder = SysUtils.RetornarPathNFeXML();
            string fullPath = Path.Combine(serverPath, serverFolder, fileName);

            if(!File.Exists(fullPath))
            {
                throw new FileNotFoundException(string.Format("Não possível achar o arquivo. Caminho buscado. {0}", fullPath));
            }

            return fullPath;
        }
        
        public ICollection<string> ChecarERetornarFileName(IList<NfeXmlDTO> lstNfeXml, string serverPath, bool batch = false, BatchContext batchContext = null)
        {
            ICollection<string> lstArquivos = new HashSet<string>();

            if(lstNfeXml != null)
            {
                foreach(var nfeXml in lstNfeXml)
                {
                    int? nfx_id = nfeXml.NFX_ID;
                    try
                    {
                        string fileName = RetornarPath(nfeXml.NFX_ID);

                        if (!string.IsNullOrWhiteSpace(fileName))
                        {
                            if (File.Exists(fileName))
                            {
                                lstArquivos.Add(fileName);
                                continue;
                            }
                            fileName = fileName.Replace(@"\", "");
                        }

                        string serverFolder = SysUtils.RetornarPathNFeXML();
                        string fullPath = Path.Combine(serverPath, serverFolder, fileName);

                        if (!File.Exists(fullPath))
                        {
                            throw new FileNotFoundException(string.Format("Não possível achar o arquivo. Caminho buscado. {0}", fullPath));
                        }

                        lstArquivos.Add(fullPath);

                    }
                    catch(Exception ex)
                    {
                        if (batch && batchContext != null)
                        {
                            string chaveErro = string.Format("NFeXML Id {0}.", nfx_id);
                            ServiceFactory.RetornarServico<BatchCustomSRV>().RegistrarErroBatch(new RegistroErroBatchDTO()
                            {
                                batchEx = batchContext,
                                context = chaveErro,
                                e = ex,
                                nomeDaExecucao = "Download Lote Nota Fiscal",
                                projeto = "CORPORATIVO",
                                servico = "ItemPedidoSRV",
                                tipoJob = 4,
                                descricaoCodigoReferencia = "Código da NFeXML",
                                codReferencia = nfx_id,
                                contabilizarFalha = false
                            });                            
                        }
                        else
                        {
                            throw ex;
                        }
                    }
                }
            }

            return lstArquivos;
        }

        public string ChecarERetornarZipFileName(string serverPath, string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                if (File.Exists(fileName))
                {
                    return fileName;
                }

                fileName = fileName.Replace(@"\", "");
            }

            string serverFolder = SysUtils.RetornarPathNFeXML();
            string fullPath = Path.Combine(serverPath, serverFolder, fileName);

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException(string.Format("Não possível achar o arquivo. Caminho buscado. {0}", fullPath));
            }

            return fullPath;
        }

        public void Incluir(int? ipeId, string chaveNota, string fileName, int? tipoQualificacao)
        {
            
                var nfeXml = new NfeXmlDTO()
                {
                    IPE_ID = ipeId,
                    NFX_CHAVE_NOTA = chaveNota,
                    NFX_PATH_NOTA = fileName,
                    NFX_TIPO = tipoQualificacao
                };

                Save(nfeXml);
        }


        public bool ChecarSeExisteNotaDeServico(IEnumerable<NfeXmlDTO> lstNFeXml)
        {
            if (lstNFeXml != null)
            {
                var count = lstNFeXml.Where(x => x.NFX_TIPO == 1).Count();
                return (count > 0);
            }

            return false;
        }


        public string RetornarChaveDaNFe(HttpPostedFileBase file)
        {
            string codigoDaNota = null;
            if (file != null)
            {
                var stream = file.InputStream;
                StreamReader reader = new StreamReader(stream);
                NotaFiscal nfe = XmlUtil.LoadFromStream<NotaFiscal>(reader);

                if (file != null)
                {
                    if (nfe.lstInfNFe != null && nfe.lstInfNFe.Count > 0)
                    {
                        codigoDaNota = nfe.lstInfNFe[0].Id;
                        codigoDaNota = codigoDaNota.Replace("NFe", "");
                    }

                }
            }

            return codigoDaNota;
        }

        /// <summary>
        /// Pega o arquivo de upload postado na sessão converte na NFe e retorna sua chave
        /// </summary>
        /// <returns></returns>
        public string RetornaChaveDoArquivoDeUploadNaSessao()
        {
            var arquivo = UploadUtil.RetornarArquivoDeUpload();
            var chaveNota = RetornarChaveDaNFe(arquivo);

            return chaveNota;
        }

        public IList<NfeXmlDTO> ListarTodasAsNotasDoTipoProduto()
        {
            return _dao.ListarTodasAsNotasDoTipoProduto();
        }

        public IList<NfeXmlDTO> ListarNFeXmlPorPedido(int? pedCrmId)
        {
            return _dao.ListarNFeXmlPorPedido(pedCrmId);
        }

        public IList<NfeXmlDTO> ListarNFeXmlProdutoPorItemPedido(int? ipeId)
        {
            return _dao.ListarNFeXmlProdutoPorItemPedido(ipeId);
        }

        public void MarcarComoExtornado(IList<NfeXmlDTO> lstNfx)
        {
            if(lstNfx != null)
            {
                foreach(var nfx in lstNfx)
                {
                    nfx.NFX_DATA_EXCLUSAO = DateTime.Now;
                    nfx.NFX_NUM_EXTORNADO = true;
                }
                SaveOrUpdateAll(lstNfx);
            }
        }
    }
}
