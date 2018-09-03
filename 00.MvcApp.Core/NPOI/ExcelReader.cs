using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace MvcApp.Core.NPOI
{
    public class ExcelReader
    {
        public static List<T> Read<T>(string fileName, string sheetName, bool isFirstRowColumnName, Func<string[], T> rowBuilder) where T : class
        {
            var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            var list = Read(fs, Path.GetExtension(fileName) == ".xlsx", workbook =>
            {
                var sheet = workbook.GetSheet(sheetName);
                if (sheet == null)
                {
                    throw new Exception("找不到名称为 " + sheetName + " 的sheet");
                }
                return sheet;
            }, isFirstRowColumnName, rowBuilder);
            fs.Close();
            return list;
        }

        public static List<T> Read<T>(string fileName, int sheetIndex, bool isFirstRowColumnName, Func<string[], T> rowBuilder) where T : class
        {
            var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            var list = Read(fs, Path.GetExtension(fileName) == ".xlsx", workbook =>
            {
                var sheet = workbook.GetSheetAt(sheetIndex);
                if (sheet == null)
                {
                    throw new Exception("找不到第 " + (sheetIndex + 1) + " 个sheet");
                }
                return sheet;
            }, isFirstRowColumnName, rowBuilder);
            fs.Close();
            return list;
        }

        public static List<T> Read<T>(HttpPostedFileBase file, int sheetIndex, bool isFirstRowColumnName, Func<string[], T> rowBuilder) where T : class
        {
            return Read(file.InputStream, Path.GetExtension(file.FileName) == ".xlsx", workbook =>
            {
                var sheet = workbook.GetSheetAt(sheetIndex);
                if (sheet == null)
                {
                    throw new Exception("找不到第 " + (sheetIndex + 1) + " 个sheet");
                }
                return sheet;
            }, isFirstRowColumnName, rowBuilder);
        }

        #region private

        private static List<T> Read<T>(Stream fs, bool is2007, Func<IWorkbook, ISheet> getSheet, bool isFirstRowColumnName, Func<string[], T> rowBuilder) where T : class
        {
            IWorkbook workbook = null;
            if (is2007)
            {
                workbook = new XSSFWorkbook(fs);
            }
            else
            {
                workbook = new HSSFWorkbook(fs);
            }
            if (workbook == null)
            {
                throw new Exception("文件错误");
            }

            var sheet = getSheet(workbook);
            var firstRow = sheet.GetRow(0);
            var columnCount = firstRow.LastCellNum;
            var rowCount = sheet.LastRowNum;
            var startRowIndex = isFirstRowColumnName ? 1 : 0;

            var list = new List<T>();

            for (var rowIndex = startRowIndex; rowIndex <= rowCount; rowIndex++)
            {
                var row = sheet.GetRow(rowIndex);
                if (row == null)
                {
                    list.Add(null);
                    continue;
                }
                
                var values = new string[columnCount];
                for (var columnIndex = row.FirstCellNum; columnIndex < columnCount; columnIndex++)
                {
                    var value = row.GetCell(columnIndex);
                    values[columnIndex] = value == null ? null : value.ToString();
                }
                var item = rowBuilder(values);
                list.Add(item);
            }
            fs.Close();
            return list;
        }

        #endregion
    }
}
