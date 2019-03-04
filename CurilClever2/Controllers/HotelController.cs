using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurilClever2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurilClever2.Controllers
{
  [Authorize]
  public class HotelController : Controller
  {
    private CleverDBContext db;
    public HotelController(CleverDBContext _db)
    {
      db = _db;
    }
    public IActionResult Index()
    {
      return View(db.Hotels.OrderByDescending(x => x.id).ToList());
    }
    [HttpGet]
    public IActionResult CreateHotel()
    {
      return View();
    }
    [HttpPost]
    public IActionResult CreateHotel(Hotel hotel)
    {
      try
      {
        db.Hotels.Add(hotel);
        db.SaveChanges();
        return RedirectToAction("Index");
      }
      catch (Exception ex)
      {

      }
      return View(hotel);
    }
    [HttpGet]
    public IActionResult EditHotel(int? id)
    {
      if (id == null)
        return RedirectToAction("index");
      Hotel hotel = db.Hotels.Where(h => h.id == id).FirstOrDefault();
      if(hotel == null)
        return RedirectToAction("index");

      return View(hotel);
    }
    [HttpPost]
    public IActionResult EditHotel(Hotel hotel)
    {
      db.Hotels.Update(hotel);
      db.SaveChanges();
      return RedirectToAction("Index");
    }

    public IActionResult DeleteHotel(int? id)
    {
      if (id != null)
      {
        Hotel hotel = db.Hotels.Where(h => h.id == id).FirstOrDefault();
        if (hotel != null)
        {
          db.Hotels.Remove(hotel);
          db.SaveChanges();
        }
      }
      return RedirectToAction("Index");
    }

  }
}