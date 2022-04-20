using System;
using System.Web;
using System.IO;
using System.Web.Mvc;
using System.Diagnostics;
using System.Collections.Generic;
using System.Reflection;

namespace COAD.CORPORATIVO.Relatorio
{
    public class ReportDataBase
    {
        public string usuario { get; set; }
        public string senha { get; set; }
        public string servidor { get; set; }
        public string banco { get; set; }

    }
    public class ReportParam
    {
        public ReportParam(string _name, string _value)
        {
            this.name = _name;
            this.value = _value;
        }
        public string name {get;set;}
        public string value {get;set;}
    }

    public class ReportViewer
    {
        private List<ReportParam> _parametros { get; set; }
        public ReportViewer()
        {
            _parametros = new List<ReportParam>();
        }
        public void Preview(HttpContext _contexto, string _pathrpt, string _database)
        {
        // //   var rd = new ReportDocument();

        //    Boolean ret = false;
        //    rd.Load(_pathrpt);

        //    ReportDataBase _param = this.GetDataBase(_database);

        //    if (_param == null)
        //        throw new Exception("Paramentros não informados!! Verifique!!");

        //    rd.SetDatabaseLogon(_param.usuario, _param.senha, _param.servidor, _param.banco);

        //    foreach (var item in this._parametros)
        //    {
        //        rd.SetParameterValue(item.name, item.value);
        //    }

        //    System.IO.Stream oStream = null;

        //    _contexto.Response.Clear();
        //    _contexto.Response.Buffer = true;

        ////    var _ExportToStream = rd.ExportToStream(ExportFormatType.PortableDocFormat);

        //    byte[] byteArray = null;

        //    oStream = (System.IO.Stream)_ExportToStream;

        //    if ((oStream != null) && (oStream.Length > 60000))
        //    {
        //        byteArray = new byte[oStream.Length];
        //        oStream.Read(byteArray, 0, Convert.ToInt32(oStream.Length - 1));

        //        _contexto.Response.ClearContent();
        //        _contexto.Response.ClearHeaders();
        //        _contexto.Response.ContentType = "application/pdf";
        //        _contexto.Response.BinaryWrite(byteArray);
        //        _contexto.Response.Flush();
        //        _contexto.Response.Close();

        //        oStream.Flush();
        //    }

        //    ret = ((oStream != null) && (oStream.Length > 60000));

        //    oStream.Close();
        //    oStream.Dispose();

        }
        public void Print(HttpContext _contexto, string _pathrpt, string _database)
        {
            //var  rd = new ReportDocument();
            //try
            //{
            //    rd.Load(_pathrpt);

            //    ReportDataBase _param = this.GetDataBase(_database);

            //    rd.SetDatabaseLogon(_param.usuario, _param.senha, _param.servidor, _param.banco);

            //    foreach (var item in this._parametros)
            //    {
            //        rd.SetParameterValue(item.name, item.value);
            //    }

            //    rd.PrintOptions.PaperOrientation = PaperOrientation.Landscape;
            //    rd.PrintToPrinter(1, false, 0, 0);
            //}
            //catch (LogOnException engEx)
            //{
            //    throw new Exception("Ocorreu um erro nos parâmetros de Login! Verifique o usuário e senha de conexão com o Banco de Dados ==> " + engEx.Message);
            //}
            //catch (DataSourceException engEx)
            //{
            //    throw new Exception("Ocorreu um erro de conexão com o Banco de Dados ==> " + engEx.Message);
            //}
            //catch (EngineException engEx)
            //{
            //    throw new Exception("Ocorreu o seguinte erro no processamento do Contracheque ==> " + engEx.Message);
            //}
            //catch
            //{
            //    throw new Exception("Ocorreu um erro desconhecido no processamento do Contracheque");
            //}
            //finally
            //{
            //    rd.Dispose();
            //}
        }
        public ReportDataBase GetDataBase(string database)
        {
            ReportDataBase db =  new ReportDataBase();
            
            if (database == "DP")
            {
                db.usuario = "sa";
                db.senha = "N3wc04dxky";
                db.servidor = "10.228.5.14";
                db.banco = "CorporeRM";
            }

            if (database == "COADCORP")
            {
                db.usuario = "sa";
                db.senha = "C04dC0nsult0r14!";
                db.servidor = "10.228.5.58";
                db.banco = "COADCORP";
            }

            return db;
        }
        public void AddParam(string _name, string _value)
        {
            this._parametros.Add(new ReportParam(_name, _value));
        }
        public void ClearParam()
        {
            this._parametros.Clear();
        }

    }
}

