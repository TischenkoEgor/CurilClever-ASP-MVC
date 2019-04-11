using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurilClever2.Models;
using CurilClever2.ViewModels;
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

    [HttpPost]
    public IActionResult AddComment(AddOrderComment model)
    {
      Order order = db.Orders.Find(model.orderid);
      OrderComment cc = new OrderComment
      {
        Order = order,
        Posted = DateTime.Now,
        Text = model.comment
      };
      string login = User.Identity.Name;
      cc.User = db.Users.Where(u => u.Login == login).FirstOrDefault();
      db.OrderComments.Add(cc);
      db.SaveChanges();

      return Comments(model.orderid);
    }
    public IActionResult Comments(int id, int page = 1)
    {
      int pageSize = 3;   // количество элементов на странице
      Order order = db.Orders
        .Include(x => x.Comments)
        .AsQueryable().Where(x => x.id == id)
        .FirstOrDefault();

      IQueryable<OrderComment> source;

      if (order.Comments.Count() > 0)
        source = order.Comments.AsQueryable().OrderByDescending(x => x.Posted);
      else
        source = new List<OrderComment>().AsQueryable().OrderByDescending(x => x.Posted);
      foreach (var coment in source)
      {
        coment.User = db.Users.Find(coment.Userid);
      }

      var count = source.Count();
      var items = source.Skip((page - 1) * pageSize).Take(pageSize).ToList();

      PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
      OrderCommentsViewModel viewModel = new OrderCommentsViewModel
      {
        PageViewModel = pageViewModel,
        Comments = items,
        orderid = id
      };
      return PartialView("Comments", viewModel);
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