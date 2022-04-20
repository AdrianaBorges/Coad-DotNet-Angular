using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using ExcelLibrary.SpreadSheet;
using System.Data;
using System.IO;
using System.Web;
using Microsoft.Office.Interop.Excel;
using System.Text.RegularExpressions;
using Coad.Reflection;
using GenericCrud.Metadatas;
using GenericCrud.Excel.Metadatas;
using System.Reflection;
using System.Security.AccessControl;
using GenericCrud.Util;

namespace GenericCrud.Excel
{
    public class ExcelLoad : IDisposable
    {
        public Workbook WorkBook { get; set; }
        public Microsoft.Office.Interop.Excel.Application app { get; set; }
        public string FileName { get; set; }

        public ExcelLoad()
        {

        }

        public ExcelLoad(string fullFileName)
        {
            LoadFile(fullFileName);
        }

        public List<string[]> LeArquivo(string Arquivo)
        {

            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();

            Workbook workBook = app.Workbooks.Open(Arquivo, ReadOnly: true);
            List<string[]> Linhas;

            try
            {
                Linhas = new List<string[]>();

               
                int numSheets = workBook.Sheets.Count;
                //esse loop vai percorrer todas as pastas de trabalho do excel.
                for (int sheetNum = 1; sheetNum <= numSheets; sheetNum++)
                {
                    Worksheet sheet = (Worksheet)workBook.Sheets[sheetNum];
                    int numColumns = sheet.Columns.Count;
                    int numRows = sheet.Rows.Count;

                    Range excelRange = sheet.UsedRange;
                    //Pega todo conteúdo de uma linha e transforma e um array de objetos.
                    object[,] Linha = (object[,])excelRange.get_Value(XlRangeValueDataType.xlRangeValueDefault);

                    if (Linha != null)
                    {
                        for (int lin = 0; lin <= Linha.GetUpperBound(0) - 1; lin++)
                        {
                            string[] celulas = new string[Linha.GetUpperBound(1)];

                            for (int col = 0; col <= Linha.GetUpperBound(1) - 1; col++)
                            {
                                if (Linha[(lin + 1), (col + 1)] != null)
                                    celulas[col] = Linha[(lin+1), (col+1)].ToString();
                            }

                            Linhas.Add(celulas);

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao importar planilha ("+ex.Message+")");
            }
            finally
            {
                workBook.Close();
            }

            return Linhas;
     
        }
        public string[,] Load(string _arquivo)
        {
            try
            {
                //----------

               string[,] _planilha = null;
               int colunas = 0;
                              
               List<string[]> arquivo = LeArquivo(_arquivo);
               var linhas = arquivo.Count();
               if (linhas > 0)
                  colunas = arquivo[0].Count();

               if (linhas > 0)
               {
                   _planilha = new string[linhas, colunas];

                   for (int lin = 0; lin <= arquivo.Count() - 1; lin++)
                   {
                       var item = arquivo[lin];

                       for (int ind = 0; ind <= item.Count() - 1; ind++)
                       {
                           if (item[ind] != null)
                               _planilha[lin, ind] = item[ind].ToString();
                       }

                   }

               }

                return _planilha;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string ExportDB(string _nomearquivo, string[,] _conteudo, HttpContext _url)
        {
            try
            {
                string _Path = "Http://" + _url.Request.Url.Authority + "/temp/" + _nomearquivo + ".xls";
                string _filePath = _url.Server.MapPath("~/temp/" + _nomearquivo + ".xls");


                Application app = new Application();
                Workbook workBook = app.Workbooks.Add(System.Reflection.Missing.Value); // Criando objeto Workbook
                Worksheet worksheet = (Worksheet)workBook.Sheets["Plan1"]; // Criando objeto Workbook

                object misValue = System.Reflection.Missing.Value;

                int _lin = _conteudo.GetLength(0) - 1;
                int _col = _conteudo.GetLength(1) - 1;
                string _valor = "";

                for (int _ind = 0; _ind <= _lin; _ind++)
                {
                    for (int _ind2 = 0; _ind2 <= _col; _ind2++)
                    {
                        _valor = _conteudo[_ind, _ind2].ToString().Trim();
                        worksheet.Cells[(_ind + 1), (_ind2 + 1)] = _valor;
                    }
                }


                //Salvando informações
                workBook.SaveAs(_filePath, XlFileFormat.xlWorkbookNormal, misValue, misValue, false, misValue, XlSaveAsAccessMode.xlNoChange, misValue, misValue, misValue, misValue, misValue);
                workBook.Close(true, misValue, misValue);

                //Eliminando o Excel da memória
                worksheet = null;
                workBook = null;
                app.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
                app = null;
                GC.Collect();

                return _Path;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Exportar (" + ex.Message + ")");
            }

        }
        public string[,] MontarArquivo<T>(List<T> _lista)
        {
            string[,] _planilha = null;
            
            if (_lista.Count > 0)
            {

                PropertyInfo[] _listacampos = _lista[0].GetType().GetProperties();

                _planilha = new string[(_lista.Count + 1), _listacampos.Length];

                for (int ind = 0; ind <= _listacampos.Length - 1; ind++)
                {
                    var _campodb = _listacampos[ind].Name;

                    if (_campodb == null || _campodb == "")
                        _campodb = "INFORME_NOME";

                    _planilha[0, ind] = _listacampos[ind].Name;
                }

                for (int lin = 0; lin <= _lista.Count - 1; lin++)
                {

                    for (int ind = 0; ind <= _listacampos.Length - 1; ind++)
                    {
                        var _campodb = _listacampos[ind].Name;
                        var _campovalor = "";
                        //----------
                        PropertyInfo propertySet = _lista[lin].GetType().GetProperty(_campodb.Trim());

                        var tipo = propertySet.GetValue(_lista[lin]);
                        
                        if (tipo != null)
                        {
                            TypeCode _campotipo = Type.GetTypeCode(tipo.GetType());
                            
                            switch (_campotipo)
                            {
                                case TypeCode.Decimal:
                                    {
                                        var _valor = propertySet.GetValue(_lista[lin]).ToString();
                                        _campovalor = _valor.Replace('.', ',');

                                        break;
                                    }
                                case TypeCode.DateTime:
                                    {
                                        var _data = (DateTime)propertySet.GetValue(_lista[lin]);
                                        _campovalor = _data.Day.ToString() + "/" +
                                                      _data.Month.ToString() + "/" +
                                                      _data.Year.ToString() ;
                                        break;
                                    }
                                default:
                                    {
                                        _campovalor = propertySet.GetValue(_lista[lin]).ToString();
                                        break;
                                    }
                            }
                        }

                        _planilha[(lin + 1), ind] = _campovalor;

                    }

                }

            }

            return _planilha;

        }

        public string Export<T>(List<T> _lista, string _arquivo, HttpContext _url)
        {
            var _conteudo = this.MontarArquivo(_lista);

            return this.Export(_arquivo, _conteudo, _url);
        }

        public string Export(string _arquivo, string[,] _conteudo, HttpContext _url)
        {
            try
            {
                //----------
                string _Path = "Http://" + _url.Request.Url.Authority + "/temp/" + _arquivo + ".xls";
                string _filePath = _url.Server.MapPath("~/temp/" + _arquivo + ".xls");
                int _lin = _conteudo.GetLength(0) - 1;
                int _col = _conteudo.GetLength(1) - 1;
                string _linhatab = "";
                string _replacement = "";

                if (!System.IO.File.Exists(_filePath))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = new StreamWriter(File.Open(_filePath, FileMode.Create), Encoding.GetEncoding("iso-8859-1")))
                    {
                        for (int _ind = 0; _ind <= _lin; _ind++)
                        {
                            _linhatab = "";
                            _replacement = "";

                            for (int _ind2 = 0; _ind2 <= _col; _ind2++)
                            {
                                if (_conteudo[_ind, _ind2] != null)
                                    _replacement = Regex.Replace(_conteudo[_ind, _ind2].ToString().Trim(), @"\t|\n|\r", ";");

                                if (_ind == 0)
                                {
                                    if (_conteudo[_ind, _ind2] != null)
                                        _linhatab += _replacement + "\t";
                                    else
                                        _linhatab += "\t";
                                }
                                else
                                {
                                    if (_conteudo[_ind, _ind2] != null)
                                        _linhatab += _replacement + "\t";
                                    else
                                        _linhatab += "\t";
                                }
                            }

                            sw.WriteLine(_linhatab, Encoding.ASCII);
                        }

                    }
                }

                return _Path;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Workbook LoadFile(string fullFileName)
        {
            this.FileName = fullFileName;
            this.app = new Microsoft.Office.Interop.Excel.Application();
            Workbook workBook = this.app.Workbooks.Open(fullFileName, ReadOnly: true,IgnoreReadOnlyRecommended: true);

            this.WorkBook = workBook;
            return workBook;
        }

       
        [MetodoTopLevel]
        public IList<T> ToDTO<T>(bool findAllSheets = true, int? sheetIndex = null, int limitTo = -1)
        {
            return ToDTO<T>(this.WorkBook, findAllSheets, sheetIndex, limitTo);
        }

        [MetodoTopLevel]
        public IList<T> ToDTO<T>(Workbook workBook, bool findAllSheets = true, int? sheetIndex = null, int limitTo = -1)
        {
            try
            {
                IList<T> lstRetorno = new List<T>();
                if (workBook != null)
                {
                    if (findAllSheets)
                    {
                        foreach (var sheet in workBook.Sheets)
                        {
                            ToDTO<T>((Worksheet)sheet, lstRetorno, limitTo);
                        }
                    }
                    else
                    {
                        var sheetsCount = workBook.Sheets.Count;
                        if ((sheetsCount - 1) >= sheetIndex)
                        {
                            var sheet = workBook.Sheets[sheetIndex];
                            ToDTO<T>((Worksheet)sheet, lstRetorno, limitTo);
                        }
                    }

                    return lstRetorno;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao recuperar os dados na planilha.", e);
            }

            return null;

        }

        private string[] GerarCabecalho(object[,] linha1)
        {
            int count = linha1.GetUpperBound(1) ;
            string[] lstCabecalho = new string[count];

            for (var index = 1; index <= count; index++)
            {
                if (linha1 != null && linha1[1, index] != null)
                {
                    lstCabecalho[index - 1] = linha1[1, index].ToString();
                }
            }

            return lstCabecalho;
        }

        private void ToDTO<T>(Worksheet sheet, ICollection<T> lstDTO, int limitTo = -1)
        {
            if (sheet != null)
            {
                var range = sheet.UsedRange;
                object[,] linha = range.get_Value(XlRangeValueDataType.xlRangeValueDefault);

                if (linha != null)
                {
                    int rowsCount = linha.GetUpperBound(0);
                    int colsCount = linha.GetUpperBound(1);
                    
                    if(limitTo > -1 && limitTo < rowsCount)
                    {
                        rowsCount = limitTo + 1;
                    }

                    var lstHeader = GerarCabecalho(linha);

                    for (int rowIndex = 2; rowIndex <= rowsCount; rowIndex++)
                    {
                        T dtoObj = Activator.CreateInstance<T>();
                        bool inseriu = false;

                        for (int colIndex = 1; colIndex <= colsCount; colIndex++)
                        {
                            var head = lstHeader[colIndex - 1];
                            var value = linha[rowIndex, colIndex];

                            var sucesso = SetValueOnDTO<T>(dtoObj, head, value);
                            inseriu  = (inseriu || sucesso);
                        }

                        if (inseriu)
                        {
                            lstDTO.Add(dtoObj);
                        }
                    }
                }
            }
        }

        private bool SetValueOnDTO<T>(T dto, string cabecalho, object value)
        {
            if (dto != null && !string.IsNullOrEmpty(cabecalho) && value != null)
            {
                string propertyName = cabecalho;
                var campoExiste = ReflectionProvider.HasMember(dto, cabecalho);

                if (campoExiste)
                {
                    var memberInfo = ReflectionProvider.GetMemberInfo<T>(propertyName);
                    campoExiste = (ReflectionProvider.GetMemberAttribute<ExcelIgnoreAttribute>(memberInfo) == null);
                }

                if (!campoExiste)
                {
                    var lstProperty = ReflectionProvider.GetMemberByAttributes<ExcelColumnAttribute>(dto);

                    foreach (var prop in lstProperty)
                    {
                        var excelAttr = ReflectionProvider.GetMemberAttribute<ExcelColumnAttribute>(prop);

                        if (excelAttr.Name == propertyName)
                        {
                            propertyName = prop.Name;
                            break;
                        }
                    }
                }                

                Type tipoDoCampo = ReflectionProvider.GetMemberType(dto, propertyName);
              
                if (tipoDoCampo == typeof(int))
                {
                    string valorString = value.ToString();
                    int valor = int.Parse(valorString);
                    ReflectionProvider.SetMemberValue<int>(dto, propertyName, valor);
                }
                else
                if (tipoDoCampo == typeof(int?))
                {
                    string valorString = value.ToString();
                    int valor = int.Parse(valorString);
                    ReflectionProvider.SetMemberValue<int?>(dto, propertyName, valor);
                }
                else
                if (tipoDoCampo == typeof(float))
                {
                    string valorString = value.ToString();
                    float valor = float.Parse(valorString);
                    ReflectionProvider.SetMemberValue<float>(dto, propertyName, valor);
                }
                else
                if (tipoDoCampo == typeof(float?))
                {
                    string valorString = value.ToString();
                    float valor = float.Parse(valorString);
                    ReflectionProvider.SetMemberValue<float?>(dto, propertyName, valor);
                }
                else
                if (tipoDoCampo == typeof(double))
                {
                    string valorString = value.ToString();
                    double valor = double.Parse(valorString);
                    ReflectionProvider.SetMemberValue<double>(dto, propertyName, valor);
                }
                else
                if (tipoDoCampo == typeof(double?))
                {
                    string valorString = value.ToString();
                    double valor = double.Parse(valorString);
                    ReflectionProvider.SetMemberValue<double?>(dto, propertyName, valor);
                }
                else
                if (tipoDoCampo == typeof(decimal))
                {
                    string valorString = value.ToString();
                    decimal valor = int.Parse(valorString);
                    ReflectionProvider.SetMemberValue<decimal>(dto, propertyName, valor);
                }
                else
                if (tipoDoCampo == typeof(decimal?))
                {
                    string valorString = value.ToString();
                    decimal valor = int.Parse(valorString);
                    ReflectionProvider.SetMemberValue<decimal?>(dto, propertyName, valor);
                }
                else
                if (tipoDoCampo == typeof(char))
                {
                    string valorString = value.ToString();
                    char valor = char.Parse(valorString);
                    ReflectionProvider.SetMemberValue<char>(dto, propertyName, valor);
                }
                else
                if (tipoDoCampo == typeof(char?))
                {
                    string valorString = value.ToString();
                    char valor = char.Parse(valorString);
                    ReflectionProvider.SetMemberValue<char?>(dto, propertyName, valor);
                }
                else
                {
                    string valorString = value.ToString();
                    ReflectionProvider.SetMemberValue<string>(dto, propertyName, valorString);
                }
                return true;
            }

                return false;
        }


        public void Dispose()
        {
            if (this.WorkBook != null)
            {
                this.WorkBook.Close(false, this.FileName);
                app.Quit();
            }
        }

        public Worksheet NewFile(string sheetName = "Planilha 1")
        {

            Application app = new Application();
            Workbook workbook = app.Workbooks.Add(System.Reflection.Missing.Value); // Criando objeto Workbook
            Worksheet sheet = (Worksheet)workbook.Sheets["Plan1"]; // Criando objeto WorkbookWorkbook workbook = this.app.Workbooks.Add(System.Reflection.Missing.Value);

            this.WorkBook = workbook;
            //Worksheet sheet = this.WorkBook.Sheets.Add();
            sheet.Name = sheetName;
            return sheet;
        }

        public void EscreverCabecalhoNaPlanilha<T>(T dtoTemplate, Worksheet worksheet)
        {
            if (dtoTemplate != null && worksheet != null)
            {
                EscreverLinhas(dtoTemplate, worksheet, 1, (label, value, rowIndex, colIndex, cellOptions) => {
                    
                    var cell = (Range)worksheet.Cells[1, colIndex];

                    cell.Value = label;
                });
            }
        }

        public void EscreverDadosNaPlanilha<T>(ICollection<T> lstDTO, Worksheet worksheet)
        {
            if (lstDTO != null && lstDTO.Count > 0 && worksheet != null)
            {
                EscreverLinhas(lstDTO, worksheet, 2, (label, value, rowIndex, colIndex, cellOptions) => {
                    var cell = (Range)worksheet.Cells[rowIndex, colIndex];
                    cell.Value = value;

                    if (cellOptions.Comentario != null)
                    {
                        cell.AddComment(cellOptions.Comentario);
                    }
                });
            }
        }

        public void EscreverLinhas<T>(T dto, Worksheet worksheet, int startRowIndex, Action<string, object, int, int, CellOptions> EscreverValor)
        {
            EscreverLinhas<T>(new List<T>() { dto }, worksheet, startRowIndex, EscreverValor);
        }

        public void EscreverLinhas<T>(ICollection<T> lstDTO, Worksheet worksheet, int startRowIndex, Action<string, object, int, int, CellOptions> EscreverValor)
        {
            if (lstDTO != null && worksheet != null)
            {
                int rowIndex = startRowIndex;
                foreach (var dto in lstDTO)
                {
                    string headerName = null;
                    var lstProperty = ReflectionProvider.GetProperties(dto);
                    //var lstProperty = ReflectionProvider.GetMemberByAttributes<ExcelColumnAttribute>(dto);

                    int columnIndex = 1;
                    foreach (var prop in lstProperty)
                    {
                        var ignore = (ReflectionProvider.GetMemberAttribute<ExcelIgnoreAttribute>(prop) != null);

                        if (ignore == false)
                        {
                            var cellOptions = new CellOptions();

                            var excelAttr = ReflectionProvider.GetMemberAttribute<ExcelColumnAttribute>(prop);
                            if (excelAttr != null && !string.IsNullOrWhiteSpace(excelAttr.Name))
                            {
                                headerName = excelAttr.Name;
                                if (!string.IsNullOrWhiteSpace(excelAttr.CommentFrom))
                                {
                                    var commentValue = ReflectionProvider.GetPropertyValue<object>(dto, excelAttr.CommentFrom);
                                    if(commentValue != null)
                                    {
                                        cellOptions.Comentario = commentValue;
                                    }
                                }
                            }
                            else
                            {
                                headerName = prop.Name;
                            }
                            var propertyValue = prop.GetValue(dto);
                            
                            EscreverValor(headerName, propertyValue, rowIndex, columnIndex, cellOptions);
                          
                            columnIndex++;
                        }
                    }

                    rowIndex++;
                }
            }
        }

        public void ToSheet<T>(string saveFileName, ICollection<T> lstDTOs)
        {
            if(!string.IsNullOrWhiteSpace(saveFileName) && lstDTOs != null && lstDTOs.Count > 0)
            {
                try
                {
                    FileUtil.CriarDiretorioPermisaoServicoWin(saveFileName);
                    if (FileUtil.IsDirectoryWritable(saveFileName))
                    {
                        var workSheet = NewFile();                    
                        var dto0 = lstDTOs.ToList()[0];
                        EscreverCabecalhoNaPlanilha<T>(dto0, workSheet);
                        EscreverDadosNaPlanilha<T>(lstDTOs, workSheet);

                        //workSheet.SaveAs(saveFileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlExcel8);
                        WorkBook.SaveCopyAs(saveFileName);
                        //workSheet.SaveAs(
                        
                        
                        
                        //    saveFileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlExcel8, Missing.Value,
                        //    Missing.Value, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                        //    Microsoft.Office.Interop.Excel.XlSaveConflictResolution.xlUserResolution, true,
                        //    Missing.Value);
                    }
                }
                catch(Exception e)
                {
                    throw new Exception("Ocorreu um erro ao tentar gerar os dados na planilha", e);
                }
                finally
                {
                    if(WorkBook != null)
                    {
                        WorkBook.Close(false);
                    }
                }
            }
        }
    }

}
