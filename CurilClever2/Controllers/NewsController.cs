using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CurilClever2.Models;

namespace CurilClever2.Controllers
{
  public class NewsController : Controller
  {
    private readonly CleverDBContext db;

    public NewsController(CleverDBContext context)
    {
      db = context;
    }

    // GET: News
    public async Task<IActionResult> Index()
    {
      var cleverDBContext = db.News.Include(n => n.User);
      return View(await cleverDBContext.ToListAsync());
    }

    // GET: News/Details/5
    public async Task<IActionResult> Details(int? id)
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
      if (id == null)
      {
        return NotFound();
      }

      var news = await db.News
          .Include(n => n.User)
          .FirstOrDefaultAsync(m => m.id == id);
      if (news == null)
      {
        return NotFound();
      }

      return View(news);
    }

    // GET: News/Create
    public IActionResult Create()
    {
      ViewData["Userid"] = new SelectList(db.Users, "id", "id");
      return View();
    }

    // POST: News/Create
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("id,TextShort,TextFull,ObjectUrl,Userid,Created")] News news)
    {
      if (ModelState.IsValid)
      {
        db.Add(news);
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      ViewData["Userid"] = new SelectList(db.Users, "id", "id", news.Userid);
      return View(news);
    }

    // GET: News/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var news = await db.News.FindAsync(id);
      if (news == null)
      {
        return NotFound();
      }
      ViewData["Userid"] = new SelectList(db.Users, "id", "id", news.Userid);
      return View(news);
    }

    // POST: News/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("id,TextShort,TextFull,ObjectUrl,Userid,Created")] News news)
    {
      if (id != news.id)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          db.Update(news);
          await db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!NewsExists(news.id))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }
        return RedirectToAction(nameof(Index));
      }
      ViewData["Userid"] = new SelectList(db.Users, "id", "id", news.Userid);
      return View(news);
    }

    // GET: News/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var news = await db.News
          .Include(n => n.User)
          .FirstOrDefaultAsync(m => m.id == id);
      if (news == null)
      {
        return NotFound();
      }

      return View(news);
    }

    // POST: News/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var news = await db.News.FindAsync(id);
      db.News.Remove(news);
      await db.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    private bool NewsExists(int id)
    {
      return db.News.Any(e => e.id == id);
    }
  }
}
