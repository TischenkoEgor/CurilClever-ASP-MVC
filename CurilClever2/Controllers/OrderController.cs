﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurilClever2.Models;
using CurilClever2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CurilClever2.Controllers
{
  [Authorize(Roles = "Admin, Moderator, Manager")]
  public class OrderController : Controller
  {
    private CleverDBContext db;
    public OrderController(CleverDBContext _db)
    {
      db = _db;
    }
    public IActionResult Index()
    {
      return View();
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
        return RedirectToAction("Details", new { id = order.id });
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

    public IActionResult GetTableOfOrders(int page = 1)
    {
      // 0. Фиксируем количество элементов на странице
      int pageSize = 3;
      // 1. Получаем данные о всех заявках (коллекцию заявок) из базы данных
      IQueryable<Order> source = db.Orders.Include(o => o.Client).Include(o => o.Hotel);
      // 1.1 Получаем общее количество заявок
      int count = source.Count();
      // 2. Получаем обрезанную выборку заявок :
      // Для этого в оргинальной коллекции пропускаем (функция Skip) Page-1  страниц по PageSize заявок на каждой
      // и из оставшихся берем (функция take) pageSize элементов
      List<Order> items = source.Skip((page - 1) * pageSize).Take(pageSize).ToList();

      // 3. Если так получилось что на последней странице 0 элементов и при этом страниц больше одной
      // такое произойдет, если удалить единственную заявку на последней странице
      if (page > 1 && items.Count == 0)
      {
        // 3.1 Уменьшаем номер страницы на 1
        page--;
        // 3.2 Заново формируем набор клиентов на страницу
        items = source.Skip((page - 1) * pageSize).Take(pageSize).ToList();
      }
      // После того, как выборка заявок сформирована:
      // 4. Создаем PageViewModel 
      PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
      // 5. Создаем CientPageViewModel используя pageViewModel и список 
      OrderPageViewModel viewModel = new OrderPageViewModel
      {
        PageViewModel = pageViewModel,
        Orders = items
      };
      // 6. Возвращаем частичное представление сформированное из viewModel
      return PartialView(viewModel);
    }
  }
}