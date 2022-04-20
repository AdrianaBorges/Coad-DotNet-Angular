using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

using COAD.SEGURANCA.Service;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.CORPORATIVO.Service;
using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Exceptions;
using COAD.CORPORATIVO.Model.Dto;

namespace INTREGRANF
{
    public partial class frmPrincipal : Form
    {
        string _retorno = "";
        NotaFiscalSRV _srv = new NotaFiscalSRV();
        Boolean _parar = false;
        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void btnProcessar_Click(object sender, EventArgs e)
        {
            this.Importar();
        }
        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            this.Atualizar();
        }

        private void Atualizar()
        {
            try
            {

                string _login = "coadsys";
                string _senha = "123456";

                SessionContext.usu_login_desktop = "COADSYS";

                _retorno = new UsuarioSRV().RealizarLogin(_login, _senha, "COADCORP");

                if (_retorno.Trim() != "")
                    throw new Exception(_retorno);

                //this.ProcessarArquivos(@"I:\nfe_gerada\XML COAD 1", "", false);
                //this.ProcessarArquivos(@"I:\nfe_gerada\XML APCJ 1", "", false);

              //  this.ProcessarArquivos(@"I:\nfe_gerada\XML APC 1\Leandro\Maio 2018", "", false);
                this.ProcessarArquivos(@"I:\nfe_gerada\XML APC 1\Leandro\JUNHO 2018", "", false);

                if (_retorno.Length > 0)
                    throw new Exception(_retorno);

                MessageBox.Show("Arquivo atualizado com sucesso!!");

            }
            catch (Exception ex)
            {
                string _erropath = Path.GetDirectoryName(Application.ExecutablePath) + "\\Erro.txt";

                MessageBox.Show("Arquivo processado com erros. Verifique o arquivo em " + _erropath + "( " + ex.Message + " )");
            }
        }
        private void Importar()
        {
            try
            {

                string _login = "coadsys";
                string _senha = "123456";

                SessionContext.usu_login_desktop = "COADSYS";

                _retorno = new UsuarioSRV().RealizarLogin(_login, _senha, "COADCORP");

                if (_retorno.Trim() != "")
                    throw new Exception(_retorno);

                //this.ProcessarArquivos(@"I:\nfe_gerada\XML APC 1\XML IMPORTADAS\MARÇO 2017","");
                //this.ProcessarArquivos(@"I:\nfe_gerada\XML APC 1\XML IMPORTADAS\Abril 2017",""); 
                //this.ProcessarArquivos(@"I:\nfe_gerada\XML APC 1\XML IMPORTADAS\MARÇO 2017",""); 
                //this.ProcessarArquivos(@"I:\nfe_gerada\XML APCJ 1",""); 
                //this.ProcessarArquivos(@"I:\nfe_gerada\XML COAD 1\XML IMPORTADAS\Abril 2017","");
                //this.ProcessarArquivos(@"I:\nfe_gerada\XML COAD 1\XML IMPORTADAS\março 2017","");


                //this.ProcessarArquivos(@"I:\nfe_gerada\XML APC 1\XML IMPORTADAS\MAIO 2017",""); 
                //this.ProcessarArquivos(@"I:\nfe_gerada\XML APC 1\XML IMPORTADAS\JUNHO 2017","");
                //this.ProcessarArquivos(@"I:\nfe_gerada\XML APCJ 1","");
                //this.ProcessarArquivos(@"I:\nfe_gerada\XML COAD 1\XML IMPORTADAS\MAIO 2017","");
                //this.ProcessarArquivos(@"I:\nfe_gerada\XML COAD 1\XML IMPORTADAS\JUNHO 2017","");

                //this.ProcessarArquivos(@"I:\nfe_gerada\XML APC 1\XML IMPORTADAS\JUNHO 2017", "");
                //this.ProcessarArquivos(@"I:\nfe_gerada\XML APC 1\XML IMPORTADAS\AGOSTO 2017","");
                //this.ProcessarArquivos(@"I:\nfe_gerada\XML APC 1\XML IMPORTADAS\JULHO 2017","");
                //this.ProcessarArquivos(@"I:\nfe_gerada\XML APC 1\XML IMPORTADAS\Setembro 2017", "");
                //this.ProcessarArquivos(@"I:\nfe_gerada\XML COAD 1\XML IMPORTADAS\JUNHO 2017","");
                //this.ProcessarArquivos(@"I:\nfe_gerada\XML COAD 1\XML IMPORTADAS\JULHO 2017", "");
                //this.ProcessarArquivos(@"I:\nfe_gerada\XML APCJ 1","");

                //this.ProcessarArquivos(@"I:\nfe_gerada\XML APC 1\Leandro\JANEIRO 2018", "", false);
                //this.ProcessarArquivos(@"I:\nfe_gerada\XML APC 1\Leandro\Fevereiro 2018", "", false);
                //this.ProcessarArquivos(@"I:\nfe_gerada\XML APC 1\Leandro\Março 2018", "", false);
                //this.ProcessarArquivos(@"I:\nfe_gerada\XML APC 1\Leandro\Abril 2018", "", false);

               // this.ProcessarArquivos(@"I:\nfe_gerada\XML APC 1\Leandro\Maio 2018", "", false);

                //this.ProcessarArquivos(@"I::\nfe_gerada\XML COAD 1", "", false);
                //this.ProcessarArquivos(@"I:\nfe_gerada\XML APCJ 1", "", false);
                
                if (_retorno.Length > 0)
                    throw new Exception(_retorno);

                MessageBox.Show("Arquivo atualizado com sucesso!!");

            }
            catch (Exception ex)
            {
                string _erropath = Path.GetDirectoryName(Application.ExecutablePath) + "\\Erro.txt";

                MessageBox.Show("Arquivo processado com erros. Verifique o arquivo em " + _erropath + "( " + ex.Message + " )");
            }
        }

