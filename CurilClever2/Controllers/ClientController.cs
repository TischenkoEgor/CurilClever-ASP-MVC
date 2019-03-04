using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurilClever2.Models;
using Microsoft.AspNetCore.Mvc;

namespace CurilClever2.Controllers
{
  public class ClientController : Controller
  {
    private CleverDBContext db;
    public ClientController(CleverDBContext _db)
    {
      db = _db;
    }
    public IActionResult Index()
    {
      return View(db.Clients.ToList());
    }
    [HttpGet]
    public IActionResult CreateClient()
    {
      return View();
    }
    [HttpPost]
    public IActionResult CreateClient(Client client)
    {
      db.Clients.Add(client);
      db.SaveChanges();
      return RedirectToAction("index");
    }
    [HttpGet]
    public IActionResult EditClient(int? id)
    {
      if (id == null)
        return RedirectToAction("index");
      Client client = db.Clients.Where(c => c.id == id).FirstOrDefault();
      if (client == null)
        return RedirectToAction("index");
      return View(client);
    }
    [HttpPost]
    public IActionResult EditClient(Client client)
    {
      db.Clients.Update(client);
      db.SaveChanges();
      return RedirectToAction("Index");
    }

    public IActionResult DeleteClient(int? id)
    {
      if (id != null)
      {
        Client client = db.Clients.Where(c => c.id == id).FirstOrDefault();
        if (client != null)
        {
          db.Clients.Remove(client);
          db.SaveChanges();
        }
      }
      return RedirectToAction("Index");
    }
  }
}