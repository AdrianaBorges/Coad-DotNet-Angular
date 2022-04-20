
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
using Coad.GenericCrud.Repositorios.Base;
using GenericCrud.Config.DataAttributes;
using Ionic.Zip;
using GenericCrud.Util;
using System.Web;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("INF_ANO", "INF_REMESSA", "INF_ENVIO", "INF_PRO_ID", "INF_TIPO")]
    public class InformativoSemanalEnvioSRV : GenericService<INFORMATIVO_SEMANAL_ENVIO, InformativoSemanalEnvioDTO, object>
    {
        public InformativoSemanalEnvioDAO _dao = new InformativoSemanalEnvioDAO();
        public ParametrosSRV _paramSRV = new ParametrosSRV();

        public InformativoSemanalEnvioSRV()
        {
            this.Dao = _dao;
        }

        public List<InformativoSemanalEnvioDTO> Buscar(string _ano, string _remessa, int _envio, int? _produto = null)
        {
            return _dao.Buscar(_ano, _remessa, _envio, _produto);
        }
        
        public void GerarRemessa(string _ano, string _remessa, string _produto, int? _envio, string _usuario, DateTime? _entrega = null, bool _temMDP = false)
        {
            _dao.GerarRemessa(_ano, _remessa, _produto, _envio, _usuario, _entrega, _temMDP);
        }

        public IList<InformativoSemanalEnvioDTO> RemessaAenviar(string ano, string remessa, int envio)
        {
            return _dao.RemessaAenviar(ano, remessa, envio);
        }
        public IList<InformativoSemanalEnvioDTO> GerarArquivo(string ano, string remessa, int envio, int produto)
        {
            return _dao.GerarArquivo(ano, remessa, envio, produto);
        }
        public void ConfirmarPostagens(string _ano, string _remessa, bool _MDP)
        {
            _dao.ConfirmarPostagens(_ano, _remessa, _MDP);
        }
        public string BaixarArquivos(string ano, string remessa, int? envio, HttpContext _url)
        {
            var _lstRetorno = new List<String>();
            var _produtosDeInformativo = new ProdutosSRV().ObterProdutosInformativoSemanal(false, envio);

            foreach (var produto in _produtosDeInformativo)
            {
                var _gerarArquivo = this.GerarArquivo(ano, remessa, (int)envio, (int)produto.PRO_ID);

                if (_gerarArquivo.Count() > 0)
                {
                    var _conteudoArquivo = "";
                    var _nomeArquivo = "";

                    foreach (var aEnviar in _gerarArquivo)
                    {
                        _nomeArquivo = aEnviar.INF_ARQUIVO;
                        _conteudoArquivo += aEnviar.INF_TEXTO + Environment.NewLine;
                    }

                    if (!String.IsNullOrWhiteSpace(_nomeArquivo))
                    {
                        string _Path = "Http://" + _url.Request.Url.Authority + "/temp/" + _nomeArquivo;
                        string _filePath = _url.Server.MapPath("~/temp/" + _nomeArquivo);
                        string _curDir = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + "\\temp\\" + _nomeArquivo;

                        System.IO.File.WriteAllText(_curDir, _conteudoArquivo, System.Text.Encoding.GetEncoding("ISO-8859-1"));

                        _lstRetorno.Add(_curDir);
                    }
                }
            }

            var _zipName = ano + remessa +"_"+envio.ToString()+ ".zip";
            var _pathretorno = this.GerarArquivos(_lstRetorno, _url, _zipName);

            return _pathretorno;

        }

        private string GerarArquivos(List<String> _lstRetorno, HttpContext _url, string _zipName)
        {
            ZipFile zipFile = new ZipFile();
            zipFile.AddFiles(_lstRetorno, "");

            string _fullPath = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString()) + "\\temp\\" + _zipName;
            string _Path = "Http://" + _url.Request.Url.Authority + "/temp/" + _zipName;

            zipFile.Save(_fullPath);

            return _Path;
        }
        public void GerarRemessaFull(string ano, string remessa, bool MDP, DateTime? dtEntrega = null)
        {
            this.MontarRemessa(ano, remessa, MDP, dtEntrega, 1);
            this.MontarRemessa(ano, remessa, MDP, dtEntrega, 2);
            var _param = _paramSRV.BuscarValor("ULTIMOINFORMATIVO");
            _param.PAR_VALOR = remessa;
            _paramSRV.Merge(_param);
        }

        public void MontarRemessa(string ano, string remessa, bool MDP, DateTime? dtEntrega = null, int? envio = null)
        {
            var _produtosDeInformativo = new ProdutosSRV().ObterProdutosInformativoSemanal(MDP, envio);
            
            foreach (var produto in _produtosDeInformativo)
            {
                this.GerarRemessa(ano, remessa, produto.PRO_ID.ToString().PadLeft(2, '0'), envio, SessionContext.autenticado.USU_LOGIN, dtEntrega, MDP);
            }
        }

    }
}