        public void ProcessarArquivos(string _path, string _pasta, bool _atualizar)
        {
            DirectoryInfo _dir = new DirectoryInfo(_path);
            _retorno = "";
            
            FileInfo[] _files = _dir.GetFiles("*.xml", SearchOption.AllDirectories);
            XmlDocument _doc = new XmlDocument();

            lbl01.Visible = true;
            lblItem.Visible = true;
            lblTotal.Visible = true;
            lbl01.Text = _path;
            progressBar1.Visible = true;
            progressBar1.Maximum = 0;
            progressBar1.Value = 0;
            progressBar1.Maximum = _files.Length;
            Application.DoEvents();

            lblTotal.Text = _files.Count().ToString();

            foreach (FileInfo _file in _files)
            {                
                _doc.Load(_file.FullName);
                // ---
                StringWriter sw = new StringWriter();
                XmlTextWriter tx = new XmlTextWriter(sw);
                _doc.WriteTo(tx);
                // ---
                string _str = sw.ToString();

                var _nf = new NotaFiscalDTO();

                try
                {
                    if (_atualizar)
                        _nf = _srv.AtualizarNotaFiscalDadosXML(_doc, 0);
                    else
                        _nf = _srv.GravarDadosXML(_doc, 0);
                }
                catch (Exception ex)
                {

                    StringBuilder sbMessage = new StringBuilder();
                    ExceptionFormatter.RecursiveShowMessage(ex, sbMessage);
                    _retorno += sbMessage.ToString();
                }

                if (_parar)
                {
                    _parar = false;

                    DialogResult dialogResult = MessageBox.Show("Deseja realmente parar a importação ?", "Importação de Notas (Saida)", MessageBoxButtons.YesNo);
                    
                    if (dialogResult == DialogResult.Yes)
                        Environment.Exit(1);
                }

                progressBar1.Value += 1;

                lbl01.Text = _file.DirectoryName;
                lblItem.Text = progressBar1.Value.ToString();

                Application.DoEvents();
            }
            
            string _erropath = Path.GetDirectoryName(Application.ExecutablePath)+"\\Erro_"+_pasta+".txt";

            System.IO.File.WriteAllText(_erropath, _retorno);

        }

        private void Parar_Click(object sender, EventArgs e)
        {
            _parar = true;
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();

            // this.Importar();
            this.Atualizar();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _srv.GravarDadosXML(0);
        }
    }
}
