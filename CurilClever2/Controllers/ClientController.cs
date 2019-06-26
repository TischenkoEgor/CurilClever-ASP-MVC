using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurilClever2.Models;
using CurilClever2.Utils;
using CurilClever2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CurilClever2.Controllers
{
  [Authorize(Roles = "Admin, Moderator, Manager")]
  public class ClientController : Controller
  {
    private const int ClientsOnPage = 12;
    private CleverDBContext db;
    public ClientController(CleverDBContext _db)
    {
      db = _db;
    }
    public IActionResult Index(int page=1, int noscript = 0)
    {
      #region учет статистики
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
      #endregion

      // если в ссылке стоит параметр noscript  в позиции не 1 (JS включен), то
      if (noscript != 1)
      {
        // просто как обычно возвращаем стандартное вью
        return View();
      }
      // если  noscript в позиции  1 (JS отключен), то формируем набор клиентов в соотвествии с параметром page
      else
      {
        // 0. Фиксируем количество элементов на странице
        int pageSize = ClientsOnPage;
        // 1. Получаем данные о всех клиентах (коллекцию клиентов) из базы данных
        List<Client> clients = db.Clients.ToList();
        // 1.1 Получаем общее количество клиентов
        int count = clients.Count();
        // 2. Получаем обрезанную выборку клиентов для текущей страницы :
        // Для этого в оргинальной коллекции пропускаем (функция Skip) Page-1  страниц по PageSize клиентов на каждой
        // и из оставшихся берем (функция take) pageSize элементов
        List<Client> items = clients.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        // 3. Если так получилось что на последней странице 0 элементов и при этом страниц больше одной
        // -----> такое произойдет, если удалить единственного клиента на последней странице
        if (page > 1 && items.Count == 0)
        {
          // 3.1 Уменьшаем номер страницы на 1
          page--;
          // 3.2 Заново формируем набор клиентов на страницу
          items = clients.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }
        // После того, как выборка клиентов сформирована
        // 4. Создаем объект классса PageViewModel 
        PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
        // 5. Создаем CientPageViewModel используя pageViewModel и список 
        ClientPageViewModel clientPageviewModel = new ClientPageViewModel
        {
          PageViewModel = pageViewModel,
          Clients = items
        };
        // возвращаем специальное вью цельной страницы без поддержки JS
        return View("index_noscript", clientPageviewModel);
      }

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
      // добавляем клиента в базу
      db.Clients.Add(client);
      db.SaveChanges();
      
      //создаем новость о новом клиенте
      News news = new News();

      // краткое описание
      news.TextShort = "добавлен новый клиент";
      // формируем полный текст
      news.TextFull = "В базу данных добавлен новый клиент!Его параметры: <br>";
      news.TextFull += "Имя " + client.FIO + "<br>";
      news.TextFull += "Почта " + client.Email + "<br>";
      news.TextFull += "пол " + client.Gender + "<br>";
      news.TextFull += "телефон " + client.Phone + "<br>";
      news.TextFull += "Дата добавления " + news.Created.ToLongDateString() + " " + news.Created.ToLongTimeString() + "<br>";
      
      // ссылка на созданного клиента
      news.ObjectUrl = "/Client/Details/" + client.id.ToString();

      // сохраняем "автора" новости
      news.User = db.Users // из всех пользователей в базе
        .Where(u => u.Login == User.Identity.Name) // отсеиваем тех, у кого Login такой же как и у ТЕКУЩЕГО АВТОРИЗОВАННОГО ПОЛЬЗОВАТЕЛЯ (User.Identity)
        .FirstOrDefault(); // из них берем первого
      
      //добавляем новость в базу
      db.News.Add(news);
      db.SaveChanges();


      // запускаем рассылку
      // для этого 
     
      // запускаем метод расслки у класа для отправки почты
      MailSender.SendNewsToSubscribers(news, db.Subscribes.Include(s => s.User).ToList());

      
      // перенаправляем к списку со всеми пользователями
      return RedirectToAction("index");
    }

    public IActionResult ClientComments(int id, int page = 1)
    {

      int pageSize = 8;   // количество элементов на странице
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
      // 0. Фиксируем количество элементов на странице
      int pageSize = ClientsOnPage;   
      // 1. Получаем данные о всех клиентах (коллекцию клиентов) из базы данных
      IQueryable<Client> clients = db.Clients;
      // 1.1 Получаем общее количество клиентов
      int count = clients.Count();
      // 2. Получаем обрезанную выборку клиентов для текущей страницы :
      // Для этого в оргинальной коллекции пропускаем (функция Skip) Page-1  страниц по PageSize клиентов на каждой
      // и из оставшихся берем (функция take) pageSize элементов
      List<Client> items = clients.Skip((page - 1) * pageSize).Take(pageSize).ToList();

      // 3. Если так получилось что на последней странице 0 элементов и при этом страниц больше одной
      // -----> такое произойдет, если удалить единственного клиента на последней странице
      if(page > 1 && items.Count == 0)
      {
        // 3.1 Уменьшаем номер страницы на 1
        page--;
        // 3.2 Заново формируем набор клиентов на страницу
        items = clients.Skip((page - 1) * pageSize).Take(pageSize).ToList();
      }
      // После того, как выборка клиентов сформирована
      // 4. Создаем объект классса PageViewModel 
      PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
      // 5. Создаем CientPageViewModel используя pageViewModel и список 
      ClientPageViewModel clientPageviewModel = new ClientPageViewModel
      {
        PageViewModel = pageViewModel,
        Clients = items
      };
      // 6. Возвращаем частичное представление сформированное из viewModel
      return PartialView(clientPageviewModel);
    }
  }
}