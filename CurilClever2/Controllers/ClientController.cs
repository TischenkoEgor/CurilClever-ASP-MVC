using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurilClever2.Models;
using CurilClever2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    [HttpPost]
    public IActionResult AddClientComment(AddClientComment model)
    {
      Client client = db.Clients.Find(model.clientid);
      ClientComment cc = new ClientComment
      {
        Client = client,
        Posted = DateTime.Now,
        Text = model.comment
      };
      string login = User.Identity.Name;
      cc.User = db.Users.Where(u => u.Login == login).FirstOrDefault();
      db.ClientComments.Add(cc);
      db.SaveChanges();

      return ClientComments(model.clientid);
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

    public IActionResult ClientComments(int id, int page = 1)
    {

      int pageSize = 3;   // количество элементов на странице
      Client client = db.Clients
        .Include(x => x.Comments)
        .AsQueryable().Where(x => x.id == id)
        .FirstOrDefault();


      IQueryable<ClientComment> source;

      if (client.Comments.Count() > 0)
        source = client.Comments.AsQueryable().OrderByDescending(x => x.Posted);
      else
        source = new List<ClientComment>().AsQueryable().OrderByDescending(x=> x.Posted);
      foreach (var coment in source)
      {
        coment.User = db.Users.Find(coment.Userid);
      }

      var count = source.Count();
      var items = source.Skip((page - 1) * pageSize).Take(pageSize).ToList();

      PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
      ClientCommentsViewModel viewModel = new ClientCommentsViewModel
      {
        PageViewModel = pageViewModel,
        Comments = items,
        clientid = id
      };
      return PartialView("ClientComments", viewModel);
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
    public IActionResult Details(int id)
    {
      Client model = db.Clients.Find(id);

      return View(model);
    }

    public IActionResult DeleteClient(int? id, int page = 1)
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

      return RedirectToAction("GetTableOfClients", new {page = page});
    }
    public IActionResult GetTableOfClients(int page = 1)
    {
      int pageSize = 3;   // количество элементов на странице
      IQueryable<Client> source = db.Clients;
      var count = source.Count();
      var items = source.Skip((page - 1) * pageSize).Take(pageSize).ToList();
      if(page > 1 && items.Count == 0)
      {
        items = source.Skip((page - 2) * pageSize).Take(pageSize).ToList();
      }
      PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
      ClientPageViewModel viewModel = new ClientPageViewModel
      {
        PageViewModel = pageViewModel,
        Clients = items
      };
      return PartialView(viewModel);
    }
  }
}