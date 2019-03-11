using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurilClever2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CurilClever2.Controllers
{
  public class OrderController : Controller
  {
    private CleverDBContext db;
    public OrderController(CleverDBContext _db)
    {
      db = _db;
    }
    public IActionResult Index()
    {
      var list = db.Orders.Include((o) => o.Client).Include((o)=>o.Hotel).ToList();
      return View(list);
    }
    [HttpGet]
    public IActionResult CreateOrder()
    {
      ViewBag.Hotels = new SelectList(db.Hotels, "id", "Name");
      ViewBag.Clients = new SelectList(db.Clients, "id", "FIO");
      return View();
    }
    [HttpPost]
    public IActionResult CreateOrder(Order order)
    {
      if (order.BeginTravelDate > order.EndTravelDate)
      {
        ModelState.AddModelError("EndTravelDate", "Дата окончания не может быть раньше даты начала тура");
      }
      if (ModelState.IsValid)
      {
        db.Orders.Add(order);
        order.CreationDate = DateTime.Now;
        db.SaveChanges();
      }
      ViewBag.Hotels = new SelectList(db.Hotels, "id", "Name");
      ViewBag.Clients = new SelectList(db.Clients, "id", "FIO");
      return View(order);
    }
  }
}