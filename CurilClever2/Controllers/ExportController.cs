using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CurilClever2.Helpers;
using CurilClever2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CurilClever2.Controllers
{
  [Authorize(Roles = "Admin, Moderator, Manager")]
  public class ExportController : Controller
  {
    private CleverDBContext db;
    public ExportController(CleverDBContext _db)
    {
      db = _db;
    }
    public FileResult Clients()
    {
      Response.Clear();
      Byte[] exportdata = XLSExport.BuildClientsXLS(db.Clients.ToList());
      return File(exportdata, "application/xlsx", "clients.xlsx");
    }
    public FileResult DBStats()
    {
      Response.Clear();
      Byte[] exportdata = XLSExport.GetDbStatsCSV(db);

      return File(exportdata, "text/csv", "data.csv");
    }
  }
}