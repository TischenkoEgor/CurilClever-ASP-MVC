using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CurilClever2.Models;
using Microsoft.AspNetCore.Authorization;

namespace CurilClever2.Controllers
{
  [Authorize]
  public class HomeController : Controller
  {
    private CleverDBContext db;

    public HomeController(CleverDBContext _db)
    {
      db = _db;
    }
    public IActionResult Index()
    {
      return View();
    }

    public IActionResult Hotels()
    {
      return View(db.Hotels.ToList());
    }
    public IActionResult AddHotel()
    {
      return View();
    }
    public IActionResult Clients()
    {
      return View();
    }

    public IActionResult Orders()
    {
      return View();
    }

    public IActionResult Login()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
