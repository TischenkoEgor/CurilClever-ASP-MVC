using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CurilClever2.Models;
using Microsoft.AspNetCore.Authorization;
using CurilClever2.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CurilClever2.Controllers
{
  [Authorize(Roles = "Admin, Moderator, Manager, DefaultUser")]
  public class HomeController : Controller
  {
    private CleverDBContext db;

    public HomeController(CleverDBContext _db)
    {
      db = _db;
    }
   
    public IActionResult Index()
    {
      // УЧЕТ СТАТИСТИКИ ПОСЕЩЕНИЯ СТРАНИЦЫ
      // получаем имя пользователя
      string Username = "anonim";
      // если есть авторизованный пользователь используем его имя
      if (User.Identity.IsAuthenticated) Username = User.Identity.Name;
      // создаем новую запись с этим пользователем
      Visit visit = new Visit(HttpContext.Request.Path, DateTime.Now, Username);
      //добавляем в базу и сохраняем изменения
      db.Visits.Add(visit);
      db.SaveChanges();
      // КОНЕЦ УЧЕТА СТАТИСТИКИ



      HomePageViewModel hpVM = new HomePageViewModel();
      hpVM.Clients = db.Clients.OrderByDescending(x => x.id).Take(10);
      hpVM.Orders = db.Orders
                      .Include(x => x.Hotel)
                      .Include(x => x.Client)
                      .OrderByDescending(x => x.CreationDate)
                      .Take(10);
      hpVM.Hotels = db.Hotels.OrderByDescending(x => x.id).Take(10);

      hpVM.ActiveOrders = from o in db.Orders
                          where o.PayStatus == PayStatus.Paid &&
                                o.BeginTravelDate <= DateTime.Now &&
                                o.EndTravelDate >= DateTime.Now
                          select o;
      hpVM.News = db.News.OrderByDescending(n => n.Created).Take(10);


      hpVM.CountOfClientComments = db.ClientComments.Count();
      hpVM.CountOfClients = db.Clients.Count();
      hpVM.CountOfHotels = db.Hotels.Count();
      hpVM.CountOfOrderComments = db.OrderComments.Count();
      hpVM.CountOfOrders = db.Orders.Count();

      return View(hpVM);
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
    [AllowAnonymous]
    public IActionResult AccountInfo()
    {
      return PartialView();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    public IActionResult Privacy()
    {
      return View();
    }
  }
}
