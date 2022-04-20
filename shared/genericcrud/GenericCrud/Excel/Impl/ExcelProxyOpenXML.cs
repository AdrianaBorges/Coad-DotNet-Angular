using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericCrud.Excel.Metadatas;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GenericCrud.Excel.Impl
{
    public class ExcelProxyOpenXML : ExcelProxyAbstract<Workbook, SpreadsheetDocument, WorksheetPart, Sheet>
    {
        private string[] cellReferenceList =
        {
            "A", "B","C", "D", "E", "F", "G", "H", "I", "J",
            "K", "L","M", "N", "O", "P", "Q", "R", "S", "T",
            "U", "V","W", "X", "Y", "Z"
        };

        private string _getLetter(int index)
        {
            if(index >= 0 && index <= 25)
            {
                var value = cellReferenceList[index];
                return value;
            }

            return null;
        }

        private string _getCellReference(int colIndex, int rowIndex)
        {
            StringBuilder sb = new StringBuilder();
            _getCellReference(colIndex, sb);
            sb.Append(rowIndex);

            return sb.ToString();
        }

        private void _getCellReference(int index, StringBuilder sb, bool somarResto = true)
        {
            int multiplicador = index / 26;
            int diferenca = index % 26;

            if (multiplicador > 0)
            {
                if (multiplicador > 26)
                {
                    _getCellReference(multiplicador, sb, false);
                }
                else
                {
                    var letraPrincipal = _getLetter(multiplicador - 1);
                    sb.Append(letraPrincipal);

                    if(diferenca > -1 && somarResto)
                    {
                        var letraDiferenca = _getLetter(diferenca - 1);
                        sb.Append(letraDiferenca);
                    }

                    return;
                }

            }
            else
            {
                var letra = _getLetter(index);
                sb.Append(letra);
            }
        }

        public override void Dispose()
        {
            if(this.app != null)
            {
                this.app.Dispose();
            }
        }

        public WorkbookPart WorkbookPart { get; set; }

        public override void ActivateApp()
        {
            
        }

        public ExcelProxyOpenXML() : base()
        {
            base.zeroBasedIndex = true;
        }

        public ExcelProxyOpenXML(string fullFileName) : base(fullFileName)
        {
            base.zeroBasedIndex = true;
        }
        private SheetData InitElementsWithIndex(WorksheetPart worksheetpart, int rowIndex, int colIndex)
        {
            if (worksheetpart.Worksheet == null)
                worksheetpart.Worksheet = new Worksheet();

            var worksheet = worksheetpart.Worksheet;
            var sheetData = worksheet.Elements<SheetData>().FirstOrDefault();

            if (sheetData == null)
            {
                sheetData = new SheetData();
                worksheet.Append(sheetData);
            }
            
            var rows = sheetData.Elements<Row>();

            if(rows != null)
            {
                var count = rows.Count();
                if ((count - 1) < rowIndex)
                {
                    var length = rowIndex - (count - 1);
                    for(int index = 0; index < length; index++)
                    {
                        Row row = new Row();
                        sheetData.Append(row);
                    }
                }
                rows = sheetData.Elements<Row>();

                var indexRow = rows.ToList()[rowIndex];

                var cells = indexRow.Elements<Cell>();

                if(cells != null)
                {
                    var count1 = cells.Count();
                    if ((count1 - 1) < colIndex)
                    {
                        var length = colIndex - (count1 - 1);
                        for (int index = 0; index < length; index++)
                        {
                            Cell cell = new Cell();
                            indexRow.Append(cell);
                        }
                    }
                }
            }
            return sheetData;
        }

        public override void EscreverCabecalho(WorksheetPart worksheetpart, string label, object value, int rowIndex, int colIndex, CellOptions cellOptions)
        {
            
            var sheetData = InitElementsWithIndex(worksheetpart, 0, colIndex);
            var cell = GetCell(sheetData, 0, colIndex);
            cell.DataType = CellValues.InlineString;
            var cellReference = _getCellReference(colIndex, rowIndex + 1);


            cell.CellReference = cellReference;
            InlineString inlineString = new InlineString();
            Text text = new Text();
            text.Text = label;
            inlineString.Append(text);
            cell.Append(inlineString);



            var cellReference1 = _getCellReference(55, rowIndex + 1);
            var cellReference2 = _getCellReference(90, rowIndex + 1);
            var cellReference3 = _getCellReference(326, rowIndex + 1);
            var cellReference4 = _getCellReference(700, rowIndex + 1);

        }

        private Cell GetCell(SheetData sheetData, int rowIndex, int colIndex)
        {
            var rows = sheetData.Elements<Row>();
            if((rows.Count() -1) <= rowIndex)
            {
                var row = rows.ToList()[rowIndex];
                if(row != null)
                {
                    var cells = row.Elements<Cell>();
                    if(cells != null && (cells.Count() - 1) <= colIndex)
                    {
                        return cells.ToList()[colIndex];
                    }
                }
            }
            return null;
        }

        public override void EscreverValorNaCelula(WorksheetPart worksheetpart, string label, object value, int rowIndex, int colIndex, CellOptions cellOptions)
        {
            var sheetData = InitElementsWithIndex(worksheetpart, rowIndex, colIndex);
            var cell = GetCell(sheetData, rowIndex, colIndex);
            OpenXmlElement dataObject = null;

            if (value != null)
            {
                
                if ((value is int || value is int?) &&
                    (value is short || value is short?) &&
                    (value is long || value is long?) &&
                    (value is float || value is float?) &&
                    (value is double || value is double?) &&
                    (value is decimal || value is decimal?)
                    )
                {
                    cell.DataType = CellValues.Number;
                    NumberItem numberItem = new NumberItem();

                    Text text = new Text();
                    text.Text = value.ToString();
                    numberItem.Append(text);

                    dataObject = numberItem;

                }
                else if (value is DateTime && value is DateTime?)
                {
                    cell.DataType = CellValues.Number;
                    cell.StyleIndex = 1;
                    dataObject = new CellValue(((DateTime) value).ToOADate().ToString(CultureInfo.InvariantCulture));

                }
                else
                {
                    cell.DataType = CellValues.InlineString;
                    InlineString inlineString = new InlineString();

                    Text text = new Text();
                    text.Text = value.ToString();

                    inlineString.Append(text);
                    dataObject = inlineString;
                }
                cell.Append(dataObject);
            }
            if (cellOptions.Comentario != null)
            {
               
            }
            var cellReference = _getCellReference(colIndex, rowIndex + 1);
            cell.CellReference = cellReference;


        }

        public override ICollection<Sheet> GetSheets(Workbook workbook)
        {
            if(workbook != null && 
                workbook.WorkbookPart != null && 
                workbook.WorkbookPart.Workbook != null)
            {

                return workbook.WorkbookPart.Workbook.Descendants<Sheet>().ToList();
            }
            return null;
        }

        public override WorksheetPart NewFile(string sheetName = "Planilha 1", string path = null)
        {
            SpreadsheetDocument document = SpreadsheetDocument.Create(path, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook);
            this.app = document;
            WorkbookPart workbookPart = document.AddWorkbookPart();
            this.WorkbookPart = workbookPart;
            Workbook workBook = new Workbook();
            workBook.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");

            Sheets sheets1 = new Sheets();
            Sheet sheet1 = new Sheet() {Name = sheetName, SheetId = 1U, Id = "rId1" };
            sheets1.Append(sheet1);
            workBook.Append(sheets1);

            workbookPart.Workbook = workBook;

            this.WorkBook = workBook;
            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>("rId1");

            var stylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();

            stylesPart.Stylesheet = new Stylesheet()
            {
                Fonts = new Fonts(new Font()),
                Fills = new Fills(new Fill()),
                Borders = new Borders(new Border()),
                CellStyleFormats = new CellStyleFormats(new CellFormat()),
                CellFormats = new CellFormats(
                    new CellFormat(),
                    new CellFormat()
                    {
                        NumberFormatId = 14,
                        ApplyNumberFormat = true
                    }
                )                
            };
            return worksheetPart;
        }

        public override Workbook OpenFile(string fullFileName)
        {
            var document = SpreadsheetDocument.Open(fullFileName, false);
            this.app = document;
            this.WorkbookPart = document.WorkbookPart;
            return document.WorkbookPart.Workbook;
        }

        public override void SaveFile(string path)
        {
            if(this.app != null)
            {
                this.app.Save();
            }
        }
        
        private Dictionary<string, string> GerarCabecalho(Row headerRow)
        {
            int count = headerRow.Elements<Cell>().Count();
            Dictionary<string, string> lstCabecalho = new Dictionary<string, string>();

            int index = 0;
            foreach (Cell cell in headerRow.Elements<Cell>())
            {
                var value = GetCellValue(cell);
                if (value != null)
                {
                    var cellReference = cell.CellReference;

                    if (!string.IsNullOrWhiteSpace(cellReference))
                    {
                        var keyReference = new Regex(@"\d").Replace(cellReference.Value, "");
                        lstCabecalho.Add(keyReference, value);
                    }
                }
                index++;
            }

            return lstCabecalho;
        }

        public override void ToDTO<T>(Sheet sheet, ICollection<T> lstDTO, int limitTo = -1)
        {

            if (sheet != null)
            {

                WorksheetPart wsPart = (WorksheetPart) WorkbookPart.GetPartById(sheet.Id);

                if (wsPart != null)
                {
                    IEnumerable<Row> rows = wsPart.Worksheet.Descendants<Row>();

                    var primeiraLinha = rows.FirstOrDefault(); 
                    var cabecalho = GerarCabecalho(primeiraLinha);

                    int rowCount = rows.Count();
                    var restantRows = rows.Skip(1);
                    int rowIndex = 2;

                    foreach (Row r in restantRows)
                    {
                        T dtoObj = Activator.CreateInstance<T>();
                        bool inseriu = false;
                        var cells = r.Elements<Cell>();

                        foreach (var key in cabecalho.Keys)
                        {
                            var head = cabecalho[key];
                            var referenceKey = $"{key}{rowIndex}";
                            var c = cells.Where(x => x.CellReference == referenceKey).FirstOrDefault();
                            if (c != null)
                            {
                                var value = GetCellValue(c);
                                var sucesso = SetValueOnDTO<T>(dtoObj, head, value, c.CellReference);
                                inseriu = (inseriu || sucesso);
                            }

                        }
                        rowIndex++;

                        if (inseriu)
                        {
                            lstDTO.Add(dtoObj);
                        }
                    }
                }
            }
        }

        public string GetCellValue(Cell cell)
        {
            string value = null;
            if (cell != null)
            {
                 value = cell.InnerText;

                if(cell.DataType != null)
                {
                    switch (cell.DataType.Value)
                    {
                        case CellValues.SharedString:

                            var stringTable = this.WorkbookPart
                                .GetPartsOfType<SharedStringTablePart>()
                                .FirstOrDefault();

                            if(stringTable != null)
                            {
                                value = stringTable
                                    .SharedStringTable
                                    .ElementAt(int.Parse(cell.InnerText))
                                    .InnerText;
                            }
                            break;
                    }
                }

            }
            return (!string.IsNullOrWhiteSpace(value)) ? value : null;
                
        }
    }
}
