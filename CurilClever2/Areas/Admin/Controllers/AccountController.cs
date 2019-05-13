using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurilClever2.Models;
using CurilClever2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CurilClever2.Areas.Admin.Controllers
{
  [Area("Admin")]
  public class AccountController : Controller
  {
    private CleverDBContext db;
    public AccountController(CleverDBContext _db)
    {
      db = _db;
    }
    public IActionResult Index()
    {
      return View();
    }
    public IActionResult GetTableOfUsers(int page = 1)
    {
      // 0. Фиксируем количество элементов на странице
      int pageSize = 12;
      // 1. Получаем данные о всех клиентах (коллекцию клиентов) из базы данных
      IQueryable<User> users = db.Users;
      // 1.1 Получаем общее количество клиентов
      int count = users.Count();
      // 2. Получаем обрезанную выборку клиентов для текущей страницы :
      // Для этого в оргинальной коллекции пропускаем (функция Skip) Page-1  страниц по PageSize клиентов на каждой
      // и из оставшихся берем (функция take) pageSize элементов
      List<User> items = users.Skip((page - 1) * pageSize).Take(pageSize).ToList();

      // 3. Если так получилось что на последней странице 0 элементов и при этом страниц больше одной
      // -----> такое произойдет, если удалить единственного клиента на последней странице
      if (page > 1 && items.Count == 0)
      {
        // 3.1 Уменьшаем номер страницы на 1
        page--;
        // 3.2 Заново формируем набор клиентов на страницу
        items = users.Skip((page - 1) * pageSize).Take(pageSize).ToList();
      }
      // После того, как выборка клиентов сформирована
      // 4. Создаем объект классса PageViewModel 
      PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
      // 5. Создаем CientPageViewModel используя pageViewModel и список 
      UserPageViewModel userPageviewModel = new UserPageViewModel
      {
        PageViewModel = pageViewModel,
        Users = items
      };
      // 6. Возвращаем частичное представление сформированное из viewModel
      return PartialView(userPageviewModel);
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
      if (!(from u in db.Users select u.id).Contains(id))
      {
        return RedirectToAction("Index");
      }

      EditUserViewModel model = new EditUserViewModel();
      model.roles = new SelectList(db.Roles, "Id", "Name");

      model.User = db.Users.Include(u => u.Role).FirstOrDefault(u => u.id == id);
      model.newRole = model.User.RoleId;
      return View(model);
    }
    [HttpPost]
    public IActionResult Edit(EditUserViewModel model)
    {
      User user = db.Users.Find(model.User.id);
      user.Role = db.Roles.Find(model.newRole);
      db.Users.Update(user);
      db.SaveChanges();

      model.User = user;
      model.roles = new SelectList(db.Roles, "Id", "Name");
      model.newRole = model.User.RoleId;

      return View(model);
    }
  }
}