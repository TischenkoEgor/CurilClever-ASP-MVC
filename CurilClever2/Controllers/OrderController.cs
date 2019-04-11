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
      var list = db.Orders.Include((o) => o.Client).Include((o) => o.Hotel).ToList();
      return View(list);
    }
    public IActionResult TableOfOrders()
    {
      var list = db.Orders.Include((o) => o.Client).Include((o) => o.Hotel).ToList();
      return PartialView(list);
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
        return RedirectToAction("Details", new { id=order.id});
      }
      ViewBag.Hotels = new SelectList(db.Hotels, "id", "Name");
      ViewBag.Clients = new SelectList(db.Clients, "id", "FIO");
      return View(order);
    }
    public IActionResult Details(int id)
    {
      Order model = db.Orders
        .Include(o => o.Hotel)
        .Include(o => o.Client)
        .Where(o => o.id == id).FirstOrDefault();
      if (model == null)
        return Index();
      return View(model);
    }
    [HttpGet]
    public IActionResult Edit(int id)
    {
      Order model = db.Orders
        .Include(o => o.Hotel)
        .Include(o => o.Client)
        .Where(o => o.id == id).FirstOrDefault();
      if (model == null)
        return Index();

      ViewBag.Hotels = new SelectList(db.Hotels, "id", "Name");
      ViewBag.Clients = new SelectList(db.Clients, "id", "FIO");

      return View(model);
    }
    [HttpPost]
    public IActionResult Edit(Order model)
    {
      db.Orders.Update(model);
      db.SaveChanges();
      model = db.Orders.Find(model.id);

      ViewBag.Hotels = new SelectList(db.Hotels, "id", "Name");
      ViewBag.Clients = new SelectList(db.Clients, "id", "FIO");
      return View(model);
    }
    public IActionResult Delete(int id)
    {
      Order ord = db.Orders.Where(o => o.id == id).FirstOrDefault();
      if (ord != null)
      {
        db.Orders.Remove(ord);
        db.SaveChanges();
      }
      var list = db.Orders.Include((o) => o.Client).Include((o) => o.Hotel).ToList();
      return PartialView("TableOfOrders", list);
    }
  }
}