/* 
using System.Threading.Tasks;
using System.Windows;

namespace VoucherPOS.Services
{
    public interface IUserControlService
    {
        MessageBoxResult ShowMessageBox(string text, string caption, MessageBoxButton messageType);
        Task<(string spCode, string spResult)> OutputExcel(EExcelType excelType, string startDate, string endDate);
    }
}
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ClosedXML.Excel;
using Microsoft.Win32;
using VoucherPOS.Helpers.SystemHelper;

namespace VoucherPOS.Services
{
    public class UserControlService : IUserControlService
    {
        public MessageBoxResult ShowMessageBox(string text, string caption, MessageBoxButton messageType)
        {
            return MessageBox.Show(text, caption, messageType);
        }

        public async Task<(string spCode, string spResult)> OutputExcel(EExcelType excelType, string startDate, string endDate)
        {
            try
            {
                var type = excelType switch
                {
                    EExcelType.Buy => "구매결과",
                    EExcelType.Sale => "판매결과",
                    EExcelType.Use => "사용결과",
                    _ => string.Empty
                };

                using var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("VoucherPOS");

                var list = new List<TestExcel>
                {
                    new()
                    {
                        GiftOrderNumber = "1111",
                        GiftName = "테스트 중..."
                    },
                    new()
                    {
                        GiftOrderNumber = "2222",
                        GiftName = "개발 진행 중..."
                    },
                };

                ws.Cell(1, 1).Value = WindowHelper.GetDisplayName<TestExcel>(i => i.GiftOrderNumber);
                ws.Cell(1, 2).Value = WindowHelper.GetDisplayName<TestExcel>(i => i.GiftName);

                ws.Cell(2, 1).InsertData(list.AsEnumerable());
                ws.Columns().AdjustToContents();

                var fileName = "vPOS" + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx";
                var dialog = new SaveFileDialog
                {
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    Filter = "Excel File (.xlsx) | *.xlsx",
                    DefaultExt = ".xlsx",
                    FileName = fileName
                };

                var isDialog = dialog.ShowDialog();
                if (isDialog == false)
                {
                    return ("1000", "엑셀 출력 취소 : " + type);
                }

                await Task.Run(() => wb.SaveAs(dialog.FileName));

                LogHelper.Logger.Info("엑셀 출력 성공 : " + type + " : " + JsonHelper.ModelToJson(list).LogEncrypt());
                return ("0000", "엑셀 출력 성공 : " + type);
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error("엑셀 출력 오류" + ex.Message);
                return ("9999", "엑셀 출력 오류" + ex.Message);
            }
        }
    }

    public enum EExcelType
    {
        Buy,
        Sale,
        Use
    }

    public class TestExcel
    {
        [DisplayName("상품주문번호")] public string GiftOrderNumber { get; init; } = string.Empty;
        [DisplayName("상품명")] public string GiftName { get; init; } = string.Empty;
    }
}
