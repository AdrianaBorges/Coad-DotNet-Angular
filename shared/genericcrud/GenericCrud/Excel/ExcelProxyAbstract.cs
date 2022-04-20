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
using System.Data.Common.CommandTrees.ExpressionBuilder;

namespace GenericCrud.Excel
{
    public abstract class ExcelProxyAbstract<TWorkbook, TApp, TSheet, TDataSheet> : IDisposable where TWorkbook : class 
    {
        public TWorkbook WorkBook { get; set; }
        public TApp app { get; set; }
        public string FileName { get; set; }
        public bool zeroBasedIndex { get; set; }
        
        public abstract TSheet NewFile(string sheetName = "Planilha 1", string path = null);
        public abstract void EscreverValorNaCelula(TSheet worksheetpart, string label, object value, int rowIndex, int colIndex, CellOptions cellOptions);
        public abstract void EscreverCabecalho(TSheet worksheetpart, string label, object value, int rowIndex, int colIndex, CellOptions cellOptions);
        public abstract void SaveFile(string path);
        public virtual void ActivateApp()
        {
            this.app = Activator.CreateInstance<TApp>();
        }

        public ExcelProxyAbstract()
        {

        }

        public ExcelProxyAbstract(string fullFileName)
        {
            LoadFile(fullFileName);
        }


        public TWorkbook LoadFile(string fullFileName)
        {
            this.FileName = fullFileName;
            ActivateApp();
            TWorkbook workBook = OpenFile(fullFileName);

            this.WorkBook = workBook;
            return workBook;
        }

        public abstract TWorkbook OpenFile(string fullFileName);
        public abstract ICollection<TDataSheet> GetSheets(TWorkbook workbook);

        [MetodoTopLevel]
        public IList<T> ToDTO<T>(bool findAllSheets = true, int? sheetIndex = null, int limitTo = -1)
        {
            return ToDTO<T>(this.WorkBook, findAllSheets, sheetIndex, limitTo);
        }

