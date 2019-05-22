using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurilClever2.Models;
using CurilClever2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CurilClever2.Areas.Admin.Controllers
{
  [Area("Admin")]
  [Authorize(Roles ="Admin")]
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
      IQueryable<User> users = db.Users.Include(u => u.Role);
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
      // проверяем есть ли пользователь с тамким ID в базе
      if (!(from u in db.Users select u.id).Contains(id))
      {
        // если нет, то перенаправляем запрос на главную страницу управления аккаунтами в админке
        return RedirectToAction("Index");
      }
      // формируем вью-модель для редактирования параметров пользователя:
      EditUserViewModel model = new EditUserViewModel();

      //заполняем список ролей из всех ролей какие есть в базе
      model.roles = new SelectList(db.Roles, "Id", "Name");

      //заполняем поьзоватля во вью-моделе 
      model.User = db.Users // загружаем из всех пользователей
        .Include(u => u.Role) // включая вложенный в них объект Роль (с описанием роли)
        .FirstOrDefault(u => u.id == id); // из них всех выбираем первый, у которого id равняется тому, которое мы ищем

      //заполняем значение роли пользователя 
      model.newRole = model.User.RoleId;

      return View(model);
    }
    [HttpPost]
    public IActionResult Edit(EditUserViewModel model)
    {
      // получаем пользователя и записываем в него новую роль
      User user = db.Users.Find(model.User.id);
      user.Role = db.Roles.Find(model.newRole);
      
      // обновляем пользователя в базе и сохраняем изменения в базе
      db.Users.Update(user);
      db.SaveChanges();

      // заново формируем вью-модель
      model.User = user;
      model.roles = new SelectList(db.Roles, "Id", "Name");
      model.newRole = model.User.RoleId;

      return View(model);
    }

    public IActionResult Details(int id)
    {
      User user = db.Users.Include(u => u.Role).FirstOrDefault(u => u.id == id);
      if (user == null)
        RedirectToAction("index");

      return View(user);
    }
  }
}