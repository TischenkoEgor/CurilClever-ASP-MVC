using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CurilClever2.ViewModels;
using CurilClever2.Models;

namespace CurilClever2.Areas.Admin.Controllers
{
  [Area("Admin")]
  [Authorize(Roles = "Admin")]
  public class HomeController : Controller
  {
    CleverDBContext db;
    public HomeController(CleverDBContext _db)
    {
      db = _db;
    }
    public IActionResult Index()
    {
      AdminHomePageViewModel adminHomePageViewModel = new AdminHomePageViewModel();
      adminHomePageViewModel.BuildStat(db);
      return View(adminHomePageViewModel);
    }
  }
}