        [MetodoTopLevel]
        public IList<T> ToDTO<T>(TWorkbook workBook, bool findAllSheets = true, int? sheetIndex = null, int limitTo = -1)
        {
            try
            {
                IList<T> lstRetorno = new List<T>();
                if (workBook != null)
                {
                    var lstSheet = GetSheets(workBook);
                    if (findAllSheets)
                    {
                        if(lstSheet != null)
                        {

                            foreach (var sheet in lstSheet)
                            {
                                ToDTO<T>((TDataSheet)sheet, lstRetorno, limitTo);
                            }
                        }
                    }
                    else
                    {
                        var sheetsCount = lstSheet.Count;
                        if ((sheetsCount - 1) >= sheetIndex)
                        {
                            var sheet = lstSheet.ToList()[(int) sheetIndex];
                            ToDTO<T>((TDataSheet)sheet, lstRetorno, limitTo);
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
        public abstract void ToDTO<T>(TDataSheet sheet, ICollection<T> lstDTO, int limitTo = -1);
        

        protected bool SetValueOnDTO<T>(T dto, string property, object value, string linhaRef = null)
        {
            try
            {


                if (dto != null && !string.IsNullOrEmpty(property) && value != null)
                {
                    string propertyName = property;
                    var campoExiste = ReflectionProvider.HasMember(dto, property);

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

                    if (!ReflectionProvider.HasMember(dto, propertyName))
                        return false;

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
            catch(Exception e)
            {
                throw new Exception($"Ocorreu um erro ao tenta setar o valor no campo: '{property}' e valor: '{value}'. Referência da célula {linhaRef}", e);
            }
        }

        public void EscreverCabecalhoNaPlanilha<T>(T dtoTemplate, TSheet worksheet)
        {
            if (dtoTemplate != null && worksheet != null)
            {
                var index = (zeroBasedIndex) ? 0 : 1;
                EscreverLinhas(dtoTemplate, worksheet, index, (label, value, rowIndex, colIndex, cellOptions) => {
                    EscreverCabecalho(worksheet, label, value, rowIndex, colIndex, cellOptions);
                });
            }
        }

        public void EscreverCabecalhoNaPlanilha(ICollection<string> lstCabecalho, TSheet worksheet)
        {
            var rowIndex = (zeroBasedIndex) ? 0 : 1;
            var colIndex = (zeroBasedIndex) ? 0 : 1;
            if (lstCabecalho != null && 
                lstCabecalho.Count > 0)
            {
                foreach(var cabecalho in lstCabecalho)
                {
                    EscreverCabecalho(worksheet, cabecalho, null, rowIndex, colIndex, null);
                    colIndex++;
                }
            }
            
        }

        public void EscreverDadosNaPlanilha<T>(ICollection<T> lstDTO, TSheet worksheet)
        {
            if (lstDTO != null && lstDTO.Count > 0 && worksheet != null)
            {
                var index = (zeroBasedIndex) ? 1 : 2;
                EscreverLinhas(lstDTO, worksheet, index, (label, value, rowIndex, colIndex, cellOptions) => {
                    EscreverValorNaCelula(worksheet, label, value, rowIndex, colIndex, cellOptions);
                });
            }
        }

        public void EscreverDadosNaPlanilha<T>(ICollection<object> lstObj, TSheet worksheet)
        {
            if (lstObj != null && lstObj.Count > 0 && worksheet != null)
            {
                var index = (zeroBasedIndex) ? 1 : 2;
                EscreverLinhas(lstObj, worksheet, index, (label, value, rowIndex, colIndex, cellOptions) => {
                    EscreverValorNaCelula(worksheet, label, value, rowIndex, colIndex, cellOptions);
                });
            }
        }

        public void EscreverLinhas<T>(T dto, TSheet worksheet, int startRowIndex, Action<string, object, int, int, CellOptions> EscreverValor)
        {
            EscreverLinhas<T>(new List<T>() { dto }, worksheet, startRowIndex, EscreverValor);
        }

        public void EscreverLinhas<T>(ICollection<T> lstDTO, TSheet worksheet, int startRowIndex, Action<string, object, int, int, CellOptions> EscreverValor)
        {
            if (lstDTO != null && worksheet != null)
            {
                int rowIndex = startRowIndex;
                foreach (var dto in lstDTO)
                {
                    string headerName = null;
                    var lstProperty = ReflectionProvider.GetProperties(dto);
                    //var lstProperty = ReflectionProvider.GetMemberByAttributes<ExcelColumnAttribute>(dto);

                    int columnIndex = (zeroBasedIndex) ? 0 : 1;
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
                        var workSheet = NewFile(path: saveFileName);                    
                        var dto0 = lstDTOs.ToList()[0];
                        EscreverCabecalhoNaPlanilha<T>(dto0, workSheet);
                        EscreverDadosNaPlanilha<T>(lstDTOs, workSheet);
                        SaveFile(saveFileName);
                    }
                }
                catch(Exception e)
                {
                    throw new Exception("Ocorreu um erro ao tentar gerar os dados na planilha", e);
                }
                finally
                {
                    Dispose();
                }
            }
        }

        //public void ToSheet<T>(string saveFileName, ICollection<string> lstColunas, ICollection<object> lstDados)
        //{
        //    if (!string.IsNullOrWhiteSpace(saveFileName) && 
        //        lstColunas != null && 
        //        lstColunas.Count > 0 &&
        //        lstDados != null && 
        //        lstDados.Count > 0)
        //    {
        //        try
        //        {
        //            FileUtil.CriarDiretorioPermisaoServicoWin(saveFileName);
        //            if (FileUtil.IsDirectoryWritable(saveFileName))
        //            {
        //                var workSheet = NewFile(path: saveFileName);
        //                EscreverCabecalhoNaPlanilha(lstColunas, workSheet);
        //                EscreverDadosNaPlanilha<T>(lstDTOs, workSheet);
        //                SaveFile(saveFileName);
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            throw new Exception("Ocorreu um erro ao tentar gerar os dados na planilha", e);
        //        }
        //        finally
        //        {
        //            Dispose();
        //        }
        //    }
        //}

        public abstract void Dispose();


    }

}
