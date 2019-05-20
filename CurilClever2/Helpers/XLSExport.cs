using System;
using System.Collections.Generic;
using System.Linq;
using OfficeOpenXml;
using System.Threading.Tasks;
using System.IO;
using CurilClever2.Models;
using OfficeOpenXml.Style;
using CsvHelper;

namespace CurilClever2.Helpers
{
  public class XLSExport
  {
    public static Byte[] BuildClientsXLS(List<Client> clients)
    {
      MemoryStream ms = new MemoryStream();

      ExcelPackage ExcelPkg = null;
      ExcelPkg = new ExcelPackage(ms);

      ExcelWorksheet wsSheet1 = ExcelPkg.Workbook.Worksheets.Add("клиенты");
      AddHeader(wsSheet1);
      
      int position = 1;
      foreach (Client client in clients)
      {

        try
        {
          AddClient(wsSheet1, client, position);
          position++;
        }
        catch
        {
        }

      }
      wsSheet1.Protection.IsProtected = false;
      wsSheet1.Protection.AllowSelectLockedCells = false;
      //ExcelPkg.SaveAs(ms);
      //ms.Seek((long)0, SeekOrigin.Begin);
      return ExcelPkg.GetAsByteArray();
    }
    private static void AddClient(ExcelWorksheet wsSheet, Client client, int position)
    {
      int shift = 0;
      wsSheet.Cells[2 + position, 2].Value = client.id.ToString();
      wsSheet.Cells[2 + position, 3].Value = client.FIO.ToString();
      wsSheet.Cells[2 + position, 4].Value = client.Gender.ToString();
      wsSheet.Cells[2 + position, 5].Value = client.DateOfBirthday.ToLongDateString();
      wsSheet.Cells[2 + position, 6].Value = client.PassportData.ToString();
      wsSheet.Cells[2 + position, 7].Value = client.Phone.ToString();
      wsSheet.Cells[2 + position, 8].Value = client.Email.ToString();
    }
    private static void AddHeader(ExcelWorksheet wsSheet)
    {
      wsSheet.Cells["A1:H1"].Merge = true;
      wsSheet.Cells["A1:H1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
      
      wsSheet.Column(1).Width = 5;
      wsSheet.Cells[1, 1].Value = "сводная таблица клиентов по состоянию на " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();

      wsSheet.Column(2).Width = 12;
      wsSheet.Cells[2, 2].Value = "ID клиента";

      wsSheet.Column(3).Width = 30;
      wsSheet.Cells[2, 3].Value = "ФИО клиента";

      wsSheet.Column(4).Width = 16;
      wsSheet.Cells[2, 4].Value = "ПОЛ клиента";

      wsSheet.Column(5).Width = 24;
      wsSheet.Cells[2, 5].Value = "Дата рождения клиента";

      wsSheet.Column(6).Width = 45;
      wsSheet.Cells[2, 6].Value = "Паспортные данные клиента";

      wsSheet.Column(7).Width = 28;
      wsSheet.Cells[2, 7].Value = "контактный телефон клиента";

      wsSheet.Column(8).Width = 26;
      wsSheet.Cells[2, 8].Value = "контактный емейл клиента";

    }

    public static Byte[] GetDbStatsCSV(CleverDBContext db)
    {
      List<Tuple<string, string>> stats = new List<Tuple<string, string>>();
      stats.Add(new Tuple<string, string>("type", "count"));
      stats.Add(new Tuple<string, string>("отели", db.Hotels.Count().ToString()));
      stats.Add(new Tuple<string, string>("клиенты", db.Clients.Count().ToString()));
      stats.Add(new Tuple<string, string>("заявки", db.Orders.Count().ToString()));
      stats.Add(new Tuple<string, string>("коментарии к клиентам", db.ClientComments.Count().ToString()));
      stats.Add(new Tuple<string, string>("коментарии к заявкам", db.OrderComments.Count().ToString()));


      MemoryStream ms = new MemoryStream();

      using (var writer = new StreamWriter(ms))
      using (var csv = new CsvWriter(writer))
      {
        csv.WriteRecords(stats);
      }

      return ms.ToArray();
    }
  }
}
