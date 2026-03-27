using Acme.BookStore.Common;
using Acme.BookStore.Constants;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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

                sheet.Cells.AutoFitColumns();

                return package.GetAsByteArray();
            }
        }

        private string SetFileName(string fileName) => $"{_clock.Now.ToString(DateTimeConstants.DateCultureFormat)}_{fileName}";

        public async Task<IRemoteStreamContent> ExportExcelAsync<T>(List<T> data, string fileName = "download")
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            return new RemoteStreamContent(new MemoryStream(ExportToByte(data)), SetFileName(fileName), ContentTypeConstants.Excel);
        }
    }
}
