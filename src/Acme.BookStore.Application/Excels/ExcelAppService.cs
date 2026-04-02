using Acme.BookStore.Common;
using Acme.BookStore.Constants;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Content;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;

namespace Acme.BookStore.Excels
{
    public class ExcelAppService : ITransientDependency
    {
        private readonly IClock _clock;

        public ExcelAppService(IClock clock)
        {
            _clock = clock;
        }

        // Export
        private byte[] ExportToByte<T>(List<T> data)
        {
            using (var package = new ExcelPackage())
            {
                var sheet = package.Workbook.Worksheets.Add("Sheet1");

                var props = typeof(T).GetProperties();

                // 👉 Header
                for (int col = 0; col < props.Length; col++)
                {
                    var prop = props[col];

                    var displayAttr = prop.GetCustomAttribute<System.ComponentModel.DisplayNameAttribute>();
                    var header = displayAttr?.DisplayName ?? prop.Name;

                    sheet.Cells[1, col + 1].Value = header;
                    sheet.Cells[1, col + 1].Style.Font.Bold = true;
                    sheet.Cells[1, col + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[1, col + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells[1, col + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[1, col + 1].Style.Fill.BackgroundColor.SetColor(Color.DarkRed);
                    sheet.Cells[1, col + 1].Style.Font.Color.SetColor(Color.White);
                }

                // 👉 Data
                for (int row = 0; row < data.Count; row++)
                {
                    for (int col = 0; col < props.Length; col++)
                    {
                        var value = props[col].GetValue(data[row]);
                        sheet.Cells[row + 2, col + 1].Value = value;
                    }
                }

                for (int row = 1; row <= sheet.Dimension.End.Row; row++)
                {
                    if (sheet.Cells[row, 1, row, sheet.Dimension.End.Column]
                        .Any(c => !string.IsNullOrWhiteSpace(c.Text)))
                    {
                        foreach (var cell in sheet.Cells[row, 1, row, sheet.Dimension.End.Column])
                        {
                            cell.Style.Border.Top.Style =
                            cell.Style.Border.Bottom.Style =
                            cell.Style.Border.Left.Style =
                            cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        }
                    }
                }

                sheet.Cells.AutoFitColumns();

                sheet.Cells.Style.Numberformat.Format = "dd/MM/yyyy";

                return package.GetAsByteArray();
            }
        }

        private string SetFileName(string fileName) => $"{_clock.Now.ToString(DateTimeConstants.DateCultureFormat)}_{fileName}";

        public async Task<IRemoteStreamContent> ExportExcelAsync<T>(List<T> data, string fileName = "download")
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            return new RemoteStreamContent(new MemoryStream(ExportToByte(data)), SetFileName(fileName), ContentTypeConstants.Excel);
        }

        // Import
        public bool IsExcelContentType(IFormFile file)
        {
            //var allowedExtensions = new[] { ".xlsx", ".xls" };
            //var extension = Path.GetExtension(file.FileName).ToLower();

            //return allowedExtensions.Contains(extension);

            var allowedTypes = new[]
            {
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", // .xlsx
                "application/vnd.ms-excel" // .xls
            };

            return allowedTypes.Contains(file.ContentType);
        }

        public async Task<ExcelFileContent> ReadExcelAsync(IFormFile file)
        {
            if (!IsExcelContentType(file))
                throw new UserFriendlyException("INVALID_FILE", "FILE_001", "File không đúng định dạng");

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);

            using var package = new ExcelPackage(stream);
            var sheet = package.Workbook.Worksheets[0];

            List<List<string>> dataRows = new List<List<string>>();

            int colCount = sheet.Dimension.Columns;
            int rowCount = sheet.Dimension.Rows;

            // Read header
            var headers = new List<string>();
            for (int col = 1; col <= colCount; col++)
            {
                headers.Add(sheet.Cells[1, col].Text.Trim());
            }

            // Read data, ignore header
            for (int row = 2; row <= rowCount; row++)
            {
                var rowData = new List<string>();

                for (int col = 1; col <= colCount; col++)
                {
                    var key = headers[col - 1];
                    var value = sheet.Cells[row, col].Text;

                    rowData.Add(sheet.Cells[row, col].Text);
                }

                dataRows.Add(rowData);
            }

            return new ExcelFileContent { Headers = headers, DataRows = dataRows };
        }
    }
}

public class ExcelFileContent
{
    public List<string> Headers { get; set; }
    public List<List<string>> DataRows { get; set; }
}
