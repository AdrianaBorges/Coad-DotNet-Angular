using System;
using System.Web;
using System.IO;
//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
//using CrystalDecisions.Web;
using System.Web.Mvc;
using System.Diagnostics;
using COAD.CORPORATIVO.Relatorio;
using COAD.SEGURANCA.Repositorios.Base;

namespace COAD.CORPORATIVO.Service.Dp
{
    // ALT: 01/10/2015 10h22m
    // Departamento Pessoal...
    
    public class DpSRV
    {
        public ReportViewer _viewer = new ReportViewer();
        public void Contracheque(System.Web.HttpContext Ctrl, string arquivoRPT, string dpEmpresa, string dpCpf, string dpAno, string dpMes, string dpPeriodo)
        {
            try
            {
                _viewer.ClearParam();
                _viewer.AddParam("empresa", dpEmpresa);
                _viewer.AddParam("cpf", dpCpf);
                _viewer.AddParam("ano", dpAno);
                _viewer.AddParam("mes", dpMes);
                _viewer.AddParam("periodo", dpPeriodo);

                _viewer.Preview(Ctrl, arquivoRPT, "DP");
            }
            catch (Exception ex)
            {
                throw new Exception(SysException.Show(ex));
            }
            
            //if (!String.IsNullOrWhiteSpace(dpCpf))
            //{
            //    ReportDocument rd = new ReportDocument();
            //    rd.Load(arquivoRPT);
            //    rd.SetDatabaseLogon(usuario, senha, servidor, banco);
            //    rd.SetParameterValue("cpf", dpCpf);
            //    rd.SetParameterValue("ano", dpAno);
            //    rd.SetParameterValue("mes", dpMes);
            //    rd.SetParameterValue("periodo", dpPeriodo);

            //    MemoryStream oStream;
            //    System.IO.Stream oStream = null;

            //    Ctrl.ControllerContext.HttpContext.Response.Clear();
            //    Ctrl.ControllerContext.HttpContext.Response.Buffer = true;

            //    var _ExportToStream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            //    byte[] byteArray = null;

            //    oStream = (System.IO.Stream)_ExportToStream;

            //    temContracheque = (oStream.Length > 60000); // achou contracheque?...

            //    if (temContracheque)
            //    {
            //        byteArray = new byte[oStream.Length];
            //        oStream.Read(byteArray, 0, Convert.ToInt32(oStream.Length - 1));
            //        Ctrl.ControllerContext.HttpContext.Response.ClearContent();
            //        Ctrl.ControllerContext.HttpContext.Response.ClearHeaders();
            //        Ctrl.ControllerContext.HttpContext.Response.ContentType = "application/pdf";
            //        Ctrl.ControllerContext.HttpContext.Response.BinaryWrite(byteArray);
            //        Ctrl.ControllerContext.HttpContext.Response.Flush();
            //        Ctrl.ControllerContext.HttpContext.Response.Close();
    
            //        Ctrl.ControllerContext.HttpContext.Response.ContentType = "application/pdf";
            //        Ctrl.ControllerContext.HttpContext.Response.BinaryWrite(oStream.ToArray());
            //        Ctrl.ControllerContext.HttpContext.Response.End();
            //        oStream.Flush();
            //    }
            //    oStream.Close();
            //    oStream.Dispose();
            //}

        }
    }
}